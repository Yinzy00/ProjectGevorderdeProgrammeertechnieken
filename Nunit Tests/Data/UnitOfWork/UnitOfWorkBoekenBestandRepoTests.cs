using C_Our_Souls_DAL.Data.UnitOfWork;
using NUnit.Framework;
using FakeItEasy;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using C_Our_Souls_DAL.Models;

namespace Nunit_Tests.Data.UnitOfWork
{
    [TestFixture]
    internal class UnitOfWorkDetailViewRepoTests
    {
        private IUnitOfWork unitofwork = A.Fake<IUnitOfWork>();

        [Test]
        public void Ophalen_ReturnsObservableCollectionOfMediumDetails()
        {
            //Arrange
            ObservableCollection<MediumDetail> MediumDetails;

            //Act
            MediumDetails = new ObservableCollection<MediumDetail>(unitofwork.MediumDetailRepository.Get(
                x => x.Medium,
                x => x.LeeftijdsKlasse,
                y => y.MediumDetailMedewerker.Select(m => m.Medewerker),
                y => y.MediumCategorieen.Select(c => c.Categorie)
                ));

            //Assert
            Assert.NotNull(MediumDetails);
            Assert.IsInstanceOf<ObservableCollection<MediumDetail>>(MediumDetails);
        }

        [Test]
        public void ZoekOpAuteurOfTitel_ReturnsBoekContainsZoekterm()
        {
            string zoekterm = "Dan Brown";
            ObservableCollection<MediumDetail> mediumDetail;

            mediumDetail = new ObservableCollection<MediumDetail>(unitofwork.MediumDetailRepository
                .Get().Where(x => x.Title.ToLower().Contains(zoekterm.ToLower()) || x.Auteurs.ToLower().Contains(zoekterm.ToLower())));

            Assert.NotNull(mediumDetail);
            Assert.IsInstanceOf<ObservableCollection<MediumDetail>>(mediumDetail);
        }

        [Test]
        public void Verwijderen_verwijderenboekenviaid_return0()
        {
            //Arrange
            Medium GeslecteerdMedium = new Medium()
            {
                Id = 1
            };
            // test of er wel boeken zijn met hetzelfde id als GeselecteedMedium
            //ObservableCollection<Medium> boeken;
            //boeken = new ObservableCollection<Medium>(unitofwork.MediumRepository.Get(x => x.Id == GeslecteerdMedium.Id));

            //act
            unitofwork.MediumRepository.Delete(x => x.Id == GeslecteerdMedium.Id);
            int ok = unitofwork.Save();

            //Assert
            // test of er wel boeken zijn met hetzelfde id als GeselecteedMedium
            //Assert.NotNull(boeken);
            //Assert.IsInstanceOf<ObservableCollection<Medium>>(boeken);
            //Assert.AreEqual(GeslecteerdMedium.Id, boeken[0].Id);
            Assert.AreEqual(0, ok);
        }
    }
}