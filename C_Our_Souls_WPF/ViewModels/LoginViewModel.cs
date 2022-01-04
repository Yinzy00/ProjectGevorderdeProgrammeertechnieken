using C_Our_Souls_DAL.Data;
using C_Our_Souls_DAL.Data.UnitOfWork;
using C_Our_Souls_DAL.Models;
using C_Our_Souls_WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace C_Our_Souls_WPF.ViewModels
{
    public class LoginViewModel : BasisViewModel
    {
        private IUnitOfWork uow = new UnitOfWork(new DatabaseContext());
        public string Email { get; set; }
        public string Password { get; set; }
        public Gebruiker Gebruiker { get; set; } = null;
        public bool IsFilledIn
        {
            get
            {
                if (!string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password))
                    return true;
                else
                    return false;
            }
        }

        public LoginView _v;

        /// <summary>
        ///
        /// </summary>
        /// <param name="v">View linked to current viewmodel</param>
        public LoginViewModel(LoginView v)
        {
            _v = v;
            ////CREATE SUPER USER
            //PasswordHandler pwh = new PasswordHandler();
            //Gebruiker g = new Gebruiker()
            //{
            //    Email = "bibProjectProgrammerenAgile@gmail.com",
            //    Wachtwoord = pwh.Encrypt("Admin_"),
            //    Admin = GebruikerType.Super,
            //    Key = pwh.Key
            //};
            //uow.GebruikerRepository.Add(g);
            //uow.Save();
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
                case "LogIn":
                    Login();
                    break;

                case "Register":
                    //Show register window
                    Register();
                    break;

                case "ForgotPassword":
                    ForgotPassword();
                    break;

                default:
                    break;
            }
        }

        public void ForgotPassword()
        {
            ForgotPasswordPopup x = new ForgotPasswordPopup();
            x.ShowDialog();
            uow = new UnitOfWork(new DatabaseContext());
        }
        public void Register()
        {
            AccountView v = new AccountView();
            AccountRegistratieViewModel vm = new AccountRegistratieViewModel(v);
            v.DataContext = vm;
            v.Show();
            _v.Close();
        }

        public void Login()
        {
            GebruikerType? x = CheckCredentials();
            if (x == null)
            {
                PopUp p = new PopUp("Fout", "Email of wachtwoord is incorrect");
                p.ShowDialog();
            }
            else
            {
                _v.Hide();
                Clear();
                switch (x)
                {
                    case GebruikerType.Gebruiker:
                        DashboardGebruikerView dgv = new DashboardGebruikerView();
                        DashboardGebruikerViewModel dgvm = new DashboardGebruikerViewModel(dgv);
                        if (Gebruiker.getTotaalBoeteBedrag > 0)
                        {
                            new PopUp("Boetes", $"U hebt een boetes openstaan met een totaabedrag van €{Gebruiker.getTotaalBoeteBedrag.ToString()}").Show();

                        }
                        dgv.DataContext = dgvm;
                        dgv.Show();
                        break;
                    case GebruikerType.Beheerder:
                        DashboardBeheerderView dbv = new DashboardBeheerderView();
                        DashboardBeheerderViewModel dbvm = new DashboardBeheerderViewModel(dbv);
                        dbv.DataContext = dbvm;
                        dbv.Show();
                        break;
                    case GebruikerType.Super:
                        SuperUserView sv = new SuperUserView();
                        sv.DataContext = new SuperUserViewModel(sv);
                        sv.Show();
                        break;
                    default:
                        break;
                }
                _v.Close();
            }
        }
        public void Clear()
        {
            Email = "";
            Password = "";
        }
        public GebruikerType? CheckCredentials()
        {
            Gebruiker = uow.GebruikerRepository.Get(g => g.Email == Email, g=>g.Uitleningen).FirstOrDefault();

            if (Gebruiker != null)
            {
                PasswordHandler pwh = new PasswordHandler(Gebruiker.Key);
                var pw = pwh.Encrypt(Password);

                if (pw == Gebruiker.Wachtwoord)
                {
                    App.Current.Properties["CurrentUserId"] = Gebruiker.Id;
                    return Gebruiker.Admin;
                }
            }
            return null;
        }

        //public enum CheckCheckCredentials
        //{
        //    User,
        //    Admin,
        //    Incorrect,
        //}
    }
}