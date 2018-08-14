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
using System.Net;

namespace EFCodeFirstTest.ControllersTests
{
    [TestFixture]
    public class StudentControllerTest
    {
        private Mock<IContext> _fakeContext = null;
        private Mock<IRepository<Student>> _fakeRepo = null;
        private Mock<IUnitOfWork> _fakeUnitOfWork = null;
        #region Setup And TearDown
        [OneTimeSetUp]
        public void InitilizeOncePerRun()
        {
            //Console.WriteLine("Initial message");
            _fakeContext = generateFakeContextWithData();
            //Create a new Mock of repository
            _fakeRepo = new Mock<IRepository<Student>>();
            _fakeRepo.SetupGet<IEnumerable<Student>>(r => r.DataSet).Returns(_fakeContext.Object.Students);
            //create fake Unit of Work
            _fakeUnitOfWork = new Mock<IUnitOfWork>();
            //configure unit of work to 
            _fakeUnitOfWork.SetupGet<IRepository<Student>>(u => u.StudentRepo).Returns(_fakeRepo.Object);
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

        private void configureRepository(int studentID)
        {
            try
            {
                _fakeRepo.Setup(r => r.GetById(studentID)).Returns(_fakeContext.Object.Students.FirstOrDefault<Student>(s => s.ID == studentID));
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
        #endregion  privateHelpMethods

        #region Index
        [Test]
        public void ShouldRenderIndexViewWithModel()
        {
            var context = generateFakeContextWithData();
            //*--> change it to call contructor with Unit of work as parameter
            var sut = new StudentController(_fakeUnitOfWork.Object);
            // Check the type of the model
            var ds = _fakeUnitOfWork.Object.StudentRepo.DataSet;
            sut.WithCallTo(x => x.Index()).ShouldRenderDefaultView().WithModel<IEnumerable<Student>>(ds);
            
        }
        #endregion Index
        #region Details_ID
        [Test]
        public void DetailsShouldReturnBadRequestResultForNullID()
        {
            var sut = new StudentController();
            sut.WithCallTo(x => x.Details(null)).ShouldGiveHttpStatus(System.Net.HttpStatusCode.BadRequest);
        }

        [TestCase(-1)]
        [TestCase(-2)]
        public void DetailsShouldReturnNotFoundResultForInexistentID(int studentID)
        {
            configureRepository(studentID);
            var sut = new StudentController(_fakeUnitOfWork.Object);
            sut.WithCallTo(x => x.Details(-1)).ShouldGiveHttpStatus(System.Net.HttpStatusCode.NotFound);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void DetailsShouldReturnStudentForValidID(int studentID)
        {
            //set
            configureRepository(studentID);
            var sut = new StudentController(_fakeUnitOfWork.Object);
            //Action and Assert
            sut.WithCallTo(x => x.Details(studentID)).ShouldRenderDefaultView().WithModel<Student>().AndNoModelErrors();
        }
        #endregion Details_ID

        #region Create
        [Test]
        public void CreateShouldRenderDefaultView()
        {
            var sut = new StudentController();
            sut.WithCallTo(x => x.Create()).ShouldRenderDefaultView();
        }

        /// <summary>
        /// Validate Model
        /// No model errors
        /// return proper view
        /// </summary>
        [Test]
        public void CreatePostShouldRenderDefaultView()
        {
            var mockStudent = new Mock<Student>();
            mockStudent.Setup(s => s.GenerateEmailFromName(It.IsAny<string>())).Callback((string schoolDomain) => {});
            _fakeRepo.Setup(r => r.Create(It.IsAny<Student>())).Callback((Student std) => {});
            _fakeUnitOfWork.Setup(u => u.Commit()).Callback(() =>{});
            var sut = new StudentController(_fakeUnitOfWork.Object);
            sut.WithCallTo(x => x.Create(mockStudent.Object)).ShouldRedirectTo(x => x.Index);
        }
        [Test]
        public void CreatePostGetInvalidModelError()
        {
            var sut = new StudentController(_fakeUnitOfWork.Object);
            //Add error to ModelState
            sut.ModelState.AddModelError("FirstMidNameRequiredError", "Model is not valid, verify required fields");
            sut.WithCallTo(x => x.Create(new Student())).ShouldRenderDefaultView().WithModel<Student>().AndModelError("FirstMidNameRequiredError");
        }

        [Test]
        public void CreatePostGetExceptionError()
        {
            //Add error to ModelState
            _fakeUnitOfWork.SetupGet(u => u.StudentRepo).Throws(new Exception("ExceptionMsg"));
            var std = new Student { ID = 23 };
            var sut = new StudentController(_fakeUnitOfWork.Object);
            sut.WithCallTo(x => x.Create(std)).ShouldRenderDefaultView().WithModel<Student>().AndModelError("ExecptionError");
        }
        #endregion Create

        #region Edit
        [Test]
        public void EditGetShouldReturnBadRequestResponse()
        {
            var sut = new StudentController();
            sut.WithCallTo(x => x.Edit(null)).ShouldGiveHttpStatus(HttpStatusCode.BadRequest);
        }

        [Test]
        public void EditGetShouldReturnNotFoundResponse()
        {
            _fakeRepo.Setup(r => r.GetById(It.IsAny<int>())).Returns((Student)null);
            var sut = new StudentController(_fakeUnitOfWork.Object);
            sut.WithCallTo(x => x.Edit(It.IsAny<int>())).ShouldGiveHttpStatus(HttpStatusCode.NotFound);
        }

        [Test]
        public void EditGetShouldRenderDefaultView()
        {
            _fakeRepo.Setup(r => r.GetById(It.IsAny<int>())).Returns((Student)null);
            var sut = new StudentController(_fakeUnitOfWork.Object);

        }
        #endregion Edit
    }
}
