using C_Our_Souls_DAL.Data.UnitOfWork;
using C_Our_Souls_DAL.Models;
using C_Our_Souls_WPF.Components;
using C_Our_Souls_WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using static C_Our_Souls_WPF.Views.PopUp;

namespace C_Our_Souls_WPF.ViewModels
{
    public class BoekenbeursGebruikerViewModel : BasisViewModel
    {
        private List<CardBoekenbeurs> Cards = new List<CardBoekenbeurs>();
        private IUnitOfWork _uow = new UnitOfWork(new C_Our_Souls_DAL.Data.DatabaseContext());
        private BoekenbeursGebruiker _v = new BoekenbeursGebruiker();

        public BoekenbeursGebruikerViewModel(BoekenbeursGebruiker v)
        {
            _v = v;
            //Test data
            //for (int i = 0; i < 30; i++)
            //{
            //    var x = new C_Our_Souls_DAL.Models.Boekenbeurs()
            //    {
            //        DatumTot = DateTime.Now,
            //        DatumVan = DateTime.Now,
            //        Naam = "Titel" + i,
            //        InkomPrijs = 3,
            //        Id = 1
            //    };
            //    _uow.BoekenbeursRepository.Add(x);
            //}
            //_uow.Save();
            var Beurzen = _uow.BoekenbeursRepository.Get().ToList();

            double rows = Math.Ceiling((double)Beurzen.Count() / 2);
            for (int i = 0; i < rows; i++)
            {
                v.Grid.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < 2; i++)
            {
                v.Grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            //g.ShowGridLines = true;

            int teller = 0;
            for (int row = 1; row <= rows; row++)
            {
                for (int col = 0; col < 2; col++)
                {
                    if (teller < Beurzen.Count)
                    {
                        var currentBeurs = Beurzen[teller];
                        //Create card per "boekenbeurs"
                        var component = new CardBoekenbeurs(currentBeurs)
                        {
                            Margin = new Thickness(0, 15, 0, 15),
                        };
                        //Set card values
                        component.Control.Name = "uc_" + currentBeurs.Id.ToString();
                        component.ID.Content = "Inschrijven_" + currentBeurs.Id;
                        int currentUserId = (int)App.Current.Properties["CurrentUserId"];
                        if (_uow.GebruikerBoekenbeursRepository.Get(gb => gb.GebruikerId == currentUserId && gb.BoekenbeursId == currentBeurs.Id).FirstOrDefault() != null)
                        {
                            component.btnInschrijven.Content = "Annuleren";
                        }
                        component.lblTitle.Content = currentBeurs.Naam;
                        component.lblDate.Content = currentBeurs.DatumVan.ToString("dd MMMM");
                        component.lblTime.Content = currentBeurs.DatumVan.ToString("hh:mm");
                        if (currentBeurs.DatumTot != null)
                        {
                            component.lblTime.Content += " - " + ((DateTime)currentBeurs.DatumTot).ToString("hh:mm");
                        }
                        component.lblPrice.Content = $"inkomstprijs €{currentBeurs.InkomPrijs} (voor niet leden)";
                        //Add card to list of cards => to be able to use them later
                        Cards.Add(component);
                        //Set card location on grid
                        v.Grid.Children.Add(component);
                        Grid.SetRow(component, row);
                        Grid.SetColumn(component, col);
                        teller++;
                    }
                }
            }
        }

        public override string this[string columnName] => throw new NotImplementedException();

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            if (parameter.ToString().Contains("Inschrijven_"))
            {
                Inschrijven(Convert.ToInt32(parameter.ToString().Split('_')[1]));
            }

            switch (parameter)
            {
                case "HomeButton":
                    DashboardGebruikerView dgv = new DashboardGebruikerView();
                    DashboardGebruikerViewModel dgvm = new DashboardGebruikerViewModel(dgv);
                    dgv.DataContext = dgvm;
                    dgv.Show();
                    _v.Close();
                    break;

                case "AccountOpties":
                    AccountOpties();
                    break;

                default:
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

        public void Inschrijven(int id)
        {
            int currentUserId = (int)App.Current.Properties["CurrentUserId"];
            //Get card form list
            var currentCard = Cards.Where(c => c.Name == "uc_" + id.ToString()).FirstOrDefault();
            if (currentCard != null)
            {
                //Check
                if (currentCard.btnInschrijven.Content.ToString() == "Inschrijven")
                {
                    var popup = new PopUp("Boekenbeurs", "Bent u zeker dat u zich wilt inschrijven voor de boekenbeurs \"" + currentCard.lblTitle.Content + "\"?", PopupButtonOptions.OkCancel);
                    popup.ShowDialog();
                    if (popup.DialogResult == PopUpResponse.Ok)
                    {
                        _uow.GebruikerBoekenbeursRepository.Add(new GebruikerBoekenbeurs()
                        {
                            GebruikerId = currentUserId,
                            BoekenbeursId = id,
                            IngeschrevenOp = DateTime.Now
                        });
                        _uow.Save();
                        currentCard.btnInschrijven.Content = "Annuleren";
                    }
                }
                else if (currentCard.btnInschrijven.Content.ToString() == "Annuleren")
                {
                    //Inschrijving verwijderen
                    var p = new PopUp("Boekenbeurs", "Weet u zeker dat u uw inschrijving voor deze boekenbeurs wil annuleren?", PopupButtonOptions.OkCancel);
                    p.ShowDialog();
                    if (p.DialogResult == PopUp.PopUpResponse.Ok)
                    {
                        _uow.GebruikerBoekenbeursRepository.Delete(gb => gb.BoekenbeursId == id && gb.GebruikerId == currentUserId);
                        _uow.Save();
                        currentCard.btnInschrijven.Content = "Inschrijven";
                    }
                }
            }
        }
    }
}