using EFApproaches.DAL.Entities;
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
    public class TeacherTest : IModelTest
    {
        [OneTimeSetUp]
        public void InitilizeOncePerRun(){ }

        [OneTimeSetUp]
        public void CleanupOncePerRun(){ }

        [SetUp]
        public void CleanupAfterEachTest(){ }

        [SetUp]
        public void InitializeBeforeEachTest(){ }

        [TestCase("Nathan", "Eldridge")]
        [TestCase("Samir", "Lakhani")]
        [TestCase("Camille", "Lozerone")]
        [TestCase("John", "Papa")]
        public void SetFullNameInConstructorTest(string firstMidName, string lastName)
        {
            try
            {
                var sut = new Teacher(firstMidName, lastName);
                Assert.That(sut.FullName, Is.EqualTo(firstMidName + " " + lastName));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
