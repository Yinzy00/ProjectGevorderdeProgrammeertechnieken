using C_Our_Souls_DAL.Data.UnitOfWork;
using FakeItEasy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C_Our_Souls_DAL.Models;
using System.Collections.ObjectModel;

namespace Nunit_Tests.Data.UnitOfWork
{
    [TestFixture]
    internal class MediumExemplaarToevoegenViewModelTest
    {
        private IUnitOfWork unitofwork = A.Fake<IUnitOfWork>();

        [Test]
        public void OpSlagen_OpslagenNieuwExeplaar_ReturnTrue()
        {
            //Arrange
            Medium record = new Medium()
            {
                Id = 5,
                EindeLevensduur = DateTime.Today.AddYears(5),
                Verkoopprijs = 5,
                MediumDetailId = 5
            };

            //Act
            unitofwork.MediumRepository.Add(record);
            int ok = unitofwork.Save();

            //Assert
            Assert.NotNull(ok);
        }
    }
}