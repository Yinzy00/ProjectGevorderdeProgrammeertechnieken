using C_Our_Souls_DAL.Data.UnitOfWork;
using C_Our_Souls_DAL.Models;
using C_Our_Souls_WPF.ViewModels;
using FakeItEasy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Nunit_Tests.ViewModels
{
    [TestFixture]
    public class AccountRegistratieViewModelTests
    {
        AccountRegistratieViewModel viewModel = new AccountRegistratieViewModel();

        [Test]
        public void Leegmaken_eigenschappen_emptyProperties()
        {
            //Arrange
            viewModel.Voornaam = "Jefke";
            viewModel.Naam = "Peeters";

            //Act
            viewModel.ClearFields();

            //Assert
            Assert.AreEqual(null, viewModel.Voornaam);
            Assert.AreEqual(null, viewModel.Naam);

        }
    }
}