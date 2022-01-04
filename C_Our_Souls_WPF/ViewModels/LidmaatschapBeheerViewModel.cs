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
    public class LidmaatschapBeheerViewModel : BasisViewModel, IDisposable
    {
        #region Eigenschappen        

        #region Variables
        private IUnitOfWork _uow = new UnitOfWork(new DatabaseContext());
        private ObservableCollection<Gebruiker> _gebruikers = new ObservableCollection<Gebruiker>();
        private Gebruiker _geselecteerdeGebruiker;
        private LidmaatschapBeheerView _v;
        private string _zoekNaam;
        #endregion Variables

        public string ZoekNaam
        {
            get { return _zoekNaam; }
            set
            {
                _zoekNaam = value;
                NotifyPropertyChanged();
                zoekOpNaam(_zoekNaam);
            }
        }

        public Gebruiker GeselecteerdeGebruiker
        {
            get { return _geselecteerdeGebruiker; }
            set
            {
                _geselecteerdeGebruiker = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<Gebruiker> Gebruikers
        {
            get { return _gebruikers; }
            set
            {
                _gebruikers = value;
                NotifyPropertyChanged();
            }
        }

        #endregion Eigenschappen

        #region Constructor

        public LidmaatschapBeheerViewModel(LidmaatschapBeheerView v)
        {
            _v = v;
            Gebruikers = new ObservableCollection<Gebruiker>(_uow.GebruikerRepository.Get(g => g.LidmaatschapAanvraag != null));
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
            switch (parameter.ToString())
            {
                case "Goedkeuren": LidmaatschapBehandelen(parameter.ToString()); break;
                case "Weigeren": LidmaatschapBehandelen(parameter.ToString()); break;
                case "HomeButton": BackToHome(); break;
                case "AccountOpties": AccountOpties(); break;
            }
        }

        public void Dispose()
        {
            _uow?.Dispose();
        }

        #endregion ICommand

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

        private void LidmaatschapBehandelen(string actie)
        {//actie = "goedkeuren" OF "weigeren"
            string bevestiging = "";
            DateTime duurLidmaatschap = DateTime.Now;
            string lidmaatschapTot = "";
            if (GeselecteerdeGebruiker != null)
            {
                var result = new PopUp($"{actie}", $"Weet u zeker dat u het lidmaatschap van '{GeselecteerdeGebruiker.VolledigeNaam}' wilt {actie.ToLower()}?", PopupButtonOptions.OkCancel, "Ja", "Nee");
                result.ShowDialog();
                //    System.Windows.MessageBox.Show($"Weet u zeker dat u het lidmaatschap van '{GeselecteerdeGebruiker.VolledigeNaam}' wilt {actie.ToLower()}?", $"{actie}", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);
                if (result.DialogResult == PopUpResponse.Ok)
                {
                    if (actie.ToLower() == "goedkeuren")
                    {
                        duurLidmaatschap = LidgeldInstellen(GeselecteerdeGebruiker.Id);
                        bevestiging = "goedgekeurd";
                        lidmaatschapTot = $" {GeselecteerdeGebruiker.VolledigeNaam} is lid tot {duurLidmaatschap.ToString("D")}";
                    }
                    else
                    {
                        bevestiging = "geweigerd";
                    }
                    GeselecteerdeGebruiker.LidmaatschapAanvraag = null;
                    _uow.GebruikerRepository.Update(GeselecteerdeGebruiker);
                    _uow.Save();

                    var popup = new PopUp($"Lidmaatschap {bevestiging}", $"Het lidmaatschap voor '{GeselecteerdeGebruiker.VolledigeNaam}' werd {bevestiging}!{lidmaatschapTot}", PopupButtonOptions.Ok);
                    popup.ShowDialog();
                    Reset();
                }
                else
                {
                    var annulatie = new PopUp("Bevestiging", "Actie is geannuleerd", PopupButtonOptions.Ok);
                    annulatie.ShowDialog();
                }
            }
            else
            {
                var p = new PopUp("Selecteer klant", "Selecteer eerst een klant", PopupButtonOptions.Ok);
                p.ShowDialog();
            }
        }

        #region Helperfunctie



        public void zoekOpNaam(string zoekWaarde)
        {
            if (ZoekNaam != "")
            {                
                Gebruikers = new ObservableCollection<Gebruiker>(_uow.GebruikerRepository.Get(x => x.LidmaatschapAanvraag != null).Where(x => x.VolledigeNaam.ToLower().Contains(zoekWaarde.ToLower())));
            }
            else
            {
                Gebruikers = new ObservableCollection<Gebruiker>(_uow.GebruikerRepository.Get(g => g.LidmaatschapAanvraag != null));
            }
        }        

        private void Reset()
        {
            Gebruikers = new ObservableCollection<Gebruiker>(_uow.GebruikerRepository.Get(g => g.LidmaatschapAanvraag != null));
            GeselecteerdeGebruiker = null;
        }

        private DateTime LidgeldInstellen(int gebruiker)
        {
            Lidgeld lg = new Lidgeld();
            lg = new Lidgeld();
            lg.GebruikerId = gebruiker;
            lg.LidgeldBetaaldOp = DateTime.Now;
            lg.DuurLidmaatschap = DateTime.Now.AddYears(1);
            _uow.LidgeldRepository.Add(lg);
            return lg.DuurLidmaatschap;
        }

        #endregion Helperfunctie

        #endregion Methoden
    }
}