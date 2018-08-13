using EFApproaches.Controllers;
using EFApproaches.DAL.Entities;
using EFApproaches.DAL.Implementations;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.FluentMVCTesting;
using EntityFramework.Testing;
using System.Data.Entity;
using EFApproaches.DAL.Interfaces;

namespace EFCodeFirstTest.ControllersTests
{
    [TestFixture]
    public class StudentControllerTest
    {
        #region Setup And TearDown
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
        public void InitializeBeforeEachTest(){ }
        [TearDown]
        public void CleanupAfterEachTestFuncto() { }
        #endregion Setup And TearDown
        #region  privateHelpMethods
        /// <summary>
        ///generate list of data in memory
        /// </summary>
        /// <returns></returns>
        private List<Student> generateStudentsList()
        {
            var data = new List<Student>
            {
                new Student {ID = 1, FirstMidName = "Nathan", LastName = "Eldridge" },
                new Student {ID = 2, FirstMidName = "Samir", LastName = "Lakhani" },
                new Student {ID = 3, FirstMidName = "Camille", LastName = "Lozerone" },
                new Student {ID = 4, FirstMidName = "John", LastName = "Papa" },
            };
            return data;
        }

        /// <summary>
        /// //create fake context with data in memory
        /// </summary>
        /// <returns></returns>
        private Mock<IContext> generateFakeContextWithData()
        {
            var data = generateStudentsList();
            //create a mock for DbSet of students
            var set = new Mock<DbSet<Student>>()
                .SetupData(data);

            var context = new Mock<IContext>();
            //context.Setup(sc => sc.Set<Student>()).Returns(set.Object); 
            context.SetupGet(c => c.Students).Returns(set.Object);
            return context;
        }
        #endregion  privateHelpMethods

        #region Index
        [Test]
        public void ShouldRenderIndexView()
        {
            var context = generateFakeContextWithData();
            //*--> change it to call contructor with Unit of work as parameter
            var sut = new StudentController((SchoolContext)context.Object);
            // Check the type of the model
            sut.WithCallTo(x => x.Index()).ShouldRenderDefaultView().WithModel<DbSet<Student>>();
        }
        #endregion Index
        #region Details_ID
        [Test]
        public void DetailsShouldReturnBadRequestResultForNullID()
        {
            var sut = new StudentController();
            sut.WithCallTo(x => x.Details(null)).ShouldGiveHttpStatus(System.Net.HttpStatusCode.BadRequest);
        }

        [Test]
        public void DetailsShouldReturnNotFoundResultForInexistentID()
        {
            var sut = new StudentController();
            sut.WithCallTo(x => x.Details(-1)).ShouldGiveHttpStatus(System.Net.HttpStatusCode.NotFound);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void DetailsShouldReturnStudentForValidID(int studentID)
        {
            var fakeContext = generateFakeContextWithData();
            //Create a new MOck of repository passing it the fake context.
            var fakeRepo = new Mock<IRepository<Student>>(fakeContext);
            //create faje Unit of Work
            var fakeUnitOfWork = new Mock<IUnitOfWork>();
            //configure unit of work to 
            fakeUnitOfWork.Setup(u => u.StudentRepo).Returns(fakeRepo.Object);
            var sut = new StudentController(fakeUnitOfWork.Object);
            sut.WithCallTo(x => x.Details(1)).ShouldRenderDefaultView().WithModel<Student>().AndNoModelErrors();
        }
        #endregion Details_ID
    }
}
