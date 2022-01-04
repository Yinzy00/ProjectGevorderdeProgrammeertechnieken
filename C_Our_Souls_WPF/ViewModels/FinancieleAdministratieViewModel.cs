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

namespace C_Our_Souls_WPF.ViewModels
{
    public class FinancieleAdministratieViewModel : BasisViewModel
    {
        #region Eigenschappen

        #region Variables
        private FinancieleAdministratieView _v;
        private ObservableCollection<Uitlening> _boetes = new ObservableCollection<Uitlening>();
        private string _zoekNaam;
        private IUnitOfWork _uow = new UnitOfWork(new DatabaseContext());
        #endregion Variables

        public ObservableCollection<Uitlening> Boetes
        {
            get { return _boetes; }
            set
            {
                _boetes = value;
                NotifyPropertyChanged();
            }
        }

        public string ZoekNaam
        {
            get { return _zoekNaam; }
            set
            {
                _zoekNaam = value;
                NotifyPropertyChanged();
                Zoeken(_zoekNaam);
            }
        }

        public Uitlening GeselecteerdeBoete { get; set; }

        #endregion Eigenschappen

        #region constructor

        public FinancieleAdministratieViewModel(FinancieleAdministratieView v)
        {
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
            switch (parameter.ToString().ToLower())
            {
                case "betalingregistreren": BetalingRegistreren(); break;
                case "homebutton": BackToHome(); break;
                case "accountopties": AccountOpties(); break;
            }
        }

        public void Dispose()
        {
            _uow?.Dispose();
        }
        #endregion ICommand

        #region helperfuncties

        private void Zoeken(string zoekwaarde)
        {
            if (ZoekNaam != "")
            {
                Boetes = new ObservableCollection<Uitlening>(_uow.UitleningRepository.Get(x => x.Gebruiker, x => x.Gebruiker.Lidegelden).Where(x => x.BoeteBetaald == null && x.BoeteBedrag() > 0 && x.Gebruiker.VolledigeNaam.ToLower().Contains(zoekwaarde.ToLower())));
            }
            else
            {
                BoetesOphalen();
            }
        }

        private void BetalingRegistreren()
        {
            if (GeselecteerdeBoete != null)
            {
                var result = new PopUp("Betaling registratie", $"Bent u zeker dat u de betaling van {GeselecteerdeBoete.Gebruiker.VolledigeNaam} wil registreren?", PopUp.PopupButtonOptions.OkCancel, "Ja", "Nee");
                result.ShowDialog();

                if (result.DialogResult == PopUp.PopUpResponse.Ok)
                {
                    GeselecteerdeBoete.BoeteBetaald = DateTime.Now;
                    DateTime BoeteBetaaldOp = DateTime.Now;
                    _uow.UitleningRepository.Update(GeselecteerdeBoete);
                    _uow.Save();

                    PopUp p = new PopUp("Betaalbevestiging", $"Het bedrag van {GeselecteerdeBoete.BoeteBedrag().ToString("C")} van klant '{GeselecteerdeBoete.Gebruiker.VolledigeNaam}' is geregistreerd als betaald op {BoeteBetaaldOp.ToString("d")}.");
                    p.ShowDialog();

                    ZoekNaam = "";
                    BoetesOphalen();
                }
                else
                {
                    PopUp p = new PopUp("Foutmelding", $"De betaling van {GeselecteerdeBoete.BoeteBedrag().ToString("C")} van klant '{GeselecteerdeBoete.Gebruiker.VolledigeNaam}' is geannuleerd.");
                    p.ShowDialog();
                }
            }
            else
            {
                PopUp popUp = new PopUp("Foutmelding", "Selecteer eerst een boete om de betaling te registreren.");
                popUp.Show();
            }
        }

        private void BackToHome()
        {
            DashboardBeheerderView v = new DashboardBeheerderView();
            DashboardBeheerderViewModel vm = new DashboardBeheerderViewModel(v);
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

        private void BoetesOphalen()
        {
            Boetes = new ObservableCollection<Uitlening>(_uow.UitleningRepository.Get(x => x.Gebruiker, x => x.Gebruiker.Lidegelden).Where(x => x.BoeteBetaald == null && x.BoeteBedrag() > 0));
        }

        #endregion helperfuncties

        #endregion Methoden
    }
}