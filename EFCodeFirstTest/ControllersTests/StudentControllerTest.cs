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
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using EFCodeFirstTest.Helpers;

namespace EFCodeFirstTest.ControllersTests
{
    [TestFixture]
    public class StudentControllerTest : BaseControllerTest
    {
        #region private members
        private Mock<IRepository<Student>> _fakeRepo = null;
        private Mock<DbSet<Student>> _fakeDbSet = null;
        private static byte inexistentStudentID = 0;
        #endregion private members
        #region Setup And TearDown
        [OneTimeSetUp]
        protected override void  InitializeOncePerRun()
        {
            var data = DataHelper.GenerateStudentsList();
            _fakeDbSet = new Mock<DbSet<Student>>().SetupData(data);
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
        protected void CleanupOncePerRun()
        {
            _fakeContext.Reset();
            _fakeRepo.Reset();
            _fakeUnitOfWork.Reset();
        }

        protected override void InitializeContext()
        {
            _fakeContext.Reset();
            _fakeContext.SetupGet(c => c.Students).Returns(_fakeDbSet.Object);
        }

        protected override void InitializeRepository()
        {
            _fakeRepo.Reset();
            _fakeRepo.SetupGet<IEnumerable<Student>>(r => r.DataSet).Returns(_fakeContext.Object.Students);
        }

        protected override void InitializeUOF()
        {
            _fakeUnitOfWork.Reset();
            _fakeUnitOfWork.SetupGet<IRepository<Student>>(u => u.StudentRepo).Returns(_fakeRepo.Object);
        }

        #endregion Setup And TearDown
        #region  privateHelpMethods
        

        /// <summary>
        /// //create fake context with data in memory
        /// </summary>
        /// <returns></returns>
        protected override Mock<IContext> generateFakeContextWithData()
        {
            var context = new Mock<IContext>();
            //context.Setup(sc => sc.Set<Student>()).Returns(set.Object); 
            context.SetupGet(c => c.Students).Returns(_fakeDbSet.Object);
            return context;
        }

        /// <summary>
        /// Configure Fake Repository to return an student given an specific ID
        /// </summary>
        /// <param name="studentID"></param>
        protected override void configureRepository(int studentID)
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
        public override void ShouldRenderIndexViewWithModel()
        {
            var sut = new StudentController(_fakeUnitOfWork.Object);
            var ds = _fakeUnitOfWork.Object.StudentRepo.DataSet;
            sut.WithCallTo(x => x.Index()).ShouldRenderDefaultView().WithModel<IEnumerable<Student>>(ds);
        }
        #endregion Index
        #region Details
        [Test]
        public override void DetailsShouldReturnBadRequestResultForNullID()
        {
            var sut = new StudentController();
            sut.WithCallTo(x => x.Details(null)).ShouldGiveHttpStatus(System.Net.HttpStatusCode.BadRequest);
        }
        
        [TestCase(-1)]
        [TestCase(-2)]
        public override void DetailsShouldReturnNotFoundResultForInexistentID(int studentID)
        {
            configureRepository(studentID);
            var sut = new StudentController(_fakeUnitOfWork.Object);
            sut.WithCallTo(x => x.Details(studentID)).ShouldGiveHttpStatus(System.Net.HttpStatusCode.NotFound);
            InitializeRepository();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectID">a Student instance</param>
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public override void DetailsShouldReturnObjecttForValidID(int studentID)
        {
            //set
            configureRepository(studentID);
            var sut = new StudentController(_fakeUnitOfWork.Object);
            //Action and Assert
            sut.WithCallTo(x => x.Details(studentID)).ShouldRenderDefaultView().WithModel<Student>().AndNoModelErrors();
            InitializeRepository();
        }
        #endregion Details
        #region Create
        [Test]
        public override void CreateShouldRenderDefaultView()
        {
            var sut = new StudentController();
            sut.WithCallTo(x => x.Create()).ShouldRenderDefaultView();
        }

        /// <summary>
        /// Validate Model
        /// No model errors
        /// return proper view with student containing the dynamically generated email
        /// </summary>
        [Test]
        public override void CreatePostShouldRedirectToIndex_Success()
        {
            var mockStudent = new Mock<Student>();
            mockStudent.Setup(s => s.GenerateEmailFromName(It.IsAny<string>())).Callback((string schoolDomain) => {});
            _fakeRepo.Setup(r => r.Create(It.IsAny<Student>())).Callback((Student std) => {});
            _fakeUnitOfWork.Setup(u => u.Commit()).Callback(() =>{});
            var sut = new StudentController(_fakeUnitOfWork.Object);
            sut.WithCallTo(x => x.Create(mockStudent.Object)).ShouldRedirectTo(x => x.Index);
            InitializeRepository();
            InitializeUOF();
        }
        [Test]
        public override void CreatePostGetInvalidModelError()
        {
            var sut = new StudentController(_fakeUnitOfWork.Object);
            //Add error to ModelState
            sut.ModelState.AddModelError("FirstMidNameRequiredError", "Model is not valid, verify required fields");
            sut.WithCallTo(x => x.Create(new Student())).ShouldRenderDefaultView().WithModel<Student>().AndModelError("FirstMidNameRequiredError");
        }

        [Test]
        public override void CreatePostGetExceptionError()
        {
            //Add error to ModelState
            _fakeUnitOfWork.SetupGet(u => u.StudentRepo).Throws(new Exception("ExceptionMsg"));
            var std = new Student { ID = 23 };
            var sut = new StudentController(_fakeUnitOfWork.Object);
            sut.WithCallTo(x => x.Create(std)).ShouldRenderDefaultView().WithModel<Student>().AndModelError("ExecptionError");
            InitializeUOF();
        }
        #endregion Create
        #region Edit
        [Test]
        public override void EditGetShouldReturnBadRequestResponse()
        {
            var sut = new StudentController();
            sut.WithCallTo(x => x.Edit(null)).ShouldGiveHttpStatus(HttpStatusCode.BadRequest);
        }

        [Test]
        public override void EditGetShouldReturnNotFoundResponse()
        {
            _fakeRepo.Setup(r => r.GetById(It.IsAny<int>())).Returns((Student)null);
            var sut = new StudentController(_fakeUnitOfWork.Object);
            sut.WithCallTo(x => x.Edit(It.IsAny<int>())).ShouldGiveHttpStatus(HttpStatusCode.NotFound);
            InitializeRepository();
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public override void EditGetShouldRenderDefaultView(int studentID)
        {
            configureRepository(studentID);
            var sut = new StudentController(_fakeUnitOfWork.Object);
            sut.WithCallTo(x => x.Edit(studentID)).ShouldRenderDefaultView().WithModel<Student>().AndNoModelErrors();
            InitializeRepository();
        }

        /// <summary>
        /// Edit, Post
        /// </summary>
        [Test]
        public override void EditPostShouldReturnBadRequestResponse()
        {
            var sut = new StudentController();
            sut.WithCallTo(x => x.EditPost(null)).ShouldGiveHttpStatus(HttpStatusCode.BadRequest);
        }

        [Test]
        public override void EditPostShouldReturnNullModelError()
        {
            var sut = new StudentController(_fakeUnitOfWork.Object);
            sut.ControllerContext = CreateControllerContextObject();
            sut.ValueProvider = new FormCollection().ToValueProvider();
            sut.WithCallTo(x => x.EditPost(0)).ShouldRenderDefaultView().WithModel<Student>().AndModelError("NullModelError");
        }

        [TestCase(1)]
        public override void EditPostShouldReturnSaveChangesExceptionError(int studentID)
        {
            configureRepository(studentID);
            var sut = new StudentController(_fakeUnitOfWork.Object);
            sut.WithCallTo(x => x.EditPost(studentID)).ShouldRenderDefaultView().WithModel<Student>().AndModelError("SaveChangesExceptionError");
            InitializeRepository();
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public override void EditPost_ShouldRedirectToIndex_Success(int studentID)
        {
            configureRepository(studentID);
            var sut = new StudentController(_fakeUnitOfWork.Object);
            sut.ControllerContext = CreateControllerContextObject();
            sut.ValueProvider = new FormCollection().ToValueProvider();
            //In this case, index does not have parameters
            sut.WithCallTo(x => x.EditPost(studentID)).ShouldRedirectTo(x => x.Index);
            InitializeRepository();
        }
        #endregion Edit
        #region Delete
        [Test]
        public override void DeleteGet_ShouldReturnBadRequestResponse()
        {
            var sut = new StudentController();
            sut.WithCallTo(x => x.Delete(null,false)).ShouldGiveHttpStatus(HttpStatusCode.BadRequest);
        }

        [TestCase(true)]
        [TestCase(false)]
        public override void DeleteGet_ShouldReturnNotFoundResponse(bool saveChangesError)
        {
            var sut = new StudentController(_fakeUnitOfWork.Object);
            sut.WithCallTo(x => x.Delete(inexistentStudentID, saveChangesError)).ShouldGiveHttpStatus(HttpStatusCode.NotFound);
        }

        [TestCase(1,true)]
        [TestCase(1,false)]
        [TestCase(2,true)]
        [TestCase(2,false)]
        public override void DeleteGet_ShouldReturnNotFoundResponse(int studentID, bool saveChangesError)
        {
            configureRepository(studentID);
            var sut = new StudentController(_fakeUnitOfWork.Object);
            sut.WithCallTo(x => x.Delete(studentID, saveChangesError)).ShouldRenderDefaultView().WithModel<Student>().AndNoModelErrors();
            InitializeRepository();
        }
        
        /// <summary>
        /// Delete, POST, not in nov
        /// </summary>
        /// <param name="studentID"></param>
        /// <param name="saveChangesError"></param>
        [Test]
        public override void DeletePost_ShouldReturnBadRequestResponse()
        {
            var sut = new StudentController();
            sut.WithCallTo(x => x.Delete_Post(null)).ShouldGiveHttpStatus(HttpStatusCode.BadRequest);
        }

        [Test]
        public override void DeletePost_ShouldReturnNotFoundResponse()
        {
            var sut = new StudentController(_fakeUnitOfWork.Object);
            sut.WithCallTo(x => x.Delete_Post(inexistentStudentID)).ShouldGiveHttpStatus(HttpStatusCode.NotFound);
        }

        [Test, Property("InitializeUOF", 1)]
        public override void DeletePost_ShouldRedirectToDeleteGetForException()
        {
            var sut = new StudentController(_fakeUnitOfWork.Object);
            _fakeUnitOfWork.SetupGet(u => u.StudentRepo).Throws(new Exception());
            // also works: _fakeUnitOfWork.SetupGet(u => u.StudentRepo).Returns((Repository<Student>)null);
            sut.WithCallTo(x => x.Delete_Post(inexistentStudentID)).ShouldRedirectTo<int?,bool?>( x => x.Delete);
            InitializeUOF();
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public override void DeletePost_ShouldRedirectToIndex_Success(int studentID)
        {
            configureRepository(studentID);
            var sut = new StudentController(_fakeUnitOfWork.Object);
            _fakeRepo.Setup(r => r.Delete(It.IsAny<Student>())).Callback( (Student student) => { });
            _fakeUnitOfWork.Setup(u => u.Commit()).Callback(() => { });
            sut.WithCallTo(x => x.Delete_Post(studentID)).ShouldRedirectTo(x => x.Index);
            InitializeRepository();
            InitializeUOF();
        }
        #endregion
    }
}
