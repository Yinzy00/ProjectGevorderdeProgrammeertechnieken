using C_Our_Souls_DAL.Data.UnitOfWork;
using C_Our_Souls_DAL.Models;
using C_Our_Souls_WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static C_Our_Souls_WPF.Views.PopUp;

namespace C_Our_Souls_WPF.Components
{
    /// <summary>
    /// Interaction logic for CardBoekenbeurs.xaml
    /// </summary>
    public partial class CardBoekenbeurs : UserControl
    {
        IUnitOfWork _uow = new UnitOfWork(new C_Our_Souls_DAL.Data.DatabaseContext());
        Boekenbeurs _b;
        public CardBoekenbeurs(Boekenbeurs b)
        {
            InitializeComponent();
            _b = b;
            ////
            //Control.Name = "uc_" + _b.Id.ToString();
            //ID.Content = "Inschrijven_" + _b.Id;
            //int currentUserId = (int)App.Current.Properties["CurrentUserId"];
            //if (_uow.GebruikerBoekenbeursRepository.Get(gb=>gb.GebruikerId == currentUserId && gb.BoekenbeursId == _b.Id).FirstOrDefault() != null)
            //{
            //    btnInschrijven.Content = "Anuleren";
            //}
            ////string name, DateTime startdate, DateTime endDate, double priceNotMembers
            //lblTitle.Content = b.Naam;
            //lblDate.Content = b.DatumVan.ToString("dd MMMM");
            //lblTime.Content = b.DatumVan.ToString("hh:mm");
            //if (b.DatumTot!= null)
            //{
            //    lblTime.Content += " - " + ((DateTime)b.DatumTot).ToString("hh:mm");
            //}
            //lblPrice.Content = $"inkomstprijs €{b.InkomPrijs} (voor niet leden)";
            ////
        }
        private void btnInschrijven_Click(object sender, RoutedEventArgs e)
        {
            //int currentUserId = (int)App.Current.Properties["CurrentUserId"];
            //if (btnInschrijven.Content.ToString() == "Inschrijven")
            //{
            //    var popup = new PopUp("Boekenbeurs", "Bent u zeker dat u zich wilt inschrijven voor de boekenbeurs \"" + _b.Naam + "\"?", PopupButtonOptions.OkCancel);
            //    popup.ShowDialog();
            //    if (popup.DialogResult == PopUpResponse.Ok)
            //    {
            //        _uow.GebruikerBoekenbeursRepository.Add(new GebruikerBoekenbeurs()
            //        {
            //            GebruikerId = currentUserId,
            //            BoekenbeursId = _b.Id,
            //            IngeschrevenOp = DateTime.Now
            //        });
            //        _uow.Save();
            //        btnInschrijven.Content = "Anuleren";
            //    }
            //}
            //else if (btnInschrijven.Content.ToString() == "Anuleren")
            //{
            //    //Inschrijving verwijderen
            //    var p = new PopUp("Boekenbeurs", "Weet u zeker dat u uw inschrijving voor deze boekenbeurs wil anuleren?", PopupButtonOptions.OkCancel);
            //    p.ShowDialog();
            //    if (p.DialogResult == PopUp.PopUpResponse.Ok)
            //    {
            //        _uow.GebruikerBoekenbeursRepository.Delete(gb => gb.BoekenbeursId == _b.Id && gb.GebruikerId == currentUserId);
            //        _uow.Save();
            //        btnInschrijven.Content = "Inschrijven";
            //    }
            //}
        }
    }
}
