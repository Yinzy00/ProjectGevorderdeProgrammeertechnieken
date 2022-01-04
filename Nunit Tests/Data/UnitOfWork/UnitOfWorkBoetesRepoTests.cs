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
    public class UnitOfWorkBoetesRepoTests
    {
        private IUnitOfWork unitOfWork = A.Fake<IUnitOfWork>();
                
        [Test]
        public void Ophalen_ReturnBoetes()
        {
            ObservableCollection<Uitlening> boetes = new ObservableCollection<Uitlening>();

            boetes = new ObservableCollection<Uitlening>(unitOfWork.UitleningRepository.Get(x => x.Medium, x => x.Medium.MediumDetail, x => x.Gebruiker.Lidegelden));

            Assert.IsNotNull(boetes);
            Assert.IsInstanceOf(typeof(ObservableCollection<Uitlening>), boetes);
        }
    }
}
