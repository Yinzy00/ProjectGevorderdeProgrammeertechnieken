using C_Our_Souls_DAL.Data.UnitOfWork;
using NUnit.Framework;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using C_Our_Souls_DAL.Models;

namespace Nunit_Tests.Data.UnitOfWork
{
    [TestFixture]
    internal class UnitOfWorkInschrijvenBoekenbeursRepoTests
    {
        private IUnitOfWork unitofwork = A.Fake<IUnitOfWork>();

        [Test]
        public void Ophalen_ReturnsObservableCollectionOfBoekenbeursById()
        {
            //Arrange
            int boekenbeursID = 5;
            ObservableCollection<GebruikerBoekenbeurs> gebruikersBoekenbeurs;
            //Act
            gebruikersBoekenbeurs = new ObservableCollection<GebruikerBoekenbeurs>(unitofwork.GebruikerBoekenbeursRepository.Get(x => x.BoekenbeursId == boekenbeursID, x => x.Gebruiker));

            //Assert
            Assert.NotNull(gebruikersBoekenbeurs);
            Assert.IsInstanceOf<ObservableCollection<GebruikerBoekenbeurs>>(gebruikersBoekenbeurs);
        }
    }
}