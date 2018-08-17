using EFCodeFirstTest.ModelTests.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCodeFirstTest.ModelTests.Implementations
{
    [TestFixture]
    public class CourseTest : IModelTest
    {
        [OneTimeSetUp]
        public void InitilizeOncePerRun()
        {
            Console.WriteLine("Initial message");
        }
        [OneTimeTearDown]
        public void CleanupOncePerRun()
        {
            Console.WriteLine("Final message");
        }
        [SetUp]
        public void InitializeBeforeEachTest()
        {
           
        }
        [TearDown]
        public void CleanupAfterEachTest()
        {
            
        }
    }
}
