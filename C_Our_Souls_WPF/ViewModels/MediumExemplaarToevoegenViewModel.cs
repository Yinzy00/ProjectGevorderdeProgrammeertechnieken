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
using System.Windows;

namespace C_Our_Souls_WPF.ViewModels
{
    internal class MediumExemplaarToevoegenViewModel : BasisViewModel, IDisposable
    {
        private IUnitOfWork unitOfWork = new UnitOfWork(new DatabaseContext());
        private Medium _mediumRecord;
        private DateTime _eindelevensduur;
        private double _verkoopprijs;
        private DateTime _registratieDatum;
        private MediumExemplaarToevoegenView _v;

        public DateTime RegistratieDatum
        {
            get { return DateTime.Now; }
        }

        public double Verkoopprijs
        {
            get { return _verkoopprijs; }
            set
            {
                _verkoopprijs = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime EindeLevensduur
        {
            get { return _eindelevensduur; }
            set
            {
                _eindelevensduur = value;
                NotifyPropertyChanged();
            }
        }

        public Medium MediumRecord
        {
            get { return _mediumRecord; }
            set { _mediumRecord = value; }
        }

        public MediumExemplaarToevoegenViewModel(MediumDetail mdet, MediumExemplaarToevoegenView v)
        {
            MediumRecord = new Medium() { EindeLevensduur = DateTime.Today.AddYears(5), Verkoopprijs = 5, MediumDetailId = mdet.Id };
            _v = v;
        }

        public MediumExemplaarToevoegenViewModel(MediumExemplaarToevoegenView v, Medium m)
        {
            MediumRecord = new Medium() { EindeLevensduur = DateTime.Today.AddYears(5), Verkoopprijs = 5, MediumDetailId = m.MediumDetailId };
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
                case "Terug": Terug(); break;
                case "Opslaan": Opslaan(); break;
            }
        }

        private void MediumRecordInstellen()
        {
            //MediumRecord.EindeLevensduur = EindeLevensduur;
            MediumRecord.Registratie = DateTime.Today;
            MediumRecord.Verkoopprijs = Verkoopprijs;
        }

        private void Opslaan()
        {
            MediumRecordInstellen();
            if (MediumRecord.IsGeldig())
            {
                unitOfWork.MediumRepository.Add(MediumRecord);
                int ok = unitOfWork.Save();
                if (ok > 0)
                {
                    PopUp popUp = new PopUp("Medium exemplaar", "Medium exemplaar werd toegevoegd!", PopUp.PopupButtonOptions.Ok);
                    popUp.ShowDialog();
                    _v.Close();
                }
                else
                {
                    PopUp pop = new PopUp("Medium exemplaar", "Er ging iets mis, probeer opnieuw!", PopUp.PopupButtonOptions.Ok);
                    pop.ShowDialog();
                }
            }
            else
            {
                PopUp pop = new PopUp("Foutmelding", MediumRecord.Error, PopUp.PopupButtonOptions.Ok);
                pop.ShowDialog();
                MediumRecord.EindeLevensduur = DateTime.Today.AddYears(5);
                MediumRecord.Verkoopprijs = 5;
            }
        }

        private void Terug()
        {
            _v.Close();
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }
    }
}