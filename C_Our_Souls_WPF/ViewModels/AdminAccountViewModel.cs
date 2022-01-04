using C_Our_Souls_DAL.Data;
using C_Our_Souls_DAL.Data.UnitOfWork;
using C_Our_Souls_DAL.Models;
using C_Our_Souls_WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace C_Our_Souls_WPF.ViewModels
{
    public class AdminAccountViewModel : BasisViewModel, IDisposable
    {
        private IUnitOfWork unitOfWork = new UnitOfWork(new DatabaseContext());
        private AdminAccountView _v;
        private Gebruiker _g;
        private string _wachtwoord;
        private string _herhalingWachtwoord;

        public string HerhalingWachtwoord
        {
            get { return _herhalingWachtwoord; }
            set
            {
                _herhalingWachtwoord = value;
                NotifyPropertyChanged();
            }
        }

        public string Wachtwoord
        {
            get { return _wachtwoord; }
            set
            {
                _wachtwoord = value;
                NotifyPropertyChanged();
            }
        }

        public Gebruiker G
        {
            get
            {
                return _g;
            }
        }

        public AdminAccountViewModel(AdminAccountView v)
        {
            _v = v;
            _g = GetCurrentUser();
            _g.Wachtwoord = "";
            Wachtwoord = _g.Wachtwoord;
            HerhalingWachtwoord = "";
        }

        private Gebruiker GetCurrentUser()
        {
            var id = Convert.ToInt32(Application.Current.Properties["CurrentUserId"]);
            return

                unitOfWork.GebruikerRepository
                .Get(gg => gg.Id == id)
                .FirstOrDefault();
        }

        public override string this[string columnName]
        {
            get { return ""; }
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            switch (parameter.ToString())
            {
                case "HomeButton": Home(); break;
                case "Save": Opslaan(); break;
                case "AccountOpties": AccountOpties(); break;
            }
        }

        private void Opslaan()
        {
            if (Wachtwoord != "")
            {
                if (Wachtwoord == HerhalingWachtwoord)
                {
                    PasswordHandler pwh = new PasswordHandler();
                    _g.Key = pwh.Key;
                    _g.Wachtwoord = pwh.Encrypt(Wachtwoord);
                    if (_g.IsGeldig())
                    {
                        unitOfWork.GebruikerRepository.Update(_g);
                        int ok = unitOfWork.Save();
                        if (ok > 0)
                        {
                            PopUp p = new PopUp("Account", "Uw wachtwoord werd aangepast!", PopUp.PopupButtonOptions.Ok);
                            p.ShowDialog();
                            Home();
                        }
                        else
                        {
                            PopUp p = new PopUp("Foutmelding", "Er ging iets mis, probeer opnieuw!", PopUp.PopupButtonOptions.Ok);
                            p.ShowDialog();
                        }
                    }
                    else
                    {
                        PopUp p = new PopUp("Foutmelding", _g.Error, PopUp.PopupButtonOptions.Ok);
                        p.ShowDialog();
                    }
                }
                else
                {
                    PopUp p = new PopUp("Foutmelding", "Wachtwoorden komen niet overeen!", PopUp.PopupButtonOptions.Ok);
                    p.ShowDialog();
                }
            }
            else
            {
                PopUp p = new PopUp("Foutmelding", "Vul een wachtwoord in!", PopUp.PopupButtonOptions.Ok);
                p.ShowDialog();
            }
        }

        private void AccountOpties()
        {
            AccountPopUpView v = new AccountPopUpView();
            AccountPopUpViewModel vm = new AccountPopUpViewModel(_v);
            v.DataContext = vm;
            v.Show();
        }

        private void Home()
        {
            DashboardBeheerderView dashboardBView = new DashboardBeheerderView();
            DashboardBeheerderViewModel dashboardBViewModel = new DashboardBeheerderViewModel(dashboardBView);
            dashboardBView.DataContext = dashboardBViewModel;
            dashboardBView.Show();
            _v.Close();
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }
    }
}