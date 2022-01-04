using C_Our_Souls_DAL.Data;
using C_Our_Souls_DAL.Data.UnitOfWork;
using C_Our_Souls_DAL.Handlers;
using C_Our_Souls_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace C_Our_Souls_WPF.Views
{
    /// <summary>
    /// Interaction logic for ForgotPasswordPopup.xaml
    /// </summary>
    public partial class ForgotPasswordPopup : Window
    {
        public ForgotPasswordPopup()
        {
            InitializeComponent();
        }
        public IUnitOfWork uow = new UnitOfWork(new DatabaseContext());
        private void btSend_Click(object sender, RoutedEventArgs e)
        {
            PasswordHandler pwh = new PasswordHandler();
            EmailHandler emh = new EmailHandler();

            var email = txtEmail.Text;
            var gebruiker = uow.GebruikerRepository.Get(g=>g.Email == email).FirstOrDefault();
            if (gebruiker!=null)
            {
                pwh.Key = gebruiker.Key;
                var newpw = Guid.NewGuid().ToString().Split('-')[0];
                var pw = pwh.Encrypt(newpw);
                gebruiker.Wachtwoord = pw;
                uow.GebruikerRepository.Update(gebruiker);
                uow.Save();
                emh.SendMail(gebruiker.VolledigeNaam, gebruiker.Email, "Wachtwoord vergeten", $"Uw nieuwe wachtwoord is {newpw}. Wijzig dit zo snel mogelijk via uw account installingen in de bib!!");
                var x = new PopUp("Gelukt", "Er is een email verzonden met een nieuw wachtwoord.");
                x.ShowDialog();
                this.Close();
            }
            else
            {
                var x = new PopUp("Foutmelding", "Er bestaat geen account met dit email adres.");
                x.ShowDialog();
            }
        }
    }
}
