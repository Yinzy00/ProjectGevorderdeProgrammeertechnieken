using C_Our_Souls_WPF.ViewModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nunit_Tests
{
    public class LoginTesten
    {
        [Test]
        public void ClearVeldenReturnTrue()
        {
            LoginViewModel vm = new LoginViewModel(null);
            vm.Email = "Test";
            vm.Password = "Test";

            vm.Clear();

            Assert.IsTrue(string.IsNullOrEmpty(vm.Email));
            Assert.IsTrue(string.IsNullOrEmpty(vm.Password));
        }
    }
}
