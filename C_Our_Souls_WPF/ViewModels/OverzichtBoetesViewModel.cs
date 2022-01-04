using C_Our_Souls_DAL.Data;
using C_Our_Souls_DAL.Data.UnitOfWork;
using C_Our_Souls_DAL.Models;
using C_Our_Souls_WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.ObjectModel;

namespace C_Our_Souls_WPF.ViewModels
{
    public class OverzichtBoetesViewModel : BasisViewModel
    {
        #region Eigenschappen
        #region variables
        private OverzichtBoetesView _v;
        private IUnitOfWork _uow = new UnitOfWork(new DatabaseContext());
        private Gebruiker _currUser = new Gebruiker();
        private ObservableCollection<Uitlening> _boetes = new ObservableCollection<Uitlening>();
        #endregion variables

        public ObservableCollection<Uitlening> Boetes
        {
            get { return _boetes; }
            set { _boetes = value; }
        }

        #endregion Eigenschappen

        #region constructor

        public OverzichtBoetesViewModel(OverzichtBoetesView v)
        {
            _currUser = GetCurrentUser();
            _v = v;
            BoetesOphalen();
        }

        #endregion constructor

        #region Methoden
        #region ICommand

        public override string this[string columnName] => throw new NotImplementedException();

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            switch (parameter.ToString())
            {
                case "AccountOpties": AccountOpties(); break;
                case "HomeButton": BackToHome(); break;
            }
        }

        #endregion ICommand

        #region helperfuncties

        private Gebruiker GetCurrentUser()
        {
            int CurrId = (int)App.Current.Properties["CurrentUserId"];
            return _uow.GebruikerRepository
                    .Get(gg => gg.Id == CurrId)
                    .FirstOrDefault();
        }

        public void BoetesOphalen()
        {
            Boetes = new ObservableCollection<Uitlening>(_uow.UitleningRepository.Get(x => x.GebruikerId == _currUser.Id, x => x.Medium, x => x.Medium.MediumDetail, x => x.Gebruiker.Lidegelden).Where(x => x.BoeteBetaald == null && x.BoeteBedrag() > 0));
        }

        #endregion helperfuncties

        private void BackToHome()
        {
            DashboardGebruikerView v = new DashboardGebruikerView();
            DashboardGebruikerViewModel vm = new DashboardGebruikerViewModel(v);
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

        #endregion Methoden
    }
}