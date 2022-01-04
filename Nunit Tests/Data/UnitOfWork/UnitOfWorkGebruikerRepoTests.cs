using C_Our_Souls_DAL.Data.UnitOfWork;
using C_Our_Souls_DAL.Models;
using FakeItEasy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Nunit_Tests.Data.UnitOfWork
{
    [TestFixture]
    public class UnitOfWorkGebruikerRepoTests
    {
        private IUnitOfWork unitOfWork = A.Fake<IUnitOfWork>();

        [Test]
        public void Ophalen_ReturnsCurrentUser()
        {
            //Arrange
            Gebruiker gebruiker = new Gebruiker();

            //Act
            gebruiker = unitOfWork.GebruikerRepository.GetById(1);

            //Assert
            Assert.IsNotNull(gebruiker);
            Assert.IsInstanceOf<Gebruiker>(gebruiker);

            A.CallTo(() => unitOfWork.GebruikerRepository.GetById(1)).MustHaveHappened();
        }

        [Test]
        public void Check_ListUsersContainsUser_ReturnsUser()
        {
            //Arrange
            ObservableCollection<Gebruiker> gebruikers = new ObservableCollection<Gebruiker>();
            Gebruiker gebruiker = new Gebruiker();
            PasswordHandler passwordHandler = new PasswordHandler();
            passwordHandler.Key = "PasswordKey";
            string ww = "mijnWachtwoord";

            //Act
            gebruikers = new ObservableCollection<Gebruiker>(unitOfWork.GebruikerRepository.Get());
            gebruiker = new Gebruiker
            {
                Id = 1,
                Voornaam = "Jefke",
                Naam = "Peeters",
                Adres = "Teststraat 2",
                Postcode = "2300",
                Woonplaats = "Turnhout",
                Email = "jefke@mail.be",
                LidmaatschapAanvraag = null,
                Key = "PasswordKey",
                Wachtwoord = passwordHandler.Encrypt(ww)
            };
            gebruikers.Add(gebruiker);

            //Assert
            Assert.Contains(gebruiker, gebruikers);
            Assert.AreEqual(ww, passwordHandler.Decrypt(gebruiker.Wachtwoord));

        }
    }
}
