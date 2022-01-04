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
    public class MediumMedewerkerViewModel : BasisViewModel
    {
        #region Eigenschappen

        #region variables

        private IUnitOfWork _uow = new UnitOfWork(new DatabaseContext());        
        private string _naam;
        private Functie _geselecteerdeFunctie;
        private ObservableCollection<Functie> _functies = new ObservableCollection<Functie>();
        private int _functieId;
        private MediumMedewerkerView _v;

        #endregion variables

        #region props

        public bool NieuweMedewerker { get; set; }

        public bool KnopVerwijder
        {
            get { return !NieuweMedewerker; }
        }

        public Medewerker CurrentMedewerker { get; set; }

        public ObservableCollection<Functie> Functies
        {
            get { return _functies; }
            set
            {
                _functies = value;
                NotifyPropertyChanged();
            }
        }

        public int FunctieId
        {
            get { return _functieId; }
            set
            {
                _functieId = value;
                NotifyPropertyChanged();
            }
        }

        public string Naam
        {
            get { return _naam; }
            set
            {
                _naam = value;
                NotifyPropertyChanged();
            }
        }

        //SelectedItem van Functie combo
        public Functie GeselecteerdeFunctie
        {
            get { return _geselecteerdeFunctie; }
            set
            {
                _geselecteerdeFunctie = value;
                NotifyPropertyChanged();
            }
        }

        #endregion props

        #endregion Eigenschappen        

        #region constructor

        public MediumMedewerkerViewModel(MediumMedewerkerView v)
        {
            CurrentMedewerker = new Medewerker();
            NieuweMedewerker = true;
            LijstenVullen();
            _v = v;
        }

        public MediumMedewerkerViewModel(MediumMedewerkerView v, int medewerkerId)
        {
            var medewerker = _uow.MedewerkerRepository.Get(m => m.Id == medewerkerId, m => m.Functie).FirstOrDefault();
            CurrentMedewerker = medewerker;
            Naam = medewerker.Naam;
            FunctieId = medewerker.FunctieId;
            GeselecteerdeFunctie = medewerker.Functie;
            NieuweMedewerker = false;
            LijstenVullen();
            GeselecteerdeFunctie = Functies.Where(f => f.Id == medewerker.Functie.Id).FirstOrDefault();
            _v = v;
        }

        #endregion constructor

        #region Methoden

        #region implementatieBasisViewModel

        public override string this[string columnName] => throw new NotImplementedException();

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            switch (parameter.ToString().ToLower())
            {
                case "opslaan": Opslaan(); break;
                case "verwijder": Verwijderen(); break;
                case "terug": Terug(); break;
            }
        }

        #endregion implementatieBasisViewModel

        #region helperfuncties

        private void Terug()
        {
            _v.Close();
        }

        private void LijstenVullen()
        {
            Functies = new ObservableCollection<Functie>(_uow.FunctieRepository.Get());
        }

        private void medewerkerInstellen()
        {
            CurrentMedewerker.Naam = Naam;
            CurrentMedewerker.FunctieId = GeselecteerdeFunctie.Id;
        }

        #endregion helperfuncties

        private void Opslaan()
        {
            if (Naam != null && GeselecteerdeFunctie != null)
            {
                medewerkerInstellen();
                if (NieuweMedewerker)
                {
                    _uow.MedewerkerRepository.Add(CurrentMedewerker);
                }
                else
                {
                    _uow.MedewerkerRepository.Update(CurrentMedewerker);
                }
                _uow.Save();
                _v.Close();
            }
            else
            {
                var x = new PopUp("Foutmelding", "Vul het formulier in!");
                x.ShowDialog();
            }
        }

        private void Verwijderen()
        {
            var result = new PopUp("Bevestiging", $"Weet u zeker dat u '{CurrentMedewerker.Naam} ({CurrentMedewerker.Functie.Naam})' wil verwijderen?", PopUp.PopupButtonOptions.OkCancel, "Ja", "Nee");
            result.ShowDialog();

            if (result.DialogResult == PopUpResponse.Ok)
            {
                string medewerkerNaam = $"{CurrentMedewerker.Naam} ({CurrentMedewerker.Functie.Naam})";
                _uow.MedewerkerRepository.Delete(CurrentMedewerker);
                _uow.Save();
                PopUp p = new PopUp("Verwijderd", $"Medewerker '{medewerkerNaam}' is verwijderd!");
                p.ShowDialog();
                _v.Close();
            }
            else
            {
                PopUp p = new PopUp("Geannuleerd", $"Actie is geannuleerd! Medewerker '{CurrentMedewerker.Naam} ({CurrentMedewerker.Functie.Naam})' is niet verwijderd!");
                p.ShowDialog();
            }
        }

        #endregion Methoden
    }
}