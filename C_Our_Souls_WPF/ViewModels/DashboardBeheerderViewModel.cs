using C_Our_Souls_WPF.Viewmodels;
using C_Our_Souls_WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Our_Souls_WPF.ViewModels
{
    public class DashboardBeheerderViewModel : BasisViewModel
    {
        #region properties

        private DashboardBeheerderView _v;

        #endregion properties

        #region constructor

        public DashboardBeheerderViewModel(DashboardBeheerderView v)
        {
            _v = v;
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

                case "Boek Reservaties": ReservatiesBoeken(); break;

                case "Boeken bestand": BoekenBestand(); break;

                case "Aanmaken boekenbeurs": AanmakenBoekenBeurs(); break;

                case "Leden aanvragen": LedenAanvragen(); break;

                case "Huidige uitleengegevens": HuidigeUitleengegevens(); break;

                case "Uitleenhistoriek": UitleenHistoriek(); break;

                case "Overzicht inschrijvingen boekenbeurs": InschrijvingenBoekenBeurs(); break;

                case "Boekenbeurzen overzicht": BoekenbeurzenOverzicht(); break;

                case "Financiële administratie": FinaniceelAdministratie(); break;

                case "AccountOpties": AccountOpties(); break;
            }
        }

        #endregion basisviewmodel


        #region helpersfuncties

        private void BoekenbeurzenOverzicht()
        {
            OverzichtBoekenbeursView v = new OverzichtBoekenbeursView();
            OverzichtBoekenbeursViewModel vm = new OverzichtBoekenbeursViewModel(v);
            v.DataContext = vm;
            v.Show();


            _v.Close();
        }

        private void AccountOpties()
        {
            AccountPopUpView v = new AccountPopUpView();
            AccountPopUpViewModel vm = new AccountPopUpViewModel(_v);
            v.DataContext = vm;
            v.Show();
        }

        public void ReservatiesBoeken()
        {
            UitleenBeheerView v = new UitleenBeheerView();
            UitleenBeheerViewModel vm = new UitleenBeheerViewModel(v);
            v.DataContext = vm;
            v.Show();
            _v.Close();
        }

        public void BoekenBestand()
        {

            BoekenbestandView v = new BoekenbestandView();
            BoekenBestandBeheerViewModel vm = new BoekenBestandBeheerViewModel(v);
            v.DataContext = vm;
            v.Show();

            _v.Close();
        }

        public void AanmakenBoekenBeurs()
        {

            BoekenbeursAanmakenView v = new BoekenbeursAanmakenView();
            BoekenbeursAanmakenViewModel vm = new BoekenbeursAanmakenViewModel(v);
            v.DataContext = vm;
            v.Show();

            _v.Close();
        }

        public void LedenAanvragen()
        {

            LidmaatschapBeheerView v = new LidmaatschapBeheerView();
            LidmaatschapBeheerViewModel vm = new LidmaatschapBeheerViewModel(v);
            v.DataContext = vm ;
            v.Show();

            _v.Close();
        }

        public void HuidigeUitleengegevens()
        {
            UitleningView v = new UitleningView();
            //True geeft de view met huidige uitleningen
            UitleningenViewModel vm = new UitleningenViewModel(v, true);
            v.DataContext = vm;
            v.Show();

            _v.Close();
        }

        public void UitleenHistoriek()
        {
            UitleningView v = new UitleningView();
            //False geeft de view met de uitleenhistoriek

            UitleningenViewModel vm = new UitleningenViewModel(v, false);
            v.DataContext = vm;
            v.Show();

            _v.Close();
        }

        public void InschrijvingenBoekenBeurs()
        {

            InschrijvingenBoekenbeursView v = new InschrijvingenBoekenbeursView();
            InschrijvingenBoekenbeursViewModel vm = new InschrijvingenBoekenbeursViewModel(v);
            v.DataContext = vm;
            v.Show();
            _v.Close();
        }

        public void FinaniceelAdministratie()
        {

            FinancieleAdministratieView v = new FinancieleAdministratieView();
            FinancieleAdministratieViewModel vm = new FinancieleAdministratieViewModel(v);
            v.DataContext = vm;
            v.Show();
            _v.Close();
        }

        #endregion helpersfuncties

    }
}