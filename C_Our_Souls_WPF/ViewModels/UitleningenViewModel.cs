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
using static C_Our_Souls_WPF.Views.PopUp;

namespace C_Our_Souls_WPF.ViewModels
{
    public class UitleningenViewModel : BasisViewModel
    {
        #region Properties

        #region Variables

        private string _zoekCriteria = "";
        private ObservableCollection<Uitlening> _huidigeUitleningen = new ObservableCollection<Uitlening>();
        private ObservableCollection<Medium> _uitleningenHistoriek = new ObservableCollection<Medium>();
        private UitleningView _v;
        private IUnitOfWork _uow = new UnitOfWork(new DatabaseContext());
        private Uitlening _geselecteerdeUitlening = new Uitlening();
        private bool _isHuidigeUitleningen;

        #endregion Variables

        public string FormulierTitel
        {
            get
            {
                if (IsHuidigeUitleningen == true)
                {
                    return "HUIDIGE UITLENINGEN";
                }
                else
                {
                    return "UITLEENHISTORIEK";
                }
            }
        }

        public bool IsUitleenhistoriek
        {
            get
            {
                if (IsHuidigeUitleningen)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public bool IsHuidigeUitleningen
        {
            get { return _isHuidigeUitleningen; }
            set { _isHuidigeUitleningen = value; }
        }

        public Uitlening GeselecteerdeUitlening
        {
            get { return _geselecteerdeUitlening; }
            set { _geselecteerdeUitlening = value; }
        }

        public ObservableCollection<Uitlening> HuidigeUitleningen
        {
            get { return _huidigeUitleningen; }
            set
            {
                _huidigeUitleningen = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<Medium> UitleningenHistoriek
        {
            get { return _uitleningenHistoriek; }
            set
            {
                _uitleningenHistoriek = value;
                NotifyPropertyChanged();
            }
        }

        public string ZoekCriteria
        {
            get { return _zoekCriteria; }
            set
            {
                _zoekCriteria = value;
                NotifyPropertyChanged();
                UitleningenOphalen();
            }
        }

        #endregion Properties

        #region Constructor

        public UitleningenViewModel(UitleningView v, bool HuidigeUitleningen)
        {
            _v = v;
            IsHuidigeUitleningen = HuidigeUitleningen;
            UitleningenOphalen();
        }

        #endregion Constructor

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
                case "mediuminleveren": MediumInleveren(); break;
                case "homebutton": BackToHome(); break;
                case "accountopties": AccountOpties(); break;
            }
        }

        #endregion ICommand

        #region helperfuncties

        private void Resetten()
        {
            ZoekCriteria = "";
            GeselecteerdeUitlening = null;
            UitleningenOphalen();
        }

        private void AccountOpties()
        {
            AccountPopUpView v = new AccountPopUpView();
            AccountPopUpViewModel vm = new AccountPopUpViewModel(_v);
            v.DataContext = vm;
            v.Show();
        }

        private void BackToHome()
        {
            DashboardBeheerderView view = new DashboardBeheerderView();
            DashboardBeheerderViewModel vm = new DashboardBeheerderViewModel(view);
            view.DataContext = vm;
            view.Show();
            _v.Close();
        }

        private void UitleningenOphalen()
        {
            if (IsHuidigeUitleningen)
            {
                if (ZoekCriteria != "")
                {
                    HuidigeUitleningen = new ObservableCollection<Uitlening>(_uow.UitleningRepository.Get((x => x.Binnengebracht == null && x.UitgeleendOp != null), x => x.Gebruiker, x => x.Medium, x => x.Medium.MediumDetail, x => x.Medium.MediumDetail.Soort).Where(x => x.Gebruiker.VolledigeNaam.ToLower().Contains(ZoekCriteria.ToLower())));
                }
                else
                {
                    HuidigeUitleningen = new ObservableCollection<Uitlening>(_uow.UitleningRepository.Get((x => x.Binnengebracht == null && x.UitgeleendOp != null), x => x.Gebruiker, x => x.Medium, x => x.Medium.MediumDetail, x => x.Medium.MediumDetail.Soort));
                }
            }
            else
            {
                if (ZoekCriteria != "")
                {
                    UitleningenHistoriek = new ObservableCollection<Medium>(_uow.MediumRepository.Get(
                        x => x.Uitleningen.Count > 0,
                        x => x.Uitleningen,
                        x => x.MediumDetail
                        ).Where(x => x.Uitleningen.Any(u => u.UitgeleendOp != null)
                        && x.MediumDetail.Title.ToLower().Contains(ZoekCriteria.ToLower())
                        ));
                }
                else
                {
                    UitleningenHistoriek = new ObservableCollection<Medium>(_uow.MediumRepository.Get(
                        x => x.Uitleningen.Count > 0,
                        x => x.Uitleningen,
                        x => x.MediumDetail
                        ).Where(x => x.Uitleningen.Any(u => u.UitgeleendOp != null)).ToList());
                }
            }
        }

        #endregion helperfuncties

        private void MediumInleveren()
        {
            if (GeselecteerdeUitlening != null)
            {
                var result = new PopUp($"{GeselecteerdeUitlening.Medium.MediumDetail.Soort.Naam} innemen", $"Bent u zeker dat u '{GeselecteerdeUitlening.Medium.MediumDetail.Title}' wil innemen?", PopupButtonOptions.OkCancel, "Ja", "Nee");
                result.ShowDialog();

                if (result.DialogResult == PopUpResponse.Ok)
                {
                    GeselecteerdeUitlening.Binnengebracht = DateTime.Now;
                    _uow.UitleningRepository.Update(GeselecteerdeUitlening);
                    _uow.Save();
                    Resetten();
                }
                else
                {
                    PopUp p = new PopUp("Annulatie", $"U heeft de actie om '{GeselecteerdeUitlening.Medium.MediumDetail.Title}' in te nemen geannuleerd");
                    p.ShowDialog();
                    Resetten();
                }
            }
            else
            {
                PopUp p = new PopUp("Foutmelding", "Selecteer een uitlening om deze in te nemen!");
                p.ShowDialog();
            }
        }

        #endregion Methoden
    }
}