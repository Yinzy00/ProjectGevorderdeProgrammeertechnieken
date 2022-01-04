using C_Our_Souls_DAL.Data.UnitOfWork;
using C_Our_Souls_DAL.Models;
using C_Our_Souls_WPF.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Our_Souls_WPF.ViewModels
{
    public class DashboardGebruikerViewModel : BasisViewModel
    {
        #region properties

        private DashboardGebruikerView _v;

        #endregion properties

        #region constructor

        public DashboardGebruikerViewModel(DashboardGebruikerView v)
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
                case "BOEK UITLENEN":
                    Uitlenen();
                    _v.Close();
                    break;

                case "BOEKENBEURS":
                    Boekenbeurs();
                    _v.Close();
                    break;

                case "BOEKENVERKOOP":
                    BoekenVerkoop();
                    _v.Close();
                    break;

                case "BOETES":
                    Boetes();
                    _v.Close();
                    break;

                case "BROWSER":
                    BrowserOpenen();
                    break;

                case "AccountOpties":
                    AccountOpties();
                    break;
            }
        }

        #endregion basisviewmodel

        #region helpersfuncties


        private void AccountOpties()
        {
            AccountPopUpView v = new AccountPopUpView();
            AccountPopUpViewModel vm = new AccountPopUpViewModel(_v);
            v.DataContext = vm;
            v.Show();
        }

        private void BrowserOpenen()
        {
            Process.Start("msedge");
        }

        public void Uitlenen()
        {


            ReserverenBoekGebruikerView v = new Views.ReserverenBoekGebruikerView();
            ReserverenBoekGebruikerViewModel vm = new ReserverenBoekGebruikerViewModel(v);
            v.DataContext = vm;
            v.Show();

        }

        public void Boekenbeurs()
        {

            Views.BoekenbeursGebruiker v = new Views.BoekenbeursGebruiker();
            BoekenbeursGebruikerViewModel vm = new BoekenbeursGebruikerViewModel(v);
            v.DataContext = vm;
            v.Show();

        }

        public void BoekenVerkoop()
        {
            BoekenverkoopGebruikerView v = new BoekenverkoopGebruikerView();
            v.DataContext = new BoekenverkoopGebruikerViewModel(v);
            v.Show();
        }

        public void Boetes()
        {
            OverzichtBoetesView v = new OverzichtBoetesView();
            OverzichtBoetesViewModel vm = new OverzichtBoetesViewModel(v);
            v.DataContext = vm;
            v.Show();
        }


        #endregion helpersfuncties

    }
}