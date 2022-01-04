using C_Our_Souls_DAL.Data;
using C_Our_Souls_DAL.Data.UnitOfWork;
using C_Our_Souls_DAL.Models;
using C_Our_Souls_WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace C_Our_Souls_WPF.ViewModels
{
    internal class AccountPopUpViewModel : BasisViewModel
    {
        private IUnitOfWork unitOfWork = new UnitOfWork(new DatabaseContext());
        private string _visibility;
        private ObservableCollection<Gebruiker> _gebruikers;
        private Gebruiker g;
        private Window _window;

        public Gebruiker G
        {
            get
            {
                return g;
            }
        }

        public ObservableCollection<Gebruiker> Gebruikers
        {
            get { return _gebruikers; }
            set { _gebruikers = value; }
        }

        public string Visibility
        {
            get { return _visibility; }
            set { _visibility = value; }
        }

        public AccountPopUpViewModel(Window v = null)
        {
            _window = v;

            g = GetCurrentUser();
            //Application.Current.Properties["CurrentUserId"] = 1;
            if (g.Admin == GebruikerType.Beheerder)
            {
                Visibility = "Hidden";
            }
            else
            {
                Visibility = "Visible";
            }
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
                case "Account": Account(); break;
                case "Lidmaatschapaanvraag": Lidmaatschapaanvraag(); break;
                case "Afmelden": Afmelden(); break;
            }
        }

        private void CloseWindow()
        {
            if (_window != null)
            {
                _window.Close();
            }
        }

        private void Afmelden()
        {
            LoginView view = new LoginView();
            LoginViewModel viewModel = new LoginViewModel(view);
            view.DataContext = viewModel;
            view.Show();
            CloseWindow();
        }

        private Gebruiker GetCurrentUser()
        {
            var id = Convert.ToInt32(Application.Current.Properties["CurrentUserId"]);
            return

                unitOfWork.GebruikerRepository
                .Get(gg => gg.Id == id)
                .FirstOrDefault();
        }

        private void Lidmaatschapaanvraag()
        {
            if (g.LidmaatschapAanvraag == null)
            {
                g.LidmaatschapAanvraag = DateTime.Now;
                unitOfWork.GebruikerRepository.Update(g);
                int ok = unitOfWork.Save();
                if (ok > 0)
                {
                    var popup = new PopUp("Lidmaatschap", "Lidmaatschap werd aangevraagd!", PopUp.PopupButtonOptions.Ok);
                    popup.ShowDialog();
                }
                else
                {
                    var popup = new PopUp("Lidmaatschap", "Er ging iets mis, probeer opnieuw!", PopUp.PopupButtonOptions.Ok);
                    popup.ShowDialog();
                }
            }
            else
            {
                var popup = new PopUp("Lidmaatschap", "U heeft al een lidmaatschap of een aanvraag voor lidmaatschap!", PopUp.PopupButtonOptions.Ok);
                popup.ShowDialog();
            }
        }

        private void Account()
        {
            if (g.Admin == GebruikerType.Beheerder)
            {
                AdminAccountView view = new AdminAccountView();
                AdminAccountViewModel viewModel = new AdminAccountViewModel(view);
                view.DataContext = viewModel;
                view.Show();
            }
            else
            {
                AccountView vi = new AccountView();
                AccountRegistratieViewModel vim = new AccountRegistratieViewModel(g.Id, vi);
                vi.DataContext = vim;
                vi.Show();
            }
            CloseWindow();
        }
    }
}