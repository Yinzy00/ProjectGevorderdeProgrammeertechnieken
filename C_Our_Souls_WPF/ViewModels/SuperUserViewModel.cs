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
    public class SuperUserViewModel : BasisViewModel
    {
        private string _Wachtwoord;
        private string _WachtwoordHerhalen;
        private Gebruiker _Account;
        private Gebruiker _SelectedBeheerder;
        private List<Gebruiker> _BeheerderAccounts = new List<Gebruiker>();

        public List<Gebruiker> BeheerderAccounts
        {
            get { return _BeheerderAccounts; }
            set { _BeheerderAccounts = value; NotifyPropertyChanged(); }
        }

        public string Wachtwoord { get => _Wachtwoord; set { _Wachtwoord = value; NotifyPropertyChanged(); } }
        public string WachtwoordHerhalen { get => _WachtwoordHerhalen; set { _WachtwoordHerhalen = value; NotifyPropertyChanged(); } }
        public Gebruiker Account { get => _Account; set { _Account = value; NotifyPropertyChanged(); } }
        public Gebruiker SelectedBeheerder { get => _SelectedBeheerder; set { _SelectedBeheerder = value; NotifyPropertyChanged(); } }

        public IUnitOfWork Uow = new UnitOfWork(new DatabaseContext());
        public bool IsNew { get; set; } = true;
        public SuperUserView V { get; set; }

        public SuperUserViewModel(SuperUserView v)
        {
            V = v;
            Account = new Gebruiker();
            LoadData();
        }
        public void LoadData()
        {
            BeheerderAccounts = Uow.GebruikerRepository.Get(g => g.Admin == GebruikerType.Beheerder).ToList();
        }

        public override string this[string columnName]
        {
            get
            {
                return "";
            }
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            switch (parameter)
            {
                case "Save":
                    Opslaan();
                    break;
                case "Update":
                    Update();
                    break;
                case "Delete":
                    Delete();
                    break;
                case "Afmelden":
                    LoginView v = new LoginView();
                    v.DataContext = new LoginViewModel(v);
                    v.Show();
                    V.Close();
                    break;
                default:
                    break;
            }
        }
        public void Opslaan()
        {
            //Create account
            PasswordHandler pwh = new PasswordHandler();
            if (!IsNew)
                pwh.Key = Account.Key;
            else
                Account.Admin = GebruikerType.Beheerder;

            if (IsNew)
            {
                if (Wachtwoord == WachtwoordHerhalen)
                {
                    Account.Wachtwoord = pwh.Encrypt(Wachtwoord);
                    Account.Key = pwh.Key;
                }
                else
                {
                    var p = new PopUp("Foutmelding", "De wachtwoorden komen niet overeen.");
                    p.ShowDialog();
                    return;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(Wachtwoord) && Wachtwoord == WachtwoordHerhalen)
                    Account.Wachtwoord = pwh.Encrypt(Wachtwoord);
                //Else => keep old password
            }

            //Check
            if (Account.IsGeldig())
            {
                if (IsNew)
                    Uow.GebruikerRepository.Add(Account);
                else
                    Uow.GebruikerRepository.Update(Account);

                Uow.Save();

                //Clear
                Wachtwoord = "";
                WachtwoordHerhalen = "";
                Account = new Gebruiker();

                LoadData();
                IsNew = true;
            }
            //error message
            else
            {
                var p = new PopUp("Foutmelding", Account.Error);
                p.ShowDialog();
            }
        }
        public void Update()
        {
            IsNew = false;
            if (SelectedBeheerder != null)
                Account = SelectedBeheerder;
            else
                new PopUp("Foutmelding", "Selecteer een beheerder!").ShowDialog();
        }
        public void Delete()
        {
            if (SelectedBeheerder != null)
            {
                Uow.GebruikerRepository.Delete(g => g.Id == SelectedBeheerder.Id);
                Uow.Save();
                LoadData();
            }
            else
                new PopUp("Foutmelding", "Selecteer een beheerder!").ShowDialog();
        }
    }
}
