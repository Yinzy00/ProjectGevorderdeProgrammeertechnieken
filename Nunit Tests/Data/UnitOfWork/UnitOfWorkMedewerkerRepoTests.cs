using C_Our_Souls_DAL.Data.UnitOfWork;
using C_Our_Souls_DAL.Models;
using FakeItEasy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nunit_Tests.Data.UnitOfWork
{
    [TestFixture]
    public class UnitOfWorkMedewerkerRepoTests
    {
        private IUnitOfWork unitOfWork = A.Fake<IUnitOfWork>();

        [Test]
        public void Ophalen_Medewerker()
        {
            Medewerker medewerker = A.Fake<Medewerker>();

            medewerker = unitOfWork.MedewerkerRepository.GetById(1);

            Assert.IsNotNull(medewerker);
            Assert.IsNotInstanceOf(typeof(Functie), medewerker);
        }
    }
}
