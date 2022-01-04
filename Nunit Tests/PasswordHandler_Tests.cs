using C_Our_Souls_DAL.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nunit_Tests
{
    public class PasswordHandler_Tests
    {
        [Test]
        public void EncryptPasswordReturnTrue()
        {
            PasswordHandler passwordHandler = new PasswordHandler();
            Gebruiker g = new Gebruiker()
            {
                Key = passwordHandler.Key,
                Wachtwoord = passwordHandler.Encrypt("Admin123_")
            };

            PasswordHandler passwordHandler2 = new PasswordHandler(passwordHandler.Key);
            string password = "Admin123_";


            Assert.AreEqual(g.Wachtwoord, passwordHandler2.Encrypt(password));
        }

        [Test]
        public void DecryptPasswordReturnTrue()
        {
            PasswordHandler passwordHandler = new PasswordHandler();
            Gebruiker g = new Gebruiker()
            {
                Key = passwordHandler.Key,
                Wachtwoord = passwordHandler.Encrypt("Admin123_")
            };

            PasswordHandler passwordHandler2 = new PasswordHandler(passwordHandler.Key);
            string password = "Admin123_";

            Assert.AreEqual(passwordHandler2.Decrypt(g.Wachtwoord), password);
        }
    }
}
