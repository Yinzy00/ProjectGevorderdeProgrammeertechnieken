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
    public class MediumAanmakenViewModel : BasisViewModel
    {
        public IUnitOfWork uow = new UnitOfWork(new DatabaseContext());
        public MediumDetail CurrentMediumDetail { get; set; }
        private MediumAanmakenView _v;
        public override string this[string columnName] => throw new NotImplementedException();

        /// <summary>
        /// Check => Update / Create MediumDetail
        /// </summary>
        public bool IsNew { get; set; } = true;

        /// <summary>
        /// Form title
        /// </summary>
        public string FormulierTitel { get; set; } = "Medium aanmaken";

        /// <summary>
        /// Add extra button content
        /// </summary>
        public string btExtrasToevoegenContent { get; set; }

        //De properties hier onder worden ingevuld doordat de gebruiker data invult.

        #region Private user props

        //Invulvelden
        private string _Title;

        private string _EAN;
        private string _KorteSamenvatting;
        private LeeftijdsKlasse _LeeftijdsCategorie;
        private Soort _TypeMedium;
        private string _BeschrijvingExtra;

        //Listbox of multiselect combobox data
        private ObservableCollection<object> _AddedGenres = new ObservableCollection<object>();

        private ObservableCollection<Medewerker> _AddedMedewerkers = new ObservableCollection<Medewerker>();
        private ObservableCollection<MediumDetailExtra> _AddedMediumDetailExtras = new ObservableCollection<MediumDetailExtra>();

        //SelectedItem
        private Medewerker _ListboxSelectedMedewerker;

        private MediumDetailExtra _SelectedMediumDetailExtra;
        private Extra _SelectedExtra;
        private Medewerker _SelectedMedewerker;

        #endregion Private user props

        #region Public user props

        #region Textvelden data

        /// <summary>
        /// Text uit title textbox
        /// </summary>
        public string Title
        {
            get => _Title; set
            {
                _Title = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Text uit ean textbox
        /// </summary>
        public string EAN
        {
            get => _EAN; set
            {
                _EAN = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Korte samenvatting textbox
        /// </summary>
        public string KorteSamenvatting
        {
            get => _KorteSamenvatting; set
            {
                _KorteSamenvatting = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Inhoud van het beschrijvings veld bij de exta's
        /// </summary>
        public string BeschrijvingExtra
        {
            get => _BeschrijvingExtra; set
            {
                _BeschrijvingExtra = value;
                NotifyPropertyChanged();
            }
        }

        #endregion Textvelden data

        #region Listbox of multiselect combobox collection data

        /// <summary>
        /// Geselecteerde genres in de multi select combobox
        /// </summary>
        public ObservableCollection<object> AddedGenres
        {
            get => _AddedGenres; set
            {
                _AddedGenres = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// //Data in overzicht extras listbox
        /// </summary>
        public ObservableCollection<MediumDetailExtra> AddedMediumDetailExtras
        {
            get => _AddedMediumDetailExtras; set
            {
                _AddedMediumDetailExtras = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// //Alle medewerkers in de medewerker listbox
        /// </summary>
        public ObservableCollection<Medewerker> AddedMedewerkers
        {
            get
            {
                return _AddedMedewerkers;
            }
            set
            {
                _AddedMedewerkers = value;
                NotifyPropertyChanged();
            }
        }

        #endregion Listbox of multiselect combobox collection data

        #region Currently SelectedItem in Listboxes and comboboxes

        /// <summary>
        /// Geselecteerde leeftijds categorie in de eeftijds categorie combobox
        /// </summary>
        public LeeftijdsKlasse LeeftijdsCategorie
        {
            get => _LeeftijdsCategorie; set
            {
                _LeeftijdsCategorie = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Geselecteerde soort in de soort combobox
        /// </summary>
        public Soort TypeMedium
        {
            get => _TypeMedium; set
            {
                _TypeMedium = value;
                //Update extra cmb when soort changed
                if (value != null)
                {
                    Extras = new ObservableCollection<Extra>(uow.ExtraRepository.Get(e => e.Soort.Id == TypeMedium.Id));
                }
                else
                {
                    Extras = new ObservableCollection<Extra>();
                }
                //Clear list of MediumDetailExtra's when soort changes
                AddedMediumDetailExtras = new ObservableCollection<MediumDetailExtra>();
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Geselecteerde medewerker in listbox
        /// </summary>
        public Medewerker ListboxSelectedMedewerker
        {
            get => _ListboxSelectedMedewerker; set
            {
                _ListboxSelectedMedewerker = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Geselecteerde MediumDetailExtra in het listbox overzicht van de extra's
        /// </summary>
        public MediumDetailExtra SelectedMediumDetailExtra
        {
            get => _SelectedMediumDetailExtra; set
            {
                _SelectedMediumDetailExtra = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Geselecteerde Extra in de Extra's combobox
        /// </summary>
        public Extra SelectedExtra
        {
            get => _SelectedExtra; set
            {
                _SelectedExtra = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Geselecteede medewerker in combobox medewerker
        /// </summary>
        public Medewerker SelectedMedewerker
        {
            get => _SelectedMedewerker; set
            {
                _SelectedMedewerker = value;
                NotifyPropertyChanged();
            }
        }

        #endregion Currently SelectedItem in Listboxes and comboboxes

        #endregion Public user props

        //De properties hier onder worden gebruikt om de collections op te vullen

        #region Private collection props

        private ObservableCollection<Extra> _Extras;
        private ObservableCollection<object> _Categories = new ObservableCollection<object>();
        private ObservableCollection<Medewerker> _Medewerkers;
        private ObservableCollection<Soort> _Types;
        private ObservableCollection<LeeftijdsKlasse> _AgeCategories;

        #endregion Private collection props

        #region Public collection props

        /// <summary>
        /// Met deze lijst word de extra's combobox opgevuld
        /// </summary>
        public ObservableCollection<Extra> Extras
        {
            get => _Extras; set
            {
                _Extras = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Met deze lijst word de genre combobox opgevuld
        /// </summary>
        public ObservableCollection<object> Categories
        {
            get => _Categories; set
            {
                _Categories = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Met deze lijst word de medewerkers combobox opgevuld
        /// </summary>
        public ObservableCollection<Medewerker> Medewerkers
        {
            get => _Medewerkers; set
            {
                _Medewerkers = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Met deze lijst word de types combobox opgevuld
        /// </summary>
        public ObservableCollection<Soort> Types
        {
            get => _Types; set
            {
                _Types = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Met deze lijst word de leeftijdscategorie combobox opgevuld
        /// </summary>
        public ObservableCollection<LeeftijdsKlasse> AgeCategories
        {
            get => _AgeCategories; set
            {
                _AgeCategories = value;
                NotifyPropertyChanged();
            }
        }

        #endregion Public collection props

        //Create constructor
        public MediumAanmakenViewModel(MediumAanmakenView v)
        {
            _v = v;
            CurrentMediumDetail = new MediumDetail();
            LoadDataCollections();
        }

        //Update constructor
        public MediumAanmakenViewModel(MediumAanmakenView v, int mediumDetailId)
        {
            _v = v;
            LoadDataCollections();

            var mediumDetail = uow.MediumDetailRepository.Get(
                md => md.Id == mediumDetailId,
                md => md.MediumCategorieen.Select(mc => mc.Categorie),
                md => md.LeeftijdsKlasse,
                md => md.MediumDetailMedewerker.Select(mdm => mdm.Medewerker),
                md => md.MediumDetailExtras.Select(mde=>mde.Extra)
                ).FirstOrDefault();

            if (mediumDetail != null)
            {
                CurrentMediumDetail = mediumDetail;

                Title = mediumDetail.Title;
                EAN = mediumDetail.Ean;
                KorteSamenvatting = mediumDetail.KorteInhoud;
                mediumDetail.MediumCategorieen.ToList().ForEach(md => AddedGenres.Add(md.Categorie));
                LeeftijdsCategorie = AgeCategories.Where(x => x.Id == mediumDetail.LeeftijdsKlasse.Id).FirstOrDefault();
                TypeMedium = Types.Where(x => x.Id == mediumDetail.Soort.Id).FirstOrDefault();
                mediumDetail.MediumDetailMedewerker.ToList().ForEach(md => AddedMedewerkers.Add(md.Medewerker));
                AddedMediumDetailExtras = new ObservableCollection<MediumDetailExtra>(mediumDetail.MediumDetailExtras);
                IsNew = false;
                FormulierTitel = "Medium bewerken";
            }
            else
            {
                var p = new PopUp("Foutmelding", "Er is een fout opgetreden. Probeer het opnieuw.");
                p.ShowDialog();
                _v.Close();
            }
        }

        /// <summary>
        /// Data uit database halen in lijsten steken
        /// </summary>
        public void LoadDataCollections()
        {
            uow.CategorieRepository.Get().ToList().ForEach(x => Categories.Add(x));
            AgeCategories = new ObservableCollection<LeeftijdsKlasse>(uow.LeeftijdsKlasseRepository.Get());
            Types = new ObservableCollection<Soort>(uow.SoortRepository.Get());
            Medewerkers = new ObservableCollection<Medewerker>(uow.MedewerkerRepository.Get());
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            switch (parameter)
            {
                case "Opslaan":
                    Opslaan();
                    break;

                case "MedewerkerToevoegen":
                    MedewerkerToevoegen();
                    break;

                case "MedewerkerBewerken":
                    MedewerkerBewerken();
                    break;

                case "MedewerkerAanmaken":
                    MedewerkerAanmaken();
                    break;

                case "MedewerkerVerwijderen":
                    MedewerkerVerwijderen();
                    break;

                case "ExtraToevoegen":
                    ExtraToevoegen();
                    break;

                case "ExtraEigenschapAanpassen":
                    ExtraEigenschapAanpassen();
                    break;

                case "ExtraEigenschapVerwijderen":
                    ExtraEigenschapVerwijderen();
                    break;

                case "HomeButton": BackToHome(); break;
                case "AccountOpties": AccountOpties(); break;
                case "Terug": Terug(); break;

                default:
                    break;
            }
        }

        private void Terug()
        {
            BoekenbestandView view = new BoekenbestandView();
            BoekenBestandBeheerViewModel vm = new BoekenBestandBeheerViewModel(view);
            view.DataContext = vm;
            view.Show();
            _v.Close();
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

        /// <summary>
        /// Medewerker toevoegen aan mediumDetail lijst
        /// </summary>
        private void MedewerkerToevoegen()
        {
            if (SelectedMedewerker != null)
            {
                AddedMedewerkers.Add(SelectedMedewerker);
                Medewerkers.Remove(SelectedMedewerker);
            }
            else
            {
                var x = new PopUp("Foutmelding", "Selecteer een medewerker!");
                x.ShowDialog();
            }
        }

        /// <summary>
        /// Medewerker verwijderen uit mediumDetail lijst
        /// </summary>
        private void MedewerkerVerwijderen()
        {
            if (ListboxSelectedMedewerker != null)
            {
                Medewerkers.Add(ListboxSelectedMedewerker);
                AddedMedewerkers.Remove(ListboxSelectedMedewerker);
            }
            else
            {
                var x = new PopUp("Foutmelding", "Selecteer een medewerker!");
                x.ShowDialog();
            }
        }

        /// <summary>
        /// MediumDetailExtra maken en toevoegen aan mediumDetail lijst
        /// </summary>
        private void ExtraToevoegen()
        {
            if (SelectedExtra!=null)
            {
                //Create new object
                var mde = new MediumDetailExtra()
                {
                    Beschrijving = BeschrijvingExtra,
                    Extra = SelectedExtra,
                    MediumDetail = CurrentMediumDetail
                };
                //Add to list
                AddedMediumDetailExtras.Add(mde);

                //Remove selected from list
                Extras.Remove(SelectedExtra);

                //Clear fields
                BeschrijvingExtra = "";

                //If updating set button text
                if (btExtrasToevoegenContent == "Opslaan")
                {
                    btExtrasToevoegenContent = "Toevoegen";
                }
            }
            else
            {
                new PopUp("Foutmelding", "Je moet een extra selecteren", PopUp.PopupButtonOptions.Ok).ShowDialog();
            }
        }

        /// <summary>
        /// MediumDetailExtra uit lijst halen en wijzigen
        /// </summary>
        private void ExtraEigenschapAanpassen()
        {
            //Get selected obj from list
            var mde = AddedMediumDetailExtras.Where(_mde => _mde == SelectedMediumDetailExtra).FirstOrDefault();
            //Add extra to list
            Extras.Add(mde.Extra);
            //Set fields data
            SelectedExtra = mde.Extra;
            BeschrijvingExtra = mde.Beschrijving;
            //Set button text
            btExtrasToevoegenContent = "Opslaan";
            //Remove item from list
            AddedMediumDetailExtras.Remove(mde);
        }

        /// <summary>
        /// MediumDetailExtra uit lijst halen en verwijderen
        /// </summary>
        private void ExtraEigenschapVerwijderen()
        {
            if (SelectedMediumDetailExtra != null)
            {
                //Get selected obj from list
                var mde = AddedMediumDetailExtras.Where(_mde => _mde == SelectedMediumDetailExtra).FirstOrDefault();
                //Add extra to list
                Extras.Add(mde.Extra);
                //Remove item from list
                AddedMediumDetailExtras.Remove(mde);
            }
            else
            {
                PopUp popUp = new PopUp("Foutmelding", "U heeft geen detail extra geselecteerd om te verwijderen!");
                popUp.ShowDialog();
            }
        }

        /// <summary>
        /// Nieuwe medewerker aanmaken
        /// </summary>
        private void MedewerkerAanmaken()
        {
            ////open medewerker beheren scherm als dialog.

            MediumMedewerkerView v = new MediumMedewerkerView();
            MediumMedewerkerViewModel vm = new MediumMedewerkerViewModel(v);
            v.DataContext = vm;
            v.ShowDialog();

            ////Reload medewerkers lijst na dat scherm sluit
            Medewerkers = new ObservableCollection<Medewerker>(uow.MedewerkerRepository.Get());
            AddedMedewerkers.ToList().ForEach(am => Medewerkers.Remove(Medewerkers.Where(m => m.Id == am.Id).First()));
        }

        /// <summary>
        /// Bestaande medewerker bewerken
        /// </summary>
        private void MedewerkerBewerken()
        {
            ////open medewerker beheren scherm met parameter (SelectedMedewerker)
            if (SelectedMedewerker != null)
            {
                int id = SelectedMedewerker.Id;
                MediumMedewerkerView v = new MediumMedewerkerView();
                MediumMedewerkerViewModel vm = new MediumMedewerkerViewModel(v, id);
                v.DataContext = vm;
                v.ShowDialog();

                ////Reload medewerkers lijst na dat scherm sluit
                uow = new UnitOfWork(new DatabaseContext());
                Medewerkers = new ObservableCollection<Medewerker>(uow.MedewerkerRepository.Get());
                if (Medewerkers.Count > 0)
                {
                    SelectedMedewerker = Medewerkers.Where(m => m.Id == id).First();
                }
                else
                {
                    SelectedMedewerker = null;
                }

                int teller = 0;
                AddedMedewerkers.ToList().ForEach(am =>
                {
                    var curm = Medewerkers.ToList().Where(m => m.Id == am.Id).First();
                    AddedMedewerkers[teller] = curm;
                    Medewerkers.Remove(curm);
                    teller++;
                });
            }
            else
            {
                new PopUp("Foutmelding", "Selecteer een medewerker!").ShowDialog();
            }
        }

        /// <summary>
        /// MediumDetail opslaan
        /// </summary>
        private void Opslaan()
        {
            SetCurrentMediumDetailData();
            if (CurrentMediumDetail.IsGeldig())
            {
                if (IsNew)
                {
                    uow.MediumDetailRepository.Add(CurrentMediumDetail);
                    var exemplaar = new Medium()
                    {
                        MediumDetail = CurrentMediumDetail,
                        EindeLevensduur = DateTime.Now.AddYears(5),
                        Verkoopprijs = 5,
                        Registratie = DateTime.Now
                    };
                    CurrentMediumDetail.Medium.Add(exemplaar);
                }
                else
                {
                    uow.MediumDetailRepository.Update(CurrentMediumDetail);
                }
                uow.Save();
                Clear();
            }
            else
            {
                var x = new PopUp("Foutmelding", CurrentMediumDetail.Error);
                x.ShowDialog();
            }
            CurrentMediumDetail = new MediumDetail();
        }

        /// <summary>
        /// Data uit velding in MediumDetail object steken
        /// </summary>
        public void SetCurrentMediumDetailData()
        {
            CurrentMediumDetail.Title = Title;
            CurrentMediumDetail.Ean = EAN;
            CurrentMediumDetail.KorteInhoud = KorteSamenvatting;
            CurrentMediumDetail.LeeftijdsKlasse = LeeftijdsCategorie;
            CurrentMediumDetail.Soort = TypeMedium;
            CurrentMediumDetail.MediumDetailExtras = AddedMediumDetailExtras;
            AddedMedewerkers.ToList().ForEach(am =>
            {
                if (!CurrentMediumDetail.MediumDetailMedewerker.Any(mdm => mdm.Medewerker.Id == am.Id))
                {
                    CurrentMediumDetail.MediumDetailMedewerker.Add(new MediumDetailMedewerker()
                    {
                        MedewerkerId = am.Id,
                        MediumDetailId = CurrentMediumDetail.Id
                    }
                    );
                }
            });
            CurrentMediumDetail.MediumDetailMedewerker.ToList().ForEach(mdm =>
            {
                if (!AddedMedewerkers.Any(am=>am.Id == mdm.MedewerkerId))
                {
                    uow.MediumDetailMedewerkerRepository.Delete(_mdm=>_mdm.Id == mdm.Id);
                    CurrentMediumDetail.MediumDetailMedewerker.Remove(mdm);
                }
            });

            AddedGenres.ToList().ForEach(ag => {
                if (!CurrentMediumDetail.MediumCategorieen.Any(mc=>mc.CategorieId == (ag as Categorie).Id))
                {
                    CurrentMediumDetail.MediumCategorieen.Add(new MediumCategorie()
                    {
                        MediumDetailId = CurrentMediumDetail.Id,
                        CategorieId = (ag as Categorie).Id
                    });
                }
            });

            CurrentMediumDetail.MediumCategorieen.ToList().ForEach(mc => {
                if (!AddedGenres.Any(ag=>(ag as Categorie).Id == mc.CategorieId))
                {
                    uow.MediumCategorieRepository.Delete(_mc=>_mc.Id == mc.Id);
                    CurrentMediumDetail.MediumCategorieen.Remove(mc);
                }
            });
        }

    public void Clear()
        {
            Title = "";
            KorteSamenvatting = "";
            EAN = "";

            SelectedExtra = null;
            SelectedMedewerker = null;
            SelectedMediumDetailExtra = null;
            ListboxSelectedMedewerker = null;

            LeeftijdsCategorie = null;
            TypeMedium = null;

            Categories.Clear();
            AddedGenres = new ObservableCollection<object>();
            AddedMedewerkers = new ObservableCollection<Medewerker>();
            AddedMediumDetailExtras = new ObservableCollection<MediumDetailExtra>();

            LoadDataCollections();
        }
    }
}