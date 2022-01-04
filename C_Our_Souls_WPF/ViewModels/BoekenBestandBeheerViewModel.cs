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
using System.Data.Entity;
using static C_Our_Souls_WPF.Views.PopUp;
using System.Windows.Threading;

namespace C_Our_Souls_WPF.ViewModels
{
    public class BoekenBestandBeheerViewModel : BasisViewModel
    {
        #region properties

        private IUnitOfWork uow = new UnitOfWork(new DatabaseContext());
        private BoekenbestandView _v;
        private LeeftijdsKlasse _geselecteerdeLeeftijdsklasse = new LeeftijdsKlasse();
        private Categorie _geselecteerdGenre = new Categorie();
        private MediumDetail _selectedMediumDetail;
        private Medium _geslecteerdMedium;
        private bool _ischecked;
        private bool _mediaExemplaren = true;
        private bool _mediaDetailExemplaren;
        private ObservableCollection<Medium> _mediums;
        private ObservableCollection<MediumDetail> _mediumdetail;
        private ObservableCollection<LeeftijdsKlasse> _leeftijdsklasse;
        private ObservableCollection<Categorie> _categorie;

        private string _zoekTerm;
        private int _paginaNummer;
        private int _maxPaginaNummer;

        public MediumDetail SelectedMediumDetail
        {
            get { return _selectedMediumDetail; }
            set { _selectedMediumDetail = value; }
        }

        public Medium GeslecteerdMedium
        {
            get { return _geslecteerdMedium; }
            set { _geslecteerdMedium = value; }
        }

        public bool Ischecked
        {
            get { return _ischecked; }
            set
            {
                _ischecked = value;
                NotifyPropertyChanged();
                ChangeVisibility();
            }
        }

        public bool MediaExemplaren
        {
            get { return _mediaExemplaren; }
            set { _mediaExemplaren = value; NotifyPropertyChanged(); }
        }

        public bool MediaDetailExemplaren
        {
            get { return _mediaDetailExemplaren; }
            set { _mediaDetailExemplaren = value; NotifyPropertyChanged(); }
        }

        public ObservableCollection<Medium> Mediums
        {
            get { return _mediums; }
            set
            {
                _mediums = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<MediumDetail> MediumDetail
        {
            get { return _mediumdetail; }
            set
            {
                _mediumdetail = value;
                NotifyPropertyChanged();
            }
        }

        public LeeftijdsKlasse GeslecteerdeLeeftijdsklasse
        {
            get { return _geselecteerdeLeeftijdsklasse; }
            set
            {
                _geselecteerdeLeeftijdsklasse = value;

                NotifyPropertyChanged();
                PaginaNummer = 1;
                VullenDatagrid();
            }
        }

        public Categorie GeslecteerdGenre
        {
            get { return _geselecteerdGenre; }
            set
            {
                _geselecteerdGenre = value;

                NotifyPropertyChanged();
                PaginaNummer = 1;
                VullenDatagrid();
            }
        }

        public ObservableCollection<LeeftijdsKlasse> LeeftijdsKlasse
        {
            get { return _leeftijdsklasse; }
            set
            {
                _leeftijdsklasse = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<Categorie> Categorie
        {
            get { return _categorie; }
            set { _categorie = value; }
        }

        public string ZoekTerm
        {
            get
            {
                return _zoekTerm;
            }
            set
            {
                _zoekTerm = value;
                NotifyPropertyChanged();
                PaginaNummer = 1;
                VullenDatagrid();
            }
        }

        public int PaginaNummer
        {
            get => _paginaNummer;
            set
            {
                _paginaNummer = value;
                VullenDatagrid();
                NotifyPropertyChanged();
            }
        }
        public int MaxPaginaNummer
        {
            get => _maxPaginaNummer;
            set
            {
                _maxPaginaNummer = value;
            }
        }

        public string PaginaNummerString { 
            get {
                return PaginaNummer + "/" + MaxPaginaNummer;
            }
        }

        #endregion properties


        #region constructor


        public BoekenBestandBeheerViewModel(BoekenbestandView v)
        {
            _v = v;

            Categorie = new ObservableCollection<Categorie>(uow.CategorieRepository.Get());
            LeeftijdsKlasse = new ObservableCollection<LeeftijdsKlasse>(uow.LeeftijdsKlasseRepository.Get());

            LeeftijdsKlasse.Insert(0, new LeeftijdsKlasse() { Omschrijving = "Selecteer een leeftijdsklasse" });

            Categorie.Insert(0, new Categorie() { Omschrijving = "Selecteer een genre" });
            PaginaNummer = 1;

            FilterVerwijderen();
        }


        #endregion constructor


        #region basisviewmodel

        public override string this[string columnName]
        {
            get
            {
                return "";
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
                case "Bewerken":
                    MediumAanmakenView v = new MediumAanmakenView();

                    if (Ischecked)
                    {
                        if (SelectedMediumDetail != null)
                        {
                            v.DataContext = new MediumAanmakenViewModel(v, SelectedMediumDetail.Id);
                            v.Show();
                            _v.Close();
                        }
                        else
                            new PopUp("Foutmelding", "Selecteer een boek.").ShowDialog();
                    }
                    else
                    {
                        if (GeslecteerdMedium != null)
                        {
                            v.DataContext = new MediumAanmakenViewModel(v, GeslecteerdMedium.MediumDetail.Id);
                            v.Show();
                            _v.Close();
                        }
                        else
                            new PopUp("Foutmelding", "Selecteer een boek.").ShowDialog();
                    }
                    break;

                case "Toevoegen": Toevoegen(); break;

                case "Verwijderen": Verwijderen(); break;

                case "ExemplaarToevoegen": ExemplaarToevoegen(); break;

                case "HomeButton": BackToHome(); break;

                case "AccountOpties": AccountOpties(); break;

                case "FiltersVerwijderen": FilterVerwijderen(); break;
                case "PageBack":
                    if (PaginaNummer > 1)
                    {
                        PaginaNummer--;
                    }
                    break;
                case "PageNext":
                    if (PaginaNummer < MaxPaginaNummer)
                    {
                        PaginaNummer++;
                    }
                    break;
            }
        }

        #endregion basisviewmodel

        #region helpersfunctie

        public void VullenDatagrid()
        {
            Mediums = new ObservableCollection<Medium>();
            var query = new DatabaseContext().Set<MediumDetail>()
                .Include(md => md.LeeftijdsKlasse)
                .Include(md => md.Medium)
                .Include(md => md.MediumCategorieen.Select(mc => mc.Categorie))
                .Include(md => md.MediumDetailMedewerker.Select(mc => mc.Medewerker));

            if (!string.IsNullOrEmpty(ZoekTerm))
            {
                query = query.Where(x => x.Title.ToLower()
                .Contains(ZoekTerm.ToLower()) || x.MediumDetailMedewerker.Any(mdm => mdm.Medewerker.Naam.Contains(ZoekTerm))
                );
            }

            if (GeslecteerdGenre.Id > 0)
            {
                query = query.Where(md => md.MediumCategorieen
                       .Any(mc => mc.CategorieId == GeslecteerdGenre.Id)
                       );
            }
            if (GeslecteerdeLeeftijdsklasse.Id > 0)
            {
                query = query.Where(md=>md.LeeftijdsKlasseId == GeslecteerdeLeeftijdsklasse.Id);
            }

            MaxPaginaNummer = (query.Count() / 20) + 1;

            MediumDetail = new ObservableCollection<MediumDetail>(
            query.OrderBy(x => x.Id)
            .Skip((PaginaNummer - 1) * 20)
            .Take(20).ToList()
            );
            MediumDetail.ToList().ForEach(m => { m.Medium.ToList().ForEach(_m => Mediums.Add(_m)); });
        }

        public void FilterVerwijderen()
        {
            GeslecteerdeLeeftijdsklasse = LeeftijdsKlasse[0];
            GeslecteerdGenre = Categorie[0];
            ZoekTerm = "";
        }

        private void Toevoegen()
        {
            MediumAanmakenView v = new MediumAanmakenView();
            MediumAanmakenViewModel vm = new MediumAanmakenViewModel(v);
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

        private void BackToHome()
        {
            DashboardBeheerderView v = new DashboardBeheerderView();
            DashboardBeheerderViewModel vm = new DashboardBeheerderViewModel(v);
            v.DataContext = vm;
            v.Show();
            _v.Close();
        }

        private void Verwijderen()
        {
            if (GeslecteerdMedium != null)
            {
                var popup = new PopUp("Verwijderen", "Bent u zeker dat u dit medium wil verwijderen?\"", PopupButtonOptions.OkCancel);
                popup.ShowDialog();
                if (popup.DialogResult == PopUpResponse.Ok)
                {
                    uow.MediumRepository.Delete(x => x.Id == GeslecteerdMedium.Id);
                    int ok = uow.Save();
                    VullenDatagrid();
                }
                else
                {
                    var pop = new PopUp("Foutmelding", "Probleem met het verwijderen\"", PopupButtonOptions.Ok);
                    pop.ShowDialog();
                }
            }
            else if (SelectedMediumDetail != null)
            {
                var popup = new PopUp("Verwijderen", "Bent u zeker dat u dit medium wil verwijderen?\"", PopupButtonOptions.OkCancel);
                popup.ShowDialog();
                if (popup.DialogResult == PopUpResponse.Ok)
                {
                    uow.MediumDetailRepository.Delete(x => x.Id == SelectedMediumDetail.Id);
                    int ok = uow.Save();
                    VullenDatagrid();
                }
                else
                {
                    var pop = new PopUp("Foutmelding", "Probleem met het verwijderen\"", PopupButtonOptions.Ok);
                    pop.ShowDialog();
                }
            }
        }

        public void ChangeVisibility()
        {
            if (_ischecked == false)
            {
                MediaExemplaren = true;
                MediaDetailExemplaren = false;
            }
            else
            {
                MediaExemplaren = false;
                MediaDetailExemplaren = true;
            }
        }

        public void ExemplaarToevoegen()
        {
            if (SelectedMediumDetail != null || GeslecteerdMedium != null)
            {
                MediumExemplaarToevoegenView v = new MediumExemplaarToevoegenView();
                MediumExemplaarToevoegenViewModel vm;
                if (_ischecked == true)
                {
                    vm = new MediumExemplaarToevoegenViewModel(SelectedMediumDetail, v);
                }
                else
                {
                    vm = new MediumExemplaarToevoegenViewModel(v, GeslecteerdMedium);
                }
                v.DataContext = vm;
                v.ShowDialog();
                VullenDatagrid();
            }
            else
            {
                var result = new PopUp($"Exemplaar toevoegen", "Gelieven een boek te selecteren!", PopUp.PopupButtonOptions.Ok);

                result.ShowDialog();
            }
        }

        #endregion helpersfunctie
    }
}