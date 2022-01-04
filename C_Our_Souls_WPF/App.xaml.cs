using C_Our_Souls_WPF.Views;
using C_Our_Souls_WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using C_Our_Souls_DAL.Handlers;
using C_Our_Souls_DAL.Data.UnitOfWork;
using C_Our_Souls_DAL.Data;
using System.Threading;
using C_Our_Souls_DAL.Models;
using C_Our_Souls_WPF.Viewmodels;

namespace C_Our_Souls_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IUnitOfWork _uow;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            LoginView loginView = new LoginView();
            LoginViewModel vm = new LoginViewModel(loginView);
            loginView.DataContext = vm;
            loginView.Show();

            //Start checking email thread
            Thread checkThread = new Thread(DoChecks);
            checkThread.IsBackground = true;
            checkThread.Start();
        }
        public List<Uitlening> Uitleningen { get; set; }
        protected void DoChecks()
        {
            //Check every 15 mins as long as the program is running.
            while (true)
            {
                _uow = new UnitOfWork(new DatabaseContext());
                Uitleningen = _uow.UitleningRepository.Get(u => u.Gebruiker, u => u.Medium.MediumDetail).ToList();
                //Check and notify fines
                CheckBoetes();
                //Check submission dates, notify approaching ones
                CheckNaderenInleverdatum();
                //Check memberships
                CheckLidmaadschappen();
                //Delete all not accepted reservations older than 2 hours
                CheckReservaties();
                //Check when selection has to be made.
                CheckBoekenverkoop();
                Thread.Sleep(900000);
            }
        }
        protected void CheckBoetes()
        {
            var uitleningen = Uitleningen.Where(u => u.UitgeleendOp != null && u.UitlenenTot < DateTime.Now).ToList();
            uitleningen.ForEach(u =>
            {
                DateTime dateToCheck = new DateTime();
                if (u.LaatsteEmail != null)
                {
                    dateToCheck = (DateTime)u.LaatsteEmail;
                    if (u.BoeteBedrag() > u.BoeteBedrag(dateToCheck))
                    {
                        SendBoeteMail(u);
                    }
                }
                else
                {
                    SendBoeteMail(u);
                }
            });
        }

        protected void SendBoeteMail(Uitlening u)
        {
            string emailBody = $"Beste {u.Gebruiker.VolledigeNaam}, <br/><br/>" +
                        $"Uw boete voor het boek {u.Medium.MediumDetail.Title} loopt op.<br /> <br />" +
                        $"De boete bedraagt momenteel een bedrag van: �{u.BoeteBedrag()}<br /><br />" +
                        $"Met vriendelijke groeten, De bib.";
            new EmailHandler().SendMail(u.Gebruiker.VolledigeNaam, u.Gebruiker.Email, "Boete inleverdatum boek.", emailBody);
            u.LaatsteEmail = DateTime.Now;
            _uow.UitleningRepository.Update(u);
        }
        protected void CheckNaderenInleverdatum()
        {

            var uitleningen = Uitleningen.Where(u => u.UitlenenTot > DateTime.Now && u.UitlenenTot.Value.AddHours(-2) < DateTime.Now).ToList();
            uitleningen.ForEach(u =>
            {
                if (u.LaatsteEmail == null)
                {
                    string emailBody = $"Beste {u.Gebruiker.VolledigeNaam}, <br/><br/>" +
                        $"De inlever datum voor het boek {u.Medium.MediumDetail.Title} nadert. Gelieve het boek optijd binnen te brengen.<br /> <br />" +
                        $"Met vriendelijke groeten, De bib.";
                    new EmailHandler().SendMail(u.Gebruiker.VolledigeNaam, u.Gebruiker.Email, "Inlevertermijn boek", emailBody);
                    u.LaatsteEmail = DateTime.Now;
                    _uow.UitleningRepository.Update(u);
                }
            });
            _uow.Save();
        }
        protected void CheckLidmaadschappen()
        {
            var dateToCheck = DateTime.Now.AddDays(-1);
            var lidgelden = _uow.LidgeldRepository.Get(lg => lg.DuurLidmaatschap >= dateToCheck, lg => lg.Gebruiker).ToList();
            lidgelden.ForEach(lg =>
            {
                bool run = true;
                if (lg.LastEmail!= null)
                {
                    if (lg.LastEmail.Value.Date != DateTime.Now.Date)
                    {
                        run = false;
                    }
                }
                if (run) { 
                    if (lg.DuurLidmaatschap.Date == DateTime.Now.Date)
                    {
                        //if (lg.LastEmail.Date != DateTime.Now.Date)
                        //{
                        new EmailHandler().SendMail(lg.Gebruiker.VolledigeNaam, lg.Gebruiker.Email, "Lidmaatschap bib", "Beste, <br/><br/>Uw ladmaatschap bij de bibliotheek vervalt vandaag. <br/><br/>Met vriendelijke groeten,<br/>De Bib");
                        lg.LastEmail = DateTime.Now;
                        _uow.LidgeldRepository.Update(lg);
                        //}
                    }
                    else if (lg.DuurLidmaatschap.AddMonths(-1).AddDays(-7).Date == DateTime.Now.Date)
                    {
                        //if (lg.LastEmail.Date != DateTime.Now.Date)
                        //{
                        new EmailHandler().SendMail(lg.Gebruiker.VolledigeNaam, lg.Gebruiker.Email, "Lidmaatschap bib", "Beste, <br/><br/>Uw ladmaatschap bij de bibliotheek vervalt binnen een maand en 1 week. <br/><br/>Met vriendelijke groeten,<br/>De Bib");
                        lg.LastEmail = DateTime.Now;
                        _uow.LidgeldRepository.Update(lg);
                        //}
                    }
                }
            });
            _uow.Save();
        }
        protected void CheckReservaties()
        {
            var dateToCheck = DateTime.Now.AddHours(-2);
            var uitleningen = Uitleningen.Where(u => u.UitgeleendOp == null && u.OntleenIntresse < dateToCheck).ToList();
            uitleningen.ForEach(u => _uow.UitleningRepository.Delete(u));
            _uow.Save();
        }
        protected void CheckBoekenverkoop()
        {
            var medium = _uow.MediumRepository.Get(m=>m.EindeLevensduur <= DateTime.Now && m.Verkocht != true).ToList();
            if (medium.Count>0)
            {
                medium.ForEach(m =>
                {
                    var regs = _uow.MediumVerkoopRepository.Get(mv => mv.MediumId == m.Id, mv => mv.Medium.MediumDetail, mv => mv.Gebruiker).ToList();
                    if (regs.Count > 0)
                    {
                        Random rnd = new Random();
                        var chosenNumber = rnd.Next(0, regs.Count);
                        var chosenOne = regs[chosenNumber];
                        _uow.MediumVerkoopRepository.Delete(mv => mv.Id != chosenOne.Id);
                        m.Verkocht = true;
                        EmailHandler emh = new EmailHandler();
                        emh.SendMail(
                            chosenOne.Gebruiker.VolledigeNaam,
                            chosenOne.Gebruiker.Email,
                            $"Verkoop {m.MediumDetail.Title}", $"Beste {chosenOne.Gebruiker.VolledigeNaam},<br/><br/>" +
                            $"U bent geselecteerd voor de verkoopvan het boek \"{chosenOne.Medium.MediumDetail.Title}\".<br/><br/>" +
                            $"Met vriendelijke groeten<br/>" +
                            $"De Bib"
                            );
                        _uow.MediumRepository.Update(m);
                    }
                    else
                    {
                        m.Verkocht = true;
                        _uow.MediumRepository.Update(m);
                    }
                });
                _uow.Save();
            }
        }
    }
}