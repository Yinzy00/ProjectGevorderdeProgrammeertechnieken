using C_Our_Souls_WPF.ViewModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nunit_Tests
{
    [TestFixture]
    public class BoekenbeursAanmakenTests
    {
        [Test]
        public void BoekenbeursAanmaken_Constructor_SetsDatumVanToDateTimeToday()
        {
            BoekenbeursAanmakenViewModel viewModel = new BoekenbeursAanmakenViewModel(null);

            //Assert
            Assert.AreEqual(DateTime.Today, viewModel.BoekenbeursRecord.DatumVan);
        }

        [Test]
        public void BoekenbeursAanmaken_Constructor_SetsLocatieToDisabled()
        {
            BoekenbeursAanmakenViewModel viewModel = new BoekenbeursAanmakenViewModel(null);

            //Assert
            Assert.AreEqual(false, viewModel.LocatieEnabled);
        }

        [Test]
        public void BoekenbeursAanmaken_Constructor_SetsButtonTekstToAanmaken()
        {
            BoekenbeursAanmakenViewModel viewModel = new BoekenbeursAanmakenViewModel(null);

            Assert.AreEqual("AANMAKEN", viewModel.ButtonTekst);
        }

        [Test]
        public void BoekenbeursAanmaken_Constructor_SetsCmbToVisible()
        {
            BoekenbeursAanmakenViewModel viewModel = new BoekenbeursAanmakenViewModel(null);

            Assert.AreEqual("Visible", viewModel.Cmb);
        }

        [Test]
        public void BoekenbeursAanmaken_Constructor_VultUrenLijstOp()
        {
            BoekenbeursAanmakenViewModel viewModel = new BoekenbeursAanmakenViewModel(null);

            List<string> urenlijst = new List<string>();
            for (int i = 5; i < 24; i++)
            {
                urenlijst.Add(i.ToString());
            }

            Assert.AreEqual(urenlijst, viewModel.Uren);
        }
    }
}