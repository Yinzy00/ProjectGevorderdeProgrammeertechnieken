using C_Our_Souls_DAL.Data.UnitOfWork;
using C_Our_Souls_DAL.Models;
using C_Our_Souls_WPF.ViewModels;
using C_Our_Souls_WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static C_Our_Souls_WPF.Views.PopUp;

namespace C_Our_Souls_WPF.Viewmodels
{
    public class OverzichtBoekenbeursViewModel : BasisViewModel
    {
        private IUnitOfWork uow = new UnitOfWork(new C_Our_Souls_DAL.Data.DatabaseContext());
        private OverzichtBoekenbeursView _v;

        public override string this[string columnName]
        {
            get { return ""; }
        }

        private ObservableCollection<Boekenbeurs> _boekenbeurs;

        private Boekenbeurs _geselecteerdeboekenbeurs;

        private string _foutmelding;

        private string _txtZoeken;

        //private DateTime _beginuur;
        private OverzichtBoekenbeursView overzichtBoekenbeursView;

        public string BoekenbeursID { get; set; }

        public Boekenbeurs GeselecteerdeBoekenbeurs
        {
            get { return _geselecteerdeboekenbeurs; }
            set
            {
                _geselecteerdeboekenbeurs = value;
                NotifyPropertyChanged();
            }
        }

        public string txtZoeken
        {
            get { return _txtZoeken; }
            set { _txtZoeken = value; NotifyPropertyChanged(); }
        }

        public string Foutmelding
        {
            get { return _foutmelding; }
            set { _foutmelding = value; NotifyPropertyChanged(); }
        }

        //public DateTime Beginuur
        //{
        //    get { return _beginuur; }
        //    set{ _beginuur = value; }
        //}

        public ObservableCollection<Boekenbeurs> Boekenbeurs
        {
            get { return _boekenbeurs; }
            set
            {
                _boekenbeurs = value;
                NotifyPropertyChanged();
            }
        }

        public OverzichtBoekenbeursViewModel(OverzichtBoekenbeursView v)
        {
            _v = v;

            Boekenbeurs = new ObservableCollection<Boekenbeurs>(uow.BoekenbeursRepository.Get());
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
                case "Aanmaken": Aanmaken(); break;
                case "Bewerken": Bewerken(); break;
                case "Verwijderen": Verwijderen(); break;
                case "Zoeken": Zoeken(); break;
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

        private void Zoeken()
        {
            Boekenbeurs = new ObservableCollection<Boekenbeurs>(uow.BoekenbeursRepository.Get().Where(x => x.Naam.ToString().Contains(txtZoeken)));
        }

        private void Verwijderen()
        {
            if (GeselecteerdeBoekenbeurs != null)
            {
                var popup = new PopUp("Verwijderen", "Bent u zeker dat u deze boekenbeurs wil verwijderen?\"", PopupButtonOptions.OkCancel);
                popup.ShowDialog();
                if (popup.DialogResult == PopUpResponse.Ok)
                {
                    uow.BoekenbeursRepository.Delete(GeselecteerdeBoekenbeurs);
                    int ok = uow.Save();
                    if (ok > 0)
                    {
                        RefreshBoekenbeurzen();
                    }
                    else
                    {
                        var popup2 = new PopUp("Boekenbeurs", "Er ging iets mis, probeer opnieuw!", PopupButtonOptions.Ok);
                        popup2.ShowDialog();
                    }
                }
            }
            else
            {
                var popup1 = new PopUp("Boekenbeurs", "Gelieve een boekenbeurs te selecteren!", PopupButtonOptions.Ok);
                popup1.ShowDialog();
            }
        }

        public void Wissen()
        {
            GeselecteerdeBoekenbeurs = null;

            Foutmelding = "";
        }

        private void RefreshBoekenbeurzen()
        {
            int boekenbeursID = GeselecteerdeBoekenbeurs.Id;
            List<Boekenbeurs> listBoekenbeurzen = uow.BoekenbeursRepository.Get(x => x.Id == boekenbeursID).ToList();

            Boekenbeurs = new ObservableCollection<Boekenbeurs>(uow.BoekenbeursRepository.Get());
        }

        private void Bewerken()
        {
            if (GeselecteerdeBoekenbeurs != null)
            {
                BoekenbeursAanmakenView aanmakenView = new BoekenbeursAanmakenView();
                BoekenbeursAanmakenViewModel boekenbeursAanmakenViewModel = new BoekenbeursAanmakenViewModel(aanmakenView, GeselecteerdeBoekenbeurs.Id);
                aanmakenView.DataContext = boekenbeursAanmakenViewModel;
                aanmakenView.Show();
                _v.Close();
            }
            else
            {
                var popup = new PopUp("Boekenbeurs", "Gelieve een boekenbeurs te selecteren!", PopupButtonOptions.Ok);
                popup.ShowDialog();
            }
        }

        private void Aanmaken()
        {
            BoekenbeursAanmakenView boekenbeursAanmakenView = new BoekenbeursAanmakenView();
            BoekenbeursAanmakenViewModel boekenbeursAanmakenViewModel = new BoekenbeursAanmakenViewModel(boekenbeursAanmakenView);
            boekenbeursAanmakenView.DataContext = boekenbeursAanmakenViewModel;
            boekenbeursAanmakenView.Show();
            _v.Close();
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