using EFApproaches.DAL.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCodeFirstTest.ControllersTests
{
    [TestFixture]
    public abstract class BaseControllerTest
    {
        protected internal Mock<IUnitOfWork> _fakeUnitOfWork = null;
        protected internal Mock<IContext> _fakeContext = null;
        [OneTimeSetUp]
        public abstract void InitializeOncePerRun();
        public abstract Mock<IContext> generateFakeContextWithData();
    }
}
