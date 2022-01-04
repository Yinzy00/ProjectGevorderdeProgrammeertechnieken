using C_Our_Souls_DAL.Data;
using C_Our_Souls_DAL.Data.UnitOfWork;
using C_Our_Souls_DAL.Models;
using C_Our_Souls_WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace C_Our_Souls_WPF.ViewModels
{
    public class AccountRegistratieViewModel : BasisViewModel, IDisposable
    {

        #region Eigenschappen

        #region Variables
        private IUnitOfWork _uow = new UnitOfWork(new DatabaseContext());
        private Gebruiker _gGebruiker; //Globale variabele gebruiker
        private ObservableCollection<Gebruiker> _gebruikers = new ObservableCollection<Gebruiker>();
        private bool _lidmaatschapAanvraagBool;
        protected AccountView _v;
        #endregion Variables

        public string FormulierTitel
        {
            get
            {
                if (IsNieuweGebruiker)
                {
                    return "REGISTREREN";
                }
                else
                {
                    return "AANPASSEN";
                };
            }
        }

        public bool IsNieuweGebruiker { get; set; }

        public bool IsBestaandeGebruiker
        {
            get
            {
                return !IsNieuweGebruiker;
            }
        }

        public string Foutmelding { get; set; }

        public int HuidigeGebruikerId { get; set; }

        public string Lidmaatschap
        {
            get
            {
                if (LidTot != null)
                {
                    return $"U bent lid tot {LidTot}";
                }
                return "";
            }
        }

        public DateTime? LidTot { get; set; }

        public List<Lidgeld> Lidgelden { get; set; }

        public bool LidmaatschapAanvraagBool
        {
            get { return _lidmaatschapAanvraagBool; }
            set
            {
                _lidmaatschapAanvraagBool = !_lidmaatschapAanvraagBool;
                if (_lidmaatschapAanvraagBool)
                {
                    LidmaatschapAanvraag = DateTime.Now;
                }
                else
                {
                    LidmaatschapAanvraag = null;
                }
            }
        }

        public DateTime? LidmaatschapAanvraag { get; set; }

        public string HerhalingWachtwoord { get; set; }

        public string Wachtwoord { get; set; }

        public string Email { get; set; }

        public string Woonplaats { get; set; }

        public string Postcode { get; set; }

        public string Adres { get; set; }

        public string Voornaam { get; set; }

        public string Naam { get; set; }

        #endregion Eigenschappen

        #region Constructor
        public AccountRegistratieViewModel(AccountView v)
        {
            _v = v;
            IsNieuweGebruiker = true;
            var g = new Gebruiker();
            g.Naam = Naam;
            g.Voornaam = Voornaam;
            g.Adres = Adres;
            g.Postcode = Postcode;
            g.Woonplaats = Woonplaats;
            g.Email = Email;
            g.LidmaatschapAanvraag = LidmaatschapAanvraag;
            _gGebruiker = g;
            _gebruikers = new ObservableCollection<Gebruiker>(_uow.GebruikerRepository.Get());
        }

        public AccountRegistratieViewModel() {}

        public AccountRegistratieViewModel(int id, AccountView v)
        {
            _v = v;

            IsNieuweGebruiker = false;
            Gebruiker g = _uow.GebruikerRepository.GetById(id);

            Naam = g.Naam;
            Voornaam = g.Voornaam;
            Adres = g.Adres;
            Postcode = g.Postcode;
            Woonplaats = g.Woonplaats;
            Email = g.Email;
            Wachtwoord = ""; //nog unhashen
            Lidgelden = _uow.LidgeldRepository.Get(l => l.GebruikerId == id).ToList();

            if (Lidgelden.Count > 0)
            {
                Lidgeld lg = Lidgelden.LastOrDefault();
                LidTot = lg.DuurLidmaatschap; //LidgeldBetaaldOp.AddYears(lg.DuurLidmaatschap); //Duurlidmaatschap in jaar uitgedrukt
            };
            _gGebruiker = g;
        }
        #endregion Constructor

        #region Methoden
        
        #region ICommand
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            switch (parameter.ToString())
            {
                case "Registreren": Registreren(); break;
                case "Aanpassen": Aanpassen(); break;
                case "Verwijderen": Verwijderen(); break;
                case "Aanmelden": Aanmelden(); break;
                case "MeerInfo": LidgeldInfo(); break;
                case "HomeButton": BackToHome(); break;
                case "AccountOpties": AccountOpties(); break;
            }
        }

        public override string this[string columnName] => throw new NotImplementedException();
        #endregion ICommand

        #region Helperfuncties
        private void AccountOpties()
        {
            AccountPopUpView v = new AccountPopUpView();
            AccountPopUpViewModel vm = new AccountPopUpViewModel(_v);
            v.DataContext = vm;
            v.Show();
        }

        private void BackToHome()
        {
            DashboardGebruikerView view = new DashboardGebruikerView();
            DashboardGebruikerViewModel vm = new DashboardGebruikerViewModel(view);
            view.DataContext = vm;
            view.Show();
            _v.Close();
        }

        private void Aanmelden()
        {
            LoginView view = new LoginView();
            LoginViewModel vm = new LoginViewModel(view);
            view.DataContext = vm;
            view.Show();
            _v.Close();
        }

        public void Dispose()
        {
            _uow?.Dispose();
        }

        public void ClearFields()
        {
            Naam = null;
            Voornaam = null;
            Adres = null;
            Postcode = null;
            Woonplaats = null;
            Email = null;
            Wachtwoord = null;
            HerhalingWachtwoord = null;
            LidmaatschapAanvraagBool = false;
        }

        private void GaNaarDashboardGebruiker()
        {
            DashboardGebruikerView view = new DashboardGebruikerView();
            DashboardGebruikerViewModel vm = new DashboardGebruikerViewModel(view);
            view.DataContext = vm;
            view.Show();
            _v.Close();
        }
        #endregion Helperfuncties

        private void LidgeldInfo()
        {
            PopUp popUp = new PopUp("Info lidmaatschap", $"Een lidmaatschap kost €5.00 voor een periode van 1 jaar.\nGedurende deze periode heeft u volgende voordelen:\n * max 5 boeken uitlenen\n * gratis toegang tot de boekenbeurs");
            popUp.ShowDialog();
        }

        private void Verwijderen()
        {
            if (_uow.GebruikerRepository.Get(g => g.Email == _gGebruiker.Email).FirstOrDefault() != null)
            {
                var response = MessageBox.Show("Bent u zeker dat u uw gegevens wil verwijderen?", "Verwijderen account", MessageBoxButton.YesNo);
                if (response == MessageBoxResult.Yes)
                {
                    _uow.GebruikerRepository.Delete(_gGebruiker);
                    MessageBox.Show("Uw gegevens zijn verwijderd."); //Doorsturen naar het loginscherm
                }
            }
            else
            {
                PopUp p = new PopUp("Foutmelding", "Deze gebruiker is niet geregistreerd", PopUp.PopupButtonOptions.Ok);
                p.ShowDialog();
            }
        }

        private void Aanpassen()
        {
            if (_gGebruiker.IsGeldig())
            {
                if (_uow.GebruikerRepository.Get(g => g.Email == _gGebruiker.Email).FirstOrDefault() != null)
                {
                    _gGebruiker.Wachtwoord = new PasswordHandler(_gGebruiker.Key).Encrypt(Wachtwoord);
                    _gGebruiker.Email = Email;
                    _gGebruiker.Naam = Naam;
                    _gGebruiker.Voornaam = Voornaam;
                    _gGebruiker.Adres = Adres;
                    _gGebruiker.Postcode = Postcode;
                    _gGebruiker.Woonplaats = Woonplaats;
                    _uow.GebruikerRepository.Update(_gGebruiker);
                    int ok = _uow.Save();
                    if (ok > 0)
                    {
                        PopUp p = new PopUp("Account", "Gegevens werden aangepast!", PopUp.PopupButtonOptions.Ok);
                        p.ShowDialog();
                        GaNaarDashboardGebruiker();
                    }
                    else
                    {
                        PopUp p = new PopUp("Foutmelding", "Er ging iets mis, probeer opnieuw!", PopUp.PopupButtonOptions.Ok);
                        p.ShowDialog();
                    }
                }
                else
                {
                    PopUp p = new PopUp("Foutmelding", "Deze gebruiker is niet geregistreerd", PopUp.PopupButtonOptions.Ok);
                    p.ShowDialog();
                }
            }
            else
            {
                PopUp p = new PopUp("Foutmelding", _gGebruiker.Error, PopUp.PopupButtonOptions.Ok);
                p.ShowDialog();
            }
        }

        private void Registreren()
        {
            if (Wachtwoord == HerhalingWachtwoord)
            {
                _gGebruiker.Naam = Naam;
                _gGebruiker.Voornaam = Voornaam;
                _gGebruiker.Adres = Adres;
                _gGebruiker.Postcode = Postcode;
                _gGebruiker.Woonplaats = Woonplaats;
                _gGebruiker.Email = Email;
                _gGebruiker.LidmaatschapAanvraag = LidmaatschapAanvraag;

                if (Wachtwoord != null && Wachtwoord != "")
                {
                    PasswordHandler pwh = new PasswordHandler();
                    if (IsNieuweGebruiker)
                    {
                        _gGebruiker.Key = pwh.Key;
                    }
                    _gGebruiker.Wachtwoord = pwh.Encrypt(Wachtwoord);
                }

                if (_gGebruiker.IsGeldig())
                {
                    if (_uow.GebruikerRepository.Get(g => g.Email == _gGebruiker.Email).FirstOrDefault() == null)
                    {
                        _uow.GebruikerRepository.Add(_gGebruiker);
                        _uow.Save();
                        App.Current.Properties["CurrentUserId"] = _gGebruiker.Id;
                        ClearFields();
                        GaNaarDashboardGebruiker();//Ga naar dashboard na registratie en sluit dit scherm
                    }
                    else
                    {
                        PopUp popUp = new PopUp("Foutmelding", "Deze gebruiker is al geregistreerd");
                        popUp.ShowDialog();
                    }
                }
                else
                {
                    PopUp popUp = new PopUp("Foutmelding", _gGebruiker.Error);
                    popUp.ShowDialog();
                }
            }
            else
            {
                PopUp popUp = new PopUp("Foutmelding", "Wachtwoorden komen niet overeen");
                popUp.ShowDialog();
            }
        }
        
        #endregion Methoden
    }
}