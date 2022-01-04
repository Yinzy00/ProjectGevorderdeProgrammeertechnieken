using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using C_Our_Souls_DAL.Data.UnitOfWork;
using C_Our_Souls_WPF.Views;
using C_Our_Souls_DAL.Models;
using System.Collections.ObjectModel;
using static C_Our_Souls_WPF.Views.PopUp;
using C_Our_Souls_DAL.Data;

namespace C_Our_Souls_WPF.ViewModels
{
    public class ReserverenBoekBeheerderViewModel : BasisViewModel
    {
        private IUnitOfWork uow = new UnitOfWork(new C_Our_Souls_DAL.Data.DatabaseContext());

        private ReserverenBoekBeheerderView _v;

        private LeeftijdsKlasse _geselecteerdeLeeftijdsklasse = new LeeftijdsKlasse();

        private int _paginaNummer;
        private int _maxPaginaNummer;

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
        public string PaginaNummerString
        {
            get
            {
                return PaginaNummer + "/" + MaxPaginaNummer;
            }
        }

        public ReserverenBoekBeheerderView reserverenboekbeheerder;

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

        private Medium _geselecteerdMedium = new Medium();

        public Medium GeselecteerdMedium
        {
            get { return _geselecteerdMedium; }
            set
            {
                _geselecteerdMedium = value;
                var ctx = new DatabaseContext();

                IQueryable<Medium> _query = ctx.Set<Medium>()
                  .Include(m => m.Uitleningen);
                NotifyPropertyChanged();
            }
        }

        private Gebruiker _geselecteerdeKlant = new Gebruiker();

        public Gebruiker GeselecteerdeKlant
        {
            get { return _geselecteerdeKlant; }
            set
            {
                _geselecteerdeKlant = value;

                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<Gebruiker> _klant;

        public ObservableCollection<Gebruiker> Klant
        {
            get { return _klant; }
            set
            {
                _klant = value;
                NotifyPropertyChanged();
            }
        }

        private Categorie _geselecteerdGenre = new Categorie();

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

        private ObservableCollection<Medium> _mediums;

        public ObservableCollection<Medium> Mediums
        {
            get { return _mediums; }
            set
            {
                _mediums = value;

                NotifyPropertyChanged();
            }
        }

        private string _zoekTerm;

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

        private string _zoekTermKlant;

        public string ZoekTermKlant
        {
            get
            {
                return _zoekTermKlant;
            }
            set
            {
                _zoekTermKlant = value;
                NotifyPropertyChanged();
                ZoekKlant(_zoekTermKlant);
            }
        }

        private ObservableCollection<MediumDetail> _mediumdetail;

        public ObservableCollection<MediumDetail> MediumDetail
        {
            get { return _mediumdetail; }
            set
            {
                _mediumdetail = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<LeeftijdsKlasse> _leeftijdsklasse;

        public ObservableCollection<LeeftijdsKlasse> LeeftijdsKlasse
        {
            get { return _leeftijdsklasse; }
            set
            {
                _leeftijdsklasse = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<Categorie> _categorie;

        public ObservableCollection<Categorie> Categorie
        {
            get { return _categorie; }
            set { _categorie = value; }
        }

        public ReserverenBoekBeheerderViewModel(ReserverenBoekBeheerderView v)
        {
            var ctx = new DatabaseContext();
            _v = v;

            this.reserverenboekbeheerder = v;

            Categorie = new ObservableCollection<Categorie>(uow.CategorieRepository.Get());
            LeeftijdsKlasse = new ObservableCollection<LeeftijdsKlasse>(uow.LeeftijdsKlasseRepository.Get());
            Klant = new ObservableCollection<Gebruiker>(uow.GebruikerRepository.Get());

            LeeftijdsKlasse.Insert(0, new LeeftijdsKlasse() { Omschrijving = "Selecteer een leeftijdsklasse" });

            Categorie.Insert(0, new Categorie() { Omschrijving = "Selecteer een genre" });

            PaginaNummer = 1;
            VullenDatagrid();

        }

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
                case "MeerWeten":
                    MeerWeten();
                    break;

                case "Reserveren": Reserveren(); break;

                case "HomeButton": BackToHome(); break;
                case "AccountOpties":
                    AccountOpties();
                    break;
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
                query = query.Where(md => md.LeeftijdsKlasseId == GeslecteerdeLeeftijdsklasse.Id);
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

        private void AccountOpties()
        {
            AccountPopUpView v = new AccountPopUpView();
            AccountPopUpViewModel vm = new AccountPopUpViewModel();
            v.DataContext = vm;
            v.Show();
        }

        public void ZoekKlant(string zoekTermKlant)
        {
            Klant = new ObservableCollection<Gebruiker>(uow.GebruikerRepository
                   .Get().Where(x => x.Naam.ToLower().Contains(zoekTermKlant.ToLower()) || x.Voornaam.ToLower().Contains(zoekTermKlant.ToLower())));

            if (Klant.Count == 0)
            {
                var result = new PopUp($"Zoeken klanten", "In ons klantenbestand vinden we niets dat overeenkomt met uw zoekterm. Probeer een andere zoekterm", PopUp.PopupButtonOptions.Ok);

                result.ShowDialog();
                if (result.DialogResult == PopUp.PopUpResponse.Ok)
                {
                    _zoekTermKlant = "";
                    Klant = new ObservableCollection<Gebruiker>(uow.GebruikerRepository.Get());
                }
            }
        }

        private void Reserveren()
        {
            int geselecteerdeKlantID = GeselecteerdeKlant.Id;
            int geselecteerdMediumID = GeselecteerdMedium.Id;
            if (geselecteerdMediumID == 0)
            {
                var meerweten = new PopUp($"Reserveren", "Selecteer eerste een medium!", PopUp.PopupButtonOptions.Ok);
                meerweten.ShowDialog();
            }
            else
            {
                if (geselecteerdeKlantID != 0)
                {
                    DateTime? uitlenenTot = DateTime.Now.AddDays(10);

                    var reserveren = new PopUp($"Reserveren boek", $"Bevestig hier uw reservatie voor het boek {GeselecteerdMedium.MediumDetail.Title}. Deze is na bevestiging beschikbaar.", PopUp.PopupButtonOptions.OkCancel, "Ja", "Nee");
                    reserveren.ShowDialog();

                    if (reserveren.DialogResult == PopUpResponse.Ok)
                    {
                        Uitlening uitlening = new Uitlening() { GebruikerId = geselecteerdeKlantID, MediumId = geselecteerdMediumID, UitgeleendOp = DateTime.Now, OntleenIntresse = DateTime.Now };
                        uow.UitleningRepository.Add(uitlening);
                        uow.Save();
                        var succes = new PopUp($"Reserveren boek", "Reservatie is gelukt!", PopUp.PopupButtonOptions.Ok);
                        succes.ShowDialog();

                        var popupsucces = new PopUp($"Uitlening goedgekeurd", $"De uitlening voor '{GeselecteerdMedium.MediumDetail.Title}' werd goedgekeurd! Het boek kan uitgeleend worden tot {uitlenenTot}.", PopupButtonOptions.Ok);
                        popupsucces.ShowDialog();
                        
                        Reset();
                    }
                    else
                    {
                        var annulatie = new PopUp("Bevestiging", "Reservering is geannuleerd", PopupButtonOptions.Ok);
                        annulatie.ShowDialog();
                    }
                }
                else
                {
                    var klant = new PopUp($"Reserveren boek", "Selecteer eerst een klant", PopUp.PopupButtonOptions.Ok);
                    klant.ShowDialog();
                }
            }
        }

        private void Reset()
        {
            Mediums = new ObservableCollection<Medium>(uow.MediumRepository.Get(d => d.Uitleningen.All(o => o.Binnengebracht != null)));
            MediumDetail = new ObservableCollection<MediumDetail>
              (uow.MediumDetailRepository.Get(
              x => x.MediumDetailMedewerker.Select(y => y.Medewerker),
              x => x.LeeftijdsKlasse,
              x => x.MediumCategorieen.Select(y => y.Categorie),
              x => x.Medium,
              x => x.Medium.Select(u => u.Uitleningen),
              z => z.Soort
              ));
            MediumDetail.ToList().ForEach(m => { m.Medium.ToList().ForEach(_m => Mediums.Add(_m)); });
            GeselecteerdMedium = null;
        }

        private void MeerWeten()
        {
            int geselecteerdMediumID = GeselecteerdMedium.Id;
            if (geselecteerdMediumID == 0)
            {
                var meerweten = new PopUp($"Meer Weten", "Selecteer eerste een medium!", PopUp.PopupButtonOptions.Ok);
                meerweten.ShowDialog();
            }
            else if (geselecteerdMediumID != 0)
            {

                Medium m = GeselecteerdMedium;
                DetailBoekView detailview = new DetailBoekView();
                DetailBoekViewModel detailvm = new DetailBoekViewModel(detailview, m, "ReserverenBeheerder");
                detailview.DataContext = detailvm;
                detailview.Show();
                _v.Close();
            }
        }

        private void BackToHome()
        {
            DashboardBeheerderView view = new DashboardBeheerderView();
            DashboardBeheerderViewModel vm = new DashboardBeheerderViewModel(view);
            view.DataContext = vm;
            view.Show();
            _v.Close();
        }
    }
}