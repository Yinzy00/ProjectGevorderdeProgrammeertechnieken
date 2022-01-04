using C_Our_Souls_DAL.Data;
using C_Our_Souls_DAL.Data.UnitOfWork;
using C_Our_Souls_DAL.Models;
using C_Our_Souls_WPF.Components;
using C_Our_Souls_WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace C_Our_Souls_WPF.ViewModels
{
    public class DetailBoekViewModel : BasisViewModel
    {

        #region Properties

        private DetailBoekView _v;
        private IUnitOfWork uow = new UnitOfWork(new DatabaseContext());
        private string _scherm;
        private Gebruiker g;

        private LeeftijdsKlasse _leeftijdsklasse;
        private Medium _medium = new Medium();
        private string _omschrijving;
        private Soort _soort;
        private string _titel;
        private string _auteur;
        private string _genre;


        public Gebruiker G
        {
            get
            {
                return g;
            }
        }

        public LeeftijdsKlasse Leeftijdsklasse
        {
            get { return _leeftijdsklasse; }
            set
            {
                _leeftijdsklasse = value;
                NotifyPropertyChanged();
            }
        }

        public Medium Medium
        {
            get { return _medium; }
            set
            {
                _medium = value;
                NotifyPropertyChanged();
            }
        }

        public string Omschrijving
        {
            get { return _omschrijving; }
            set { _omschrijving = value; NotifyPropertyChanged(); }
        }

        public Soort Soort
        {
            get { return _soort; }
            set { _soort = value; NotifyPropertyChanged(); }
        }

        public string Titel
        {
            get { return _titel; }
            set { _titel = value; NotifyPropertyChanged(); }
        }

        public string Genres
        {
            get { return _genre; }
            set { _genre = value; NotifyPropertyChanged(); }
        }

        public string Auteur
        {
            get { return _auteur; }
            set
            {
                _auteur = value;
                NotifyPropertyChanged();
            }
        }


        #endregion Properties

        #region constructor

        public DetailBoekViewModel(DetailBoekView v, Medium boek, string scherm)

        {
            _scherm = scherm;
            _v = v;
            Titel = boek.MediumDetail.Title;
            Auteur = boek.MediumDetail.Auteurs;
            Leeftijdsklasse = boek.MediumDetail.LeeftijdsKlasse;
            Omschrijving = boek.MediumDetail.KorteInhoud;
            Soort = boek.MediumDetail.Soort;
            Genres = boek.MediumDetail.Genres;
        }

        #endregion constructor

        #region basisviewmodel

        public override string this[string columnName] => throw new NotImplementedException();

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            switch (parameter.ToString())
            {
                case "HomeButton": BackToHome(); break;

                case "AccountOpties": AccountOpties(); break;

                case "Terug": Terug(); break;

            }
        }

        #endregion basisviewmodel


        #region helpersfuncties

        private void Terug()
        {
            if (_scherm == "ReserverenGebruiker")
            {
                ReserverenBoekGebruikerView v = new ReserverenBoekGebruikerView();
                ReserverenBoekGebruikerViewModel vm = new ReserverenBoekGebruikerViewModel(v);
                v.DataContext = vm;
                v.Show();
                _v.Close();
            }
            else if (_scherm == "ReserverenBeheerder")
            {
                ReserverenBoekBeheerderView v = new ReserverenBoekBeheerderView();
                ReserverenBoekBeheerderViewModel vm = new ReserverenBoekBeheerderViewModel(v);
                v.DataContext = vm;
                v.Show();
                _v.Close();
            }
            else if (_scherm == "BvGebruiker")
            {
                BoekenverkoopGebruikerView v = new BoekenverkoopGebruikerView();
                BoekenverkoopGebruikerViewModel vm = new BoekenverkoopGebruikerViewModel(v);
                v.DataContext = vm;
                v.Show();
                _v.Close();
            }

        }

        private void AccountOpties()
        {
            AccountPopUpView v = new AccountPopUpView();
            AccountPopUpViewModel vm = new AccountPopUpViewModel();
            v.DataContext = vm;
            v.Show();
        }

        private void BackToHome()
        {

            g = GetCurrentUser();
            //Application.Current.Properties["CurrentUserId"] = 1;
            if (g.Admin == GebruikerType.Beheerder)
            {
                DashboardBeheerderView v = new DashboardBeheerderView();
                DashboardBeheerderViewModel vm = new DashboardBeheerderViewModel(v);
                v.DataContext = vm;
                v.Show();
                _v.Close();
            }
            else
            {
                DashboardGebruikerView v = new DashboardGebruikerView();
                DashboardGebruikerViewModel vm = new DashboardGebruikerViewModel(v);
                v.DataContext = vm;
                v.Show();
                _v.Close();
            }
        }

        private Gebruiker GetCurrentUser()
        {
            var id = Convert.ToInt32(Application.Current.Properties["CurrentUserId"]);
            return

                uow.GebruikerRepository
                .Get(gg => gg.Id == id)
                .FirstOrDefault();
        }

        #endregion helpersfuncties

    }
}