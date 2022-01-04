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
using System.Data.Entity;

namespace C_Our_Souls_WPF.ViewModels
{
    public class ReserverenBoekGebruikerViewModel : BasisViewModel
    {
        private IUnitOfWork uow = new UnitOfWork(new C_Our_Souls_DAL.Data.DatabaseContext());

        public int CurrentuserId;

        private ReserverenBoekGebruikerView _v;

        private LeeftijdsKlasse _geselecteerdeLeeftijdsklasse = new LeeftijdsKlasse();

        public ReserverenBoekGebruikerView reserverenboekGebruiker;
        private int _paginaNummer;
        private int _maxPaginaNummer;
        public int PaginaNummer
        {
            get => _paginaNummer;
            set
            {
                _paginaNummer = value;
                GeefMediumLijst();
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
        public LeeftijdsKlasse GeslecteerdeLeeftijdsklasse
        {
            get { return _geselecteerdeLeeftijdsklasse; }
            set
            {
                _geselecteerdeLeeftijdsklasse = value;

                NotifyPropertyChanged();
                PaginaNummer = 1;
                GeefMediumLijst();
            }
        }

        private Medium _geselecteerdMedium = new Medium();

        public Medium GeselecteerdMedium
        {
            get { return _geselecteerdMedium; }
            set
            {
                _geselecteerdMedium = value;
                //var ctx = new DatabaseContext();

                //IQueryable<Medium> _query = ctx.Set<Medium>()
                //  .Include(m => m.Uitleningen);
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
                GeefMediumLijst();
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
                //Zoeken(_zoekTerm);
                PaginaNummer = 1;
                GeefMediumLijst();
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

        public ReserverenBoekGebruikerViewModel(ReserverenBoekGebruikerView v)
        {
            _v = v;
            CurrentuserId = (int)App.Current.Properties["CurrentUserId"];

            this.reserverenboekGebruiker = v;
            //PaginaNummer = 1;
            //GeefMediumLijst();


           //Mediums = new ObservableCollection<Medium>(uow.MediumRepository.Get(d => d.Uitleningen.All(o => o.Binnengebracht != null)));

           // Mediums = new ObservableCollection<Medium>();
           // MediumDetail = new ObservableCollection<MediumDetail>
           //   (uow.MediumDetailRepository.Get(
           //   x => x.MediumDetailMedewerker.Select(y => y.Medewerker),
           //   x => x.LeeftijdsKlasse,
           //   x => x.MediumCategorieen.Select(y => y.Categorie),
           //    x => x.Medium.Select(u => u.Uitleningen),
           //   x => x.Soort
           //   ));

           // MediumDetail.ToList().ForEach(m => { m.Medium.ToList().ForEach(_m => Mediums.Add(_m)); });

            Categorie = new ObservableCollection<Categorie>(uow.CategorieRepository.Get());
            LeeftijdsKlasse = new ObservableCollection<LeeftijdsKlasse>(uow.LeeftijdsKlasseRepository.Get());
            LeeftijdsKlasse.Insert(0, new LeeftijdsKlasse() { Omschrijving = "Selecteer een leeftijdsklasse" });

            Categorie.Insert(0, new Categorie() { Omschrijving = "Selecteer een genre" });
            Reset();
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
                case "FiltersVerwijderen":
                    Reset();
                    break;
            }
        }

        private void AccountOpties()
        {
            AccountPopUpView v = new AccountPopUpView();
            AccountPopUpViewModel vm = new AccountPopUpViewModel(_v);
            v.DataContext = vm;
            v.Show();
        }

        //public void Zoeken(string zoekterm)
        //{
        //    Mediums = new ObservableCollection<Medium>(uow.MediumRepository
        //           .Get(d => d.Uitleningen.All(o => o.Binnengebracht != null)).Where(x => x.MediumDetail.Title.ToLower().Contains(zoekterm.ToLower()) || x.MediumDetail.Auteurs.ToLower().Contains(zoekterm.ToLower())));

        //    if (Mediums.Count == 0)
        //    {
        //        var result = new PopUp($"Zoeken boekenbestand", "In ons boekenbestand vinden we niets dat overeenkomt met uw zoekterm. Probeer een andere zoekterm", PopUp.PopupButtonOptions.Ok);

        //        result.ShowDialog();
        //        if (result.DialogResult == PopUp.PopUpResponse.Ok)
        //        {
        //            _zoekTerm = "";
        //            Mediums = new ObservableCollection<Medium>((List<Medium>)uow.MediumRepository.Get(d => d.Uitleningen.All(o => o.Binnengebracht != null)));
        //        }
        //    }
        //}

        private void Reserveren()
        {
            if (GeselecteerdMedium != null)
            {
                int geselecteerdMediumID = GeselecteerdMedium.Id;
                var reserveren = new PopUp($"Reserveren boek", $"Bevestig hier uw reservatie voor het boek {GeselecteerdMedium.MediumDetail.Title}. Deze is na bevestiging beschikbaar.", PopUp.PopupButtonOptions.OkCancel, "Ja", "Nee");
                reserveren.ShowDialog();

                if (reserveren.DialogResult == PopUpResponse.Ok)
                {
                    Uitlening uitlening = new Uitlening() { GebruikerId = CurrentuserId, MediumId = geselecteerdMediumID, OntleenIntresse = DateTime.Now };
                    uow.UitleningRepository.Add(uitlening);
                    uow.Save();
                    var succes = new PopUp($"Reserveren boek", "Reservatie is gelukt!", PopUp.PopupButtonOptions.Ok);
                    succes.ShowDialog();
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
                var meerweten = new PopUp($"Reserveren", "Selecteer eerste een medium!", PopUp.PopupButtonOptions.Ok);
                meerweten.ShowDialog();
            }
        }

        private void Reset()
        {
            //Mediums = new ObservableCollection<Medium>(uow.MediumRepository.Get(d => d.Uitleningen.All(o => o.Binnengebracht != null)));
            //MediumDetail = new ObservableCollection<MediumDetail>
            //  (uow.MediumDetailRepository.Get(
            //  x => x.MediumDetailMedewerker.Select(y => y.Medewerker),
            //  x => x.LeeftijdsKlasse,
            //  x => x.MediumCategorieen.Select(y => y.Categorie),
            //  x => x.Medium,
            //  x => x.Medium.Select(u => u.Uitleningen),
            //  z => z.Soort
            //  ));
            //MediumDetail.ToList().ForEach(m => { m.Medium.ToList().ForEach(_m => Mediums.Add(_m)); });
            GeslecteerdeLeeftijdsklasse = LeeftijdsKlasse[0];
            GeslecteerdGenre = Categorie[0];
            ZoekTerm = "";
            GeselecteerdMedium = null;
            PaginaNummer = 1;
            GeefMediumLijst();
        }

        private void MeerWeten()
        {
            if (GeselecteerdMedium != null)
            {
                Medium m = GeselecteerdMedium;
                DetailBoekView detailview = new DetailBoekView();
                DetailBoekViewModel detailvm = new DetailBoekViewModel(detailview, m, "ReserverenGebruiker");
                detailview.DataContext = detailvm;
                detailview.Show();
                _v.Close();
            }
            else
            {
                var meerweten = new PopUp($"Meer Weten", "Selecteer eerste een medium!", PopUp.PopupButtonOptions.Ok);
                meerweten.ShowDialog();
            }
        }

        private void BackToHome()
        {
            DashboardGebruikerView view = new DashboardGebruikerView();
            DashboardGebruikerViewModel vm = new DashboardGebruikerViewModel(view);
            view.DataContext = vm;
            view.Show();
            _v.Close();
        }
        public void GeefMediumLijst()
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

        //private void GeefMediumLijst()
        //{
        //    var ctx = new DatabaseContext();
        //    if (GeslecteerdGenre.Id > 0 && GeslecteerdeLeeftijdsklasse.Id == 0)
        //    {
        //        int genreID = GeslecteerdGenre.Id;
        //        if (GeslecteerdGenre != null)
        //        {
        //            IQueryable<Medium> _query = ctx.Set<Medium>()
        //            .Include(t => t.Uitleningen)
        //            .Include(m => m.MediumDetail.MediumCategorieen.Select(mc => mc.Categorie))
        //            .Include(m => m.MediumDetail.LeeftijdsKlasse)
        //            .Include(m => m.MediumDetail.MediumDetailMedewerker
        //            .Select(mdm => mdm.Medewerker))
        //            .Where(md => md.MediumDetail.MediumCategorieen
        //           .Any(mc => mc.CategorieId == GeslecteerdGenre.Id))
        //            .Where(f => f.Uitleningen.All(t => t.Binnengebracht != null));
        //            Mediums = new ObservableCollection<Medium>(_query.ToList());
        //        }
        //        else
        //        {
        //            Mediums = new ObservableCollection<Medium>(uow.MediumRepository.Get(d => d.Uitleningen.All(o => o.Binnengebracht != null)));
        //            MediumDetail = new ObservableCollection<MediumDetail>
        //              (uow.MediumDetailRepository.Get(
        //              x => x.MediumDetailMedewerker.Select(y => y.Medewerker),
        //              x => x.LeeftijdsKlasse,
        //              x => x.MediumCategorieen.Select(y => y.Categorie),
        //              x => x.Medium,
        //              x => x.Medium.Select(u => u.Uitleningen)
        //              ));
        //            MediumDetail.ToList().ForEach(m => { m.Medium.ToList().ForEach(_m => Mediums.Add(_m)); });
        //        }
        //    }
        //    else if (GeslecteerdeLeeftijdsklasse.Id > 0 && GeslecteerdGenre.Id == 0)
        //    {
        //        int leeftijdsklasseID = GeslecteerdeLeeftijdsklasse.Id;
        //        if (GeslecteerdeLeeftijdsklasse != null)
        //        {
        //            Mediums = new ObservableCollection<Medium>(uow.MediumRepository.Get(d => d.Uitleningen.All(o => o.Binnengebracht != null)).Where(x => x.MediumDetail.LeeftijdsKlasse.Id == leeftijdsklasseID));
        //        }
        //        else
        //        {
        //            Mediums = new ObservableCollection<Medium>(uow.MediumRepository.Get(d => d.Uitleningen.All(o => o.Binnengebracht != null)));
        //            MediumDetail = new ObservableCollection<MediumDetail>
        //              (uow.MediumDetailRepository.Get(
        //              x => x.MediumDetailMedewerker.Select(y => y.Medewerker),
        //              x => x.LeeftijdsKlasse,
        //              x => x.MediumCategorieen.Select(y => y.Categorie),
        //              x => x.Medium,
        //              x => x.Medium.Select(u => u.Uitleningen)
        //              ));
        //            MediumDetail.ToList().ForEach(m => { m.Medium.ToList().ForEach(_m => Mediums.Add(_m)); });
        //        }
        //    }
        //    else
        //    if (GeslecteerdGenre.Id > 0 && GeslecteerdeLeeftijdsklasse.Id > 0)
        //    {
        //        if (GeslecteerdeLeeftijdsklasse != null && GeslecteerdGenre != null)
        //        {
        //            IQueryable<Medium> _query = ctx.Set<Medium>()
        //           .Include(t => t.Uitleningen)
        //          .Include(m => m.MediumDetail.MediumCategorieen.Select(mc => mc.Categorie))
        //          .Include(m => m.MediumDetail.LeeftijdsKlasse)
        //          .Include(m => m.MediumDetail.MediumDetailMedewerker
        //          .Select(mdm => mdm.Medewerker))
        //          .Where(x => x.MediumDetail.LeeftijdsKlasse.Id == GeslecteerdeLeeftijdsklasse.Id)
        //          .Where(md => md.MediumDetail.MediumCategorieen
        //          .Any(mc => mc.CategorieId == GeslecteerdGenre.Id))
        //            .Where(f => f.Uitleningen.All(t => t.Binnengebracht != null));

        //            Mediums = new ObservableCollection<Medium>(_query.ToList());
        //        }
        //        else
        //        {
        //            Mediums = new ObservableCollection<Medium>(uow.MediumRepository.Get(d => d.Uitleningen.All(o => o.Binnengebracht != null)));
        //            MediumDetail = new ObservableCollection<MediumDetail>
        //              (uow.MediumDetailRepository.Get(
        //              x => x.MediumDetailMedewerker.Select(y => y.Medewerker),
        //              x => x.LeeftijdsKlasse,
        //              x => x.MediumCategorieen.Select(y => y.Categorie),
        //              x => x.Medium,
        //               x => x.Medium.Select(u => u.Uitleningen)
        //              ));
        //            MediumDetail.ToList().ForEach(m => { m.Medium.ToList().ForEach(_m => Mediums.Add(_m)); });
        //        }
        //    }
        //    else if (GeslecteerdGenre.Id == 0 && GeslecteerdeLeeftijdsklasse.Id == 0)
        //    {
        //        Mediums = new ObservableCollection<Medium>(uow.MediumRepository.Get(d => d.Uitleningen.All(o => o.Binnengebracht != null)));
        //        MediumDetail = new ObservableCollection<MediumDetail>
        //          (uow.MediumDetailRepository.Get(
        //          x => x.MediumDetailMedewerker.Select(y => y.Medewerker),
        //          x => x.LeeftijdsKlasse,
        //          x => x.MediumCategorieen.Select(y => y.Categorie),
        //          x => x.Medium,
        //          x => x.Medium.Select(u => u.Uitleningen)
        //          ));
        //        MediumDetail.ToList().ForEach(m => { m.Medium.ToList().ForEach(_m => Mediums.Add(_m)); });
        //    }
        //}
    }
}