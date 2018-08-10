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
    public class BaseControllerTest
    {
        protected internal Mock<IUnitOfWork> _mockUnitOfWork;

        [OneTimeSetUp]
        public void InitilizeOncePerRun()
        {
            Console.WriteLine("Initial message");
        }
    }
}
