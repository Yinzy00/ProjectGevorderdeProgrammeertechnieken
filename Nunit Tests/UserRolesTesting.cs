using C_Our_Souls_DAL.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nunit_Tests
{
    public class UserRolesTesting
    {
        public object Boekenebestandviewmodel { get; private set; }

        [Test]
        public void IsGebruiker_User_ReturnTrue()
        {
            Gebruiker g = new Gebruiker()
            {
                Admin = GebruikerType.Gebruiker
            };
            Assert.AreEqual(g.Admin, GebruikerType.Gebruiker);

        }
        [Test]
        public void IsGebruiker_Admin_ReturnTrue()
        {
            Gebruiker g = new Gebruiker()
            {
                Admin = GebruikerType.Beheerder
            };
            Assert.AreEqual(g.Admin, GebruikerType.Beheerder);
        }
        [Test]
        public void IsGebruiker_Super_ReturnTrue()
        {
            Gebruiker g = new Gebruiker()
            {
                Admin = GebruikerType.Super
            };
            Assert.AreEqual(g.Admin, GebruikerType.Super);
        }
    }
}
