using C_Our_Souls_DAL.Data;
using C_Our_Souls_DAL.Data.UnitOfWork;
using C_Our_Souls_DAL.Models;
using C_Our_Souls_WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static C_Our_Souls_WPF.Views.PopUp;

namespace C_Our_Souls_WPF.ViewModels
{
    public class UitleenBeheerViewModel : BasisViewModel
    {
        public override string this[string columnName]
        {
            get
            {
                return "";
            }
        }

        private IUnitOfWork uow = new UnitOfWork(new DatabaseContext());
        private List<Uitlening> _uitleningen;
        private Uitlening _geselecteerdeUitlening;
        private UitleenBeheerView _uitleenbeheerview;
        public UitleenBeheerView uitleenbeheerview;
        private string _zoekTerm;
        public int CurrentuserId;

        public List<Uitlening> Uitleningen
        {
            get { return _uitleningen; }
            set
            {
                _uitleningen = value;
                NotifyPropertyChanged();
            }
        }

        private List<Medium> _mediums;

        public List<Medium> Mediums
        {
            get { return _mediums; }
            set
            {
                _mediums = value;
                NotifyPropertyChanged();
            }
        }

        public Uitlening GeselecteerdeUitlening
        {
            get { return _geselecteerdeUitlening; }
            set
            {
                _geselecteerdeUitlening = value;
                NotifyPropertyChanged();
            }
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
                Zoeken(_zoekTerm);
            }
        }

        public UitleenBeheerViewModel(UitleenBeheerView v)
        {
            _uitleenbeheerview = v;

            this._uitleenbeheerview = v;
            //CurrentuserId = (int)App.Current.Properties["CurrentUserId"];

            Uitleningen = new List<Uitlening>
                (uow.UitleningRepository.Get(
                    x => x.Gebruiker,
                    y => y.Medium.MediumDetail
                    ).Where(x => x.UitgeleendOp == null).Where(k => k.UitlenenTot == null)); 
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            switch (parameter.ToString())
            {
                case "Accepteren": ReservatieBehandelen(parameter.ToString()); break;
                case "Weigeren": ReservatieBehandelen(parameter.ToString()); break;
                case "ReservatieAanmaken": ReservatieAanmaken(); break;
                case "HomeButton": BackToHome(); break;
                case "AccountOpties": AccountOpties(); break;
            }
        }

        private void AccountOpties()
        {
            AccountPopUpView v = new AccountPopUpView();
            AccountPopUpViewModel vm = new AccountPopUpViewModel(_uitleenbeheerview);
            v.DataContext = vm;
            v.Show();
        }

        private void BackToHome()
        {
            DashboardBeheerderView view = new DashboardBeheerderView();
            DashboardBeheerderViewModel vm = new DashboardBeheerderViewModel(view);
            view.DataContext = vm;
            view.Show();
            _uitleenbeheerview.Close();
        }

        public void Zoeken(string zoekterm)
        {
            Uitleningen = uow.UitleningRepository.Get(x => x.Medium).Where(x => x.UitgeleendOp == null).Where(k => k.UitlenenTot == null).Where(x => x.Gebruiker.VolledigeNaam.ToLower().Contains(zoekterm.ToLower())).ToList();
            
            if (Uitleningen.Count == 0)
            {
                var result = new PopUp($"Zoeken uitleningen", "In onze uitleningen vinden we niets dat overeenkomt met uw zoekterm. Probeer een andere zoekterm", PopUp.PopupButtonOptions.Ok);

                result.ShowDialog();
                if (result.DialogResult == PopUp.PopUpResponse.Ok)
                {
                    _zoekTerm = "";
                    Uitleningen = new List<Uitlening>(uow.UitleningRepository.Get().Where(x => x.UitgeleendOp == null).Where(k => k.UitlenenTot == null));
                }
            }
            else
            {
                Uitleningen = (List<Uitlening>)uow.UitleningRepository.Get(g => g.Medium).Where(x => x.UitgeleendOp == null).Where(k => k.UitlenenTot == null);
            }
        }

        private void ReservatieAanmaken()
        {
            ReserverenBoekBeheerderView beheerderreserverenview = new ReserverenBoekBeheerderView();
            ReserverenBoekBeheerderViewModel vmm = new ReserverenBoekBeheerderViewModel(beheerderreserverenview);
            beheerderreserverenview.DataContext = vmm;
            beheerderreserverenview.Show();
            _uitleenbeheerview.Close();
        }

        private void ReservatieBehandelen(string actie)
        {
            string bevestiging = "";
            DateTime? uitlenenTot = DateTime.Now.AddDays(10);


            if (GeselecteerdeUitlening != null)
            {
                var result = new PopUp($"{actie}", $"Weet u zeker dat u het boek '{GeselecteerdeUitlening.Medium.MediumDetail.Title}' wilt {actie.ToLower()}?", PopupButtonOptions.OkCancel, "Ja", "Nee");
                result.ShowDialog();
                if (result.DialogResult == PopUpResponse.Ok)
                {
                    if (actie.ToLower() == "accepteren")
                    {
                        GeselecteerdeUitlening.UitgeleendOp = DateTime.Now; ;
                        bevestiging = "goedgekeurd";
                        uitlenenTot = GeselecteerdeUitlening.UitlenenTot;

                        uow.UitleningRepository.Update(GeselecteerdeUitlening);
                        uow.Save();

                        var popupsucces = new PopUp($"Uitlening {bevestiging}", $"De uitlening voor '{GeselecteerdeUitlening.Medium.MediumDetail.Title}' werd {bevestiging}! Het boek kan uitgeleend worden tot {uitlenenTot}.", PopupButtonOptions.Ok);
                        popupsucces.ShowDialog();
                        Reset();
                    }
                    else
                    {
                        bevestiging = "geweigerd";

                        var popup = new PopUp($"Uitlening {bevestiging}", $"De uitlening voor '{GeselecteerdeUitlening.Medium.MediumDetail.Title}' werd {bevestiging}!", PopupButtonOptions.Ok);
                        popup.ShowDialog();

                        uow.UitleningRepository.Delete(GeselecteerdeUitlening);
                        uow.Save();

                        Reset();

                    }


                }
                else
                {
                    var annulatie = new PopUp("Bevestiging", "Actie is geannuleerd", PopupButtonOptions.Ok);
                    annulatie.ShowDialog();
                }
            }
            else
            {
                var p = new PopUp("Selecteer uitlening", "Selecteer eerst een uitlening", PopupButtonOptions.Ok);
                p.ShowDialog();
            }
        }

        private void Reset()
        {
            Uitleningen = new List<Uitlening>
                (uow.UitleningRepository.Get(
                    x => x.Gebruiker,
                    y => y.Medium.MediumDetail)
                    .Where(x => x.UitgeleendOp == null).Where(k => k.UitlenenTot == null)
                    );
            GeselecteerdeUitlening = null;
        }
    }
 }

