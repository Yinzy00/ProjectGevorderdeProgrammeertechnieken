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
using C_Our_Souls_DAL.BasisModels;
using C_Our_Souls_WPF.Viewmodels;
using C_Our_Souls_DAL.Handlers;

namespace C_Our_Souls_WPF.ViewModels
{
    public class BoekenbeursAanmakenViewModel : BasisViewModel, IDisposable
    {
        private IUnitOfWork unitOfWork = new UnitOfWork(new DatabaseContext());
        public ObservableCollection<Boekenbeurs> _boekenbeurs;
        private string _naam;
        private DateTime _datumVan;
        private DateTime _datumTot;
        private string _locatie;
        private Boekenbeurs _boekenbeursRecord;
        private List<string> _uren;
        private List<string> _minuten;
        private double _beginuur;
        private double _beginminuten;
        private double _eindminuten;
        private double _einduur;
        private string _buttonTekst;
        private bool _locatieEnabled;
        private string _aanmakenWijzigen;
        private BoekenbeursAanmakenView _v;
        private string _cmb;
        public string _oldLocation { get; set; }

        public string Cmb
        {
            get { return _cmb; }
            set { _cmb = value; }
        }

        public string AanmakenWijzigen
        {
            get { return _aanmakenWijzigen; }
            set { _aanmakenWijzigen = value; }
        }

        public bool LocatieEnabled
        {
            get { return _locatieEnabled; }
            set
            {
                _locatieEnabled = value;
                NotifyPropertyChanged();
            }
        }

        public string ButtonTekst
        {
            get { return _buttonTekst; }
            set
            {
                _buttonTekst = value;
                NotifyPropertyChanged();
            }
        }

        public double Einduur
        {
            get { return _einduur; }
            set
            {
                _einduur = value;
                NotifyPropertyChanged();
            }
        }

        public double Eindminuten
        {
            get { return _eindminuten; }
            set
            {
                _eindminuten = value;
                NotifyPropertyChanged();
            }
        }

        public double Beginminuten
        {
            get { return _beginminuten; }
            set
            {
                _beginminuten = value;
                NotifyPropertyChanged();
            }
        }

        public double Beginuur
        {
            get { return _beginuur; }
            set
            {
                _beginuur = value;
                NotifyPropertyChanged();
            }
        }

        public List<string> Minuten
        {
            get { return _minuten; }
            set { _minuten = value; }
        }

        public List<string> Uren
        {
            get { return _uren; }
            set
            {
                _uren = value;
            }
        }

        public Boekenbeurs BoekenbeursRecord
        {
            get { return _boekenbeursRecord; }
            set
            {
                _boekenbeursRecord = value;
                NotifyPropertyChanged();
            }
        }

        public string Locatie
        {
            get { return _locatie; }
            set
            {
                _locatie = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime DatumTot
        {
            get { return _datumTot; }
            set
            {
                _datumTot = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime DatumVan
        {
            get { return _datumVan; }
            set
            {
                _datumVan = value;
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

        public ObservableCollection<Boekenbeurs> Boekenbeurs
        {
            get { return _boekenbeurs; }
            set
            {
                _boekenbeurs = value;
                NotifyPropertyChanged();
            }
        }

        public BoekenbeursAanmakenViewModel(BoekenbeursAanmakenView v)
        {
            _v = v;
            Cmb = "Visible";
            AanmakenWijzigen = "Aanmaken";
            LocatieEnabled = false;
            ButtonTekst = "AANMAKEN";
            //Combobox uren
            List<string> urenlijst = new List<string>();
            for (int i = 5; i < 24; i++)
            {
                urenlijst.Add(i.ToString());
            }
            Uren = urenlijst;

            //Combobox minuten
            List<string> minutenLijst = new List<string>();
            minutenLijst.Add("00");
            minutenLijst.Add("15");
            minutenLijst.Add("30");
            minutenLijst.Add("45");
            Minuten = minutenLijst;

            BoekenbeursRecord = new Boekenbeurs() { DatumVan = DateTime.Today, DatumTot = DateTime.Today };
        }

        public BoekenbeursAanmakenViewModel(BoekenbeursAanmakenView v, int id)
        {
            Boekenbeurs b = unitOfWork.BoekenbeursRepository.Get(_b => _b.Id == id, _b => _b.GebruikerBoekenbeurzen.Select(gb => gb.Gebruiker)).First();
            Cmb = "Hidden";
            ButtonTekst = "WIJZIGEN";
            AanmakenWijzigen = "Wijzigen";
            LocatieEnabled = true;
            BoekenbeursRecord = b;
            _oldLocation = b.Locatie;
            Naam = b.Naam;
            DatumTot = (DateTime)b.DatumTot;
            DatumVan = b.DatumVan;
            _v = v;
        }

        public override string this[string columnName]
        {
            get { return ""; }
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            switch (parameter.ToString())
            {
                case "HomeButton": Home(); break;
                case "Aanmaken": Aanmaken(); break;
                case "Wijzigen": Wijzigen(); break;
                case "AccountOpties": AccountOpties(); break;
            }
        }

        private void AccountOpties()
        {
            AccountPopUpView v = new AccountPopUpView();
            AccountPopUpViewModel vm = new AccountPopUpViewModel(_v);
            v.DataContext = vm;
            v.Show();
        }

        private void Wijzigen()
        {
            if (string.IsNullOrWhiteSpace(BoekenbeursRecord.Locatie))
            {
                var popup = new PopUp("Boekenbeurs wijzigen", "Vul een locatie in!", PopUp.PopupButtonOptions.Ok);
                popup.ShowDialog();
            }
            else
            {
                if (string.IsNullOrWhiteSpace(BoekenbeursRecord.Naam))
                {
                    var popup = new PopUp("Boekenbeurs wijzigen", "Vul een naam in voor deze boekenbeurs!", PopUp.PopupButtonOptions.Ok);
                    popup.ShowDialog();
                }
                else
                {
                    if (BoekenbeursRecord.DatumVan > BoekenbeursRecord.DatumTot)
                    {
                        var popup = new PopUp("Boekenbeurs wijzigen", "De beurs kan niet eindigen voor de begindatum!", PopUp.PopupButtonOptions.Ok);
                        popup.ShowDialog();
                    }
                    else
                    {
                        unitOfWork.BoekenbeursRepository.Update(BoekenbeursRecord);
                        int ok = unitOfWork.Save();
                        if (ok > 0)
                        {
                            if (BoekenbeursRecord.Locatie != _oldLocation)
                            {
                                BoekenbeursRecord.GebruikerBoekenbeurzen.ToList().ForEach(gb =>
                                {
                                    var gebruiker = gb.Gebruiker;
                                    new EmailHandler().SendMail(
                                        gebruiker.VolledigeNaam,
                                        gebruiker.Email,
                                        $"{BoekenbeursRecord.Naam}",
                                        $"Beste {gebruiker.VolledigeNaam}, <br/><br/>" +
                                        $"De locatie voor boekenebeurs {BoekenbeursRecord.Naam} is bekendgemaakt.<br/>" +
                                        $"Deze boekenbeurs zal doorgaan te {BoekenbeursRecord.Locatie}.<br/><br/>" +
                                        $"Met vriendelijke groeten<br/>" +
                                        $"De bib"
                                        );
                                });
                            }

                            //keer terug naar boekenbeursoverzicht
                            OverzichtBoekenbeursView overzichtboekenbeursV = new OverzichtBoekenbeursView();
                            OverzichtBoekenbeursViewModel overzichtboekenbeursVM = new OverzichtBoekenbeursViewModel(overzichtboekenbeursV);
                            overzichtboekenbeursV.DataContext = overzichtboekenbeursVM;
                            overzichtboekenbeursV.Show();
                            _v.Close();
                        }
                        else
                        {
                            var popup = new PopUp("Boekenbeurs wijzigen", "Er ging iets mis.\nBoekenbeurs werd niet gewijzigd!", PopUp.PopupButtonOptions.Ok);
                            popup.ShowDialog();
                        }
                    }
                }
            }
        }

        private void Aanmaken()
        {
            if (!string.IsNullOrEmpty(BoekenbeursRecord.Naam))
            {
                if (BoekenbeursRecord.DatumVan < DateTime.Today)
                {
                    var popup = new PopUp("Boekenbeurs", "Een boekenbeurs kan niet in het verleden liggen!", PopUp.PopupButtonOptions.Ok);
                    popup.ShowDialog();
                }
                else
                {
                    if (BoekenbeursRecord.DatumVan > BoekenbeursRecord.DatumTot)
                    {
                        var popup = new PopUp("Boekenbeurs", "Een boekenbeurs kan niet vroeger eindigen dan ze begint!", PopUp.PopupButtonOptions.Ok);
                        popup.ShowDialog();
                    }
                    else
                    {
                        if (BoekenbeursRecord.DatumVan < DateTime.Now.AddDays(21))
                        {
                            var popup = new PopUp("Boekenbeurs", "Een boekenbeurs moet minimum 3 weken in de toekomst liggen.", PopUp.PopupButtonOptions.Ok);
                            popup.ShowDialog();
                        }
                        else
                        {
                            if (Beginuur <= 0 || Einduur <= 0)
                            {
                                var popup = new PopUp("Boekenbeurs", "Gelieve een eind -en beginuur te selecteren!", PopUp.PopupButtonOptions.Ok);
                                popup.ShowDialog();
                            }
                            else
                            {
                                BoekenbeursRecord.DatumVan = BoekenbeursRecord.DatumVan.AddHours(Beginuur).AddMinutes(Beginminuten);
                                BoekenbeursRecord.DatumTot = BoekenbeursRecord.DatumTot.Value.AddHours(Einduur).AddMinutes(Eindminuten);
                                BoekenbeursRecord.InkomPrijs = 3;
                                if (Beginuur > Einduur)
                                {
                                    var popup = new PopUp("Boekenbeurs", "Je gekozen einduur komt eerder dan het beginuur!", PopUp.PopupButtonOptions.Ok);
                                    popup.ShowDialog();
                                }
                                else
                                {
                                    unitOfWork.BoekenbeursRepository.Add(BoekenbeursRecord);
                                    int ok = unitOfWork.Save();
                                    if (ok > 0)
                                    {
                                        OverzichtBoekenbeursView overzichtboekenbeursV = new OverzichtBoekenbeursView();
                                        OverzichtBoekenbeursViewModel overzichtboekenbeursVM = new OverzichtBoekenbeursViewModel(overzichtboekenbeursV);
                                        overzichtboekenbeursV.DataContext = overzichtboekenbeursVM;
                                        overzichtboekenbeursV.Show();
                                        _v.Close();
                                    }
                                    else
                                    {
                                        var popup = new PopUp("Boekenbeurs", "Er ging iets mis... \nBoekenbeurs werd niet toegevoegd!", PopUp.PopupButtonOptions.Ok);
                                        popup.ShowDialog();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                var popup = new PopUp("Boekenbeurs", "Geef de boekenbeurs een naam!", PopUp.PopupButtonOptions.Ok);
                popup.ShowDialog();
            }
        }

        private void Home()
        {
            DashboardBeheerderView dashboardBView = new DashboardBeheerderView();
            DashboardBeheerderViewModel dashboardBViewModel = new DashboardBeheerderViewModel(dashboardBView);
            dashboardBView.DataContext = dashboardBViewModel;
            dashboardBView.Show();
            _v.Close();
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }
    }
}