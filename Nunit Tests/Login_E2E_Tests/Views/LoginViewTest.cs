using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using NUnit.Framework.Internal;
using OpenQA.Selenium.Appium.MultiTouch;

namespace Nunit_Tests.Login_E2E_Tests.Views
{
    [TestFixture]
    public class LoginViewTest
    {
        string WindowsApplicationDriverUri = "http://127.0.0.1:4723";
        string WpfAppId = @"D:\School\2021\Vakken\Programmeren\2021-TD-Bibliotheek-C_Our_Souls\C_Our_Souls_WPF\bin\Debug\C_Our_Souls_WPF.exe";

        WindowsDriver<WindowsElement> driver;
        WindowsElement tbEmail, tbWachtwoord, btAanmelden;
        [SetUp]
        public void Setup()
        {
            
            if (driver == null)
            {
                var appiumOptions = new AppiumOptions();
                appiumOptions.AddAdditionalCapability("app", WpfAppId);
                driver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUri), appiumOptions);

                tbEmail = driver.FindElementByAccessibilityId("tbEmail");
                tbWachtwoord = driver.FindElementByAccessibilityId("tbWachtwoord");
                btAanmelden = driver.FindElementByAccessibilityId("btAanmelden");

                Assert.IsNotNull(driver);
            }
        }

        [Test]
        public void TryLoginReturnTrue()
        {
            //Act 
            tbEmail.SendKeys("user.for@testing.be");
            tbWachtwoord.SendKeys("admin");
            btAanmelden.Click();

            bool loggedin = false;
            try
            {
                tbEmail.Clear();
                loggedin = false;
            }
            catch (Exception)
            {
                loggedin = true;
            }
            Assert.IsTrue(loggedin);
        }

        //[TearDown]
        //public void TearDown()
        //{
            
        //}

        [OneTimeTearDown]
        public void CloseSession()
        {
            driver.Close();
        }
    }
}