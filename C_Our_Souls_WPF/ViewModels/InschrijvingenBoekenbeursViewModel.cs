using C_Our_Souls_DAL.Data.UnitOfWork;
using C_Our_Souls_DAL.Models;
using C_Our_Souls_WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Our_Souls_WPF.ViewModels
{
    public class InschrijvingenBoekenbeursViewModel : BasisViewModel
    {
        private IUnitOfWork uow = new UnitOfWork(new C_Our_Souls_DAL.Data.DatabaseContext());
        private ObservableCollection<GebruikerBoekenbeurs> _gebruikerboekenbeurs;
        private ObservableCollection<Boekenbeurs> _boekenbeurs;
        private Boekenbeurs _geselecteerdeboekenbeurs;
        private string _foutmelding;
        private InschrijvingenBoekenbeursView _v;
        public override string this[string columnName]
        {
            get { return ""; }
        }

        public InschrijvingenBoekenbeursViewModel(InschrijvingenBoekenbeursView inschrijvingenboekenbeurs)
        {
            Boekenbeurs = new ObservableCollection<Boekenbeurs>(uow.BoekenbeursRepository.Get());
            _v = inschrijvingenboekenbeurs;
        }

        public string BoekenbeursID { get; set; }

        public int TelInschrijvingen { get; set; }

        public Boekenbeurs GeselecteerdeBoekenbeurs
        {
            get { return _geselecteerdeboekenbeurs; }
            set
            {
                _geselecteerdeboekenbeurs = value;
                GeefGebruikersBoekenbeurs();
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<Boekenbeurs> Boekenbeurs
        {
            get { return _boekenbeurs; }
            set
            {
                //update list & sort list by DatumVan reversed new => old
                _boekenbeurs = new ObservableCollection<Boekenbeurs>(value.OrderBy(i => i.DatumVan).Reverse());
                NotifyPropertyChanged();

                //Select beurs with DatumVan closest to today in CBDatumBoekenbeurs
                if (Boekenbeurs.Count > 0)
                {
                    long min = long.MaxValue;
                    foreach (var beurs in _boekenbeurs)
                    {
                        if (Math.Abs(beurs.DatumVan.Ticks - DateTime.Now.Ticks) < min)
                        {
                            min = Math.Abs(beurs.DatumVan.Ticks - DateTime.Now.Ticks);
                            GeselecteerdeBoekenbeurs = beurs;
                        }
                    }
                }
            }
        }

        public string Foutmelding
        {
            get { return _foutmelding; }
            set { _foutmelding = value; NotifyPropertyChanged(); }
        }

        public ObservableCollection<GebruikerBoekenbeurs> GebruikerBoekenbeurs
        {
            get { return _gebruikerboekenbeurs; }
            set
            {
                _gebruikerboekenbeurs = value;
                NotifyPropertyChanged();
            }
        }

        private void GeefGebruikersBoekenbeurs()
        {
            int boekenbeursID = GeselecteerdeBoekenbeurs.Id;
            if (GeselecteerdeBoekenbeurs != null)
            {
                GebruikerBoekenbeurs = new ObservableCollection<GebruikerBoekenbeurs>(uow.GebruikerBoekenbeursRepository.Get(x => x.BoekenbeursId == boekenbeursID, x => x.Gebruiker));
                TelInschrijvingen = GebruikerBoekenbeurs.Count();
            }
            else
            {
                Foutmelding = "Selecteer eerst een boekenbeurs!";
            }
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
                case "AccountOpties": AccountOpties(); break;
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
    }
}