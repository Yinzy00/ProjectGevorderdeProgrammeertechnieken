using C_Our_Souls_DAL.Data;
using C_Our_Souls_DAL.Data.UnitOfWork;
using C_Our_Souls_DAL.Handlers;
using C_Our_Souls_DAL.Models;
using C_Our_Souls_WPF.Components;
using C_Our_Souls_WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Data.Entity;

namespace C_Our_Souls_WPF.ViewModels
{
    public class BoekenverkoopGebruikerViewModel : BasisViewModel
    {
        public int CurrentuserId;
        private List<CardBoekenverkoper> Cards = new List<CardBoekenverkoper>();
        private BoekenverkoopGebruikerView _v;
        private IUnitOfWork _uow = new UnitOfWork(new DatabaseContext());
        private ObservableCollection<Medium> medium;
        private string _scherm;

        public ObservableCollection<Medium> Medium
        {
            get { return medium; }
            set
            {
                medium = value;
                var ctx = new DatabaseContext();

                IQueryable<Medium> _query = ctx.Set<Medium>()
                  .Include(m => m.MediumDetail.Auteurs)
                  .Include(o => o.MediumDetail.Genres)
                  .Include(o => o.MediumDetail.Soort);
                NotifyPropertyChanged();
            }
        }

        public BoekenverkoopGebruikerViewModel(BoekenverkoopGebruikerView v)
        {
            int amountOfCols = 2;
            _v = v;
            CurrentuserId = (int)App.Current.Properties["CurrentUserId"];
            var dateToCheck = DateTime.Now.AddDays(14);
            var boekenVoorVerkoop = _uow.MediumRepository.Get(m => m.EindeLevensduur >= DateTime.Now && m.EindeLevensduur < dateToCheck, b => b.MediumDetail.MediumDetailMedewerker.Select(mdm => mdm.Medewerker)).OrderBy(x => x.MediumDetail.Title).ToList();

            double rows = Math.Ceiling((double)boekenVoorVerkoop.Count() / amountOfCols);
            for (int i = 0; i < rows; i++)
            {
                v.Grid.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < amountOfCols; i++)
            {
                v.Grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            int teller = 0;
            for (int row = 1; row <= rows; row++)
            {
                for (int col = 0; col < amountOfCols; col++)
                {
                    if (teller < boekenVoorVerkoop.Count)
                    {
                        var currentBoek = boekenVoorVerkoop[teller];
                        //Create card per "boekenbeurs"
                        var component = new CardBoekenverkoper()
                        {
                            Margin = new Thickness(0, 15, 0, 15),
                        };
                        //Set card values
                        component.ID.Content = "boekId_" + currentBoek.Id;
                        component.lblTitel.Text = $"Titel: {currentBoek.MediumDetail.Title} {component.ID.Content}";
                        component.lblAuteur.Text = $"Auteur(s): {currentBoek.MediumDetail.Auteurs}";
                        component.lblPrijs.Text = $"€{currentBoek.Verkoopprijs}";
                        component.btnMeerInfo.Name = "btnMeerInfo_" + currentBoek.Id;
                        if (_uow.MediumVerkoopRepository.Get(mv => mv.GebruikerId == CurrentuserId && mv.MediumId == currentBoek.Id).FirstOrDefault() != null)
                        {
                            component.btnInschrijven.Content = "annuleren";
                        }
                        //Add card to list of cards => to be able to use them later
                        Cards.Add(component);
                        //Set card location on grid
                        v.Grid.Children.Add(component);
                        Grid.SetRow(component, row - 1);
                        Grid.SetColumn(component, col);
                        teller++;
                    }
                }
            }
        }

        public override string this[string columnName] => throw new NotImplementedException();

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            if (parameter.ToString().Contains("boekId_"))
            {
                AankoopRegistreren(Convert.ToInt32(parameter.ToString().Split('_')[1]));
            }
            else if (parameter.ToString().Contains("MeerInfo"))
            {
                MeerInfo(parameter.ToString(), _scherm);
            }
            else
            {
                switch (parameter)
                {
                    case "HomeButton":
                        DashboardGebruikerView dgv = new DashboardGebruikerView();
                        DashboardGebruikerViewModel dgvm = new DashboardGebruikerViewModel(dgv);
                        dgv.DataContext = dgvm;
                        dgv.Show();
                        _v.Close();
                        break;
                    case "AccountOpties": AccountOpties(); break;

                    default: break;
                }
            }
        }
        private void AccountOpties()
        {
            AccountPopUpView v = new AccountPopUpView();
            AccountPopUpViewModel vm = new AccountPopUpViewModel(_v);
            v.DataContext = vm;
            v.Show();
        }

        public void MeerInfo(string btnId, string scherm)
        {
            string[] ID = btnId.Split('_');
            int id = int.Parse(ID[1]);
            _scherm = scherm;

            medium = new ObservableCollection<Medium>(_uow.MediumRepository.Get(o => o.MediumDetail.Soort, p => p.MediumDetail.LeeftijdsKlasse, o => o.MediumDetail.MediumCategorieen.Select(p => p.Categorie)).Where(x => x.Id == id));

            medium = new ObservableCollection<Medium>(_uow.MediumRepository.Get().Where(x => x.Id == id));

            medium.Cast<Medium>().ToArray();
            Medium medium1 = medium[0];

            DetailBoekView detailview = new DetailBoekView();
            DetailBoekViewModel detailvm = new DetailBoekViewModel(detailview, medium1, "BvGebruiker");

            //if (currentCard != null)
            //{
            //MessageBox.Show(medium1.ToString());

            //DetailBoekView detailview = new DetailBoekView();
            //DetailBoekViewModel detailvm = new DetailBoekViewModel(detailview, medium1, "test");

            detailview.DataContext = detailvm;
            detailview.Show();
            _v.Close();

            //  }
        }

        public void AankoopRegistreren(int id)
        {
            var currentCard = Cards.Where(c => c.ID.Content.ToString() == $"boekId_{id}").FirstOrDefault();
            if (currentCard != null)
            {
                if (currentCard.btnInschrijven.Content.ToString() == "Registreren")
                {
                    var p = new PopUp("Boekenverkoop", "Weet u zeker dat u zich voor de aankoop van dit boek wil aanmelden?", PopUp.PopupButtonOptions.OkCancel, "Ja", "Nee");
                    p.ShowDialog();

                    if (p.DialogResult == PopUp.PopUpResponse.Ok)
                    {
                        _uow.MediumVerkoopRepository.Add(new MediumVerkoop()
                        {
                            GebruikerId = CurrentuserId,
                            MediumId = id,
                            IntresseDatum = DateTime.Now
                        });
                        _uow.Save();
                        currentCard.btnInschrijven.Content = "Annuleren";
                    }
                }
                else
                {
                    var p = new PopUp("Boekenverkoop", "Weet u zeker dat uw aanmelding voor de aankoop van dit boek wil annuleren?", PopUp.PopupButtonOptions.OkCancel, "Ja", "Nee");
                    p.ShowDialog();

                    if (p.DialogResult == PopUp.PopUpResponse.Ok)
                    {
                        _uow.MediumVerkoopRepository.Delete(x => x.GebruikerId == CurrentuserId && x.MediumId == id);
                        _uow.Save();
                        currentCard.btnInschrijven.Content = "Registreren";
                    }
                }
            }
        }
    }
}