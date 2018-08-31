using EFApproaches.Controllers;
using EFApproaches.DAL.Entities;
using EFApproaches.DAL.Interfaces;
using EFCodeFirstTest.Helpers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TestStack.FluentMVCTesting;

namespace EFCodeFirstTest.ControllersTests
{
    public class TeacherControllerTest : BaseControllerTest
    {
        #region private members
        private Mock<IRepository<Teacher>> _fakeRepo = null;
        private Mock<DbSet<Teacher>> _fakeDbSet = null;
        private static byte inexistentTeacherID = 0;
        private static int invalidTeacherID = 23;
        #endregion private members
        #region Setup And TearDown
        [OneTimeSetUp]
        protected override void InitializeOncePerRun()
        {
            var data = DataHelper.GenerateTeachersList();
            _fakeDbSet = new Mock<DbSet<Teacher>>().SetupData(data);
            //Console.WriteLine("Initial message");
            _fakeContext = generateFakeContextWithData();
            //Create a new Mock of repository
            _fakeRepo = new Mock<IRepository<Teacher>>();
            _fakeRepo.SetupGet<IEnumerable<Teacher>>(r => r.DataSet).Returns(_fakeContext.Object.Teachers);
            //create fake Unit of Work
            _fakeUnitOfWork = new Mock<IUnitOfWork>();
            //configure unit of work to 
            _fakeUnitOfWork.SetupGet<IRepository<Teacher>>(u => u.TeacherRepo).Returns(_fakeRepo.Object);
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
            _fakeContext.SetupGet(c => c.Teachers).Returns(_fakeDbSet.Object);
        }

        protected override void InitializeRepository()
        {
            _fakeRepo.Reset();
            _fakeRepo.SetupGet<IEnumerable<Teacher>>(r => r.DataSet).Returns(_fakeContext.Object.Teachers);
        }

        protected override void InitializeUOF()
        {
            _fakeUnitOfWork.Reset();
            _fakeUnitOfWork.SetupGet<IRepository<Teacher>>(u => u.TeacherRepo).Returns(_fakeRepo.Object);
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
            //context.Setup(sc => sc.Set<Teacher>()).Returns(set.Object); 
            context.SetupGet(c => c.Teachers).Returns(_fakeDbSet.Object);
            return context;
        }

        /// <summary>
        /// Configure Fake Repository to return an Teacher given an specific ID
        /// </summary>
        /// <param name="TeacherID"></param>
        protected override void configureRepository(int TeacherID)
        {
            try
            {
                _fakeRepo.Setup(r => r.GetById(TeacherID)).Returns(_fakeContext.Object.Teachers.FirstOrDefault<Teacher>(s => s.ID == TeacherID));
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
            var sut = new TeacherController(_fakeUnitOfWork.Object);
            var ds = _fakeUnitOfWork.Object.TeacherRepo.DataSet;
            sut.WithCallTo(x => x.Index()).ShouldRenderDefaultView().WithModel<IEnumerable<Teacher>>(ds);
        }
        #endregion Index
        #region Details
        [Test]
        public override void DetailsShouldReturnBadRequestResultForNullID()
        {
            var sut = new TeacherController();
            sut.WithCallTo(x => x.Details(null)).ShouldGiveHttpStatus(System.Net.HttpStatusCode.BadRequest);
        }

        [TestCase(-1)]
        [TestCase(-2)]
        public override void DetailsShouldReturnNotFoundResultForInexistentID(int TeacherID)
        {
            configureRepository(TeacherID);
            var sut = new TeacherController(_fakeUnitOfWork.Object);
            sut.WithCallTo(x => x.Details(TeacherID)).ShouldGiveHttpStatus(System.Net.HttpStatusCode.NotFound);
            InitializeRepository();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectID">a Teacher instance</param>
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public override void DetailsShouldReturnObjecttForValidID(int TeacherID)
        {
            //set
            configureRepository(TeacherID);
            var sut = new TeacherController(_fakeUnitOfWork.Object);
            //Action and Assert
            sut.WithCallTo(x => x.Details(TeacherID)).ShouldRenderDefaultView().WithModel<Teacher>().AndNoModelErrors();
            InitializeRepository();
        }
        #endregion Details
        #region Create
        [Test]
        public override void CreateShouldRenderDefaultView()
        {
            var sut = new TeacherController();
            sut.WithCallTo(x => x.Create()).ShouldRenderDefaultView();
        }

        /// <summary>
        /// Validate Model
        /// No model errors
        /// return proper view with Teacher containing the dynamically generated email
        /// </summary>
        [Test]
        public override void CreatePostShouldRedirectToIndex_Success()
        {
            var mockTeacher = new Mock<Teacher>();
            _fakeRepo.Setup(r => r.Create(It.IsAny<Teacher>())).Callback((Teacher std) => { });
            _fakeUnitOfWork.Setup(u => u.Commit()).Callback(() => { });
            var sut = new TeacherController(_fakeUnitOfWork.Object);
            sut.WithCallTo(x => x.Create(mockTeacher.Object)).ShouldRedirectTo(x => x.Index);
            InitializeRepository();
            InitializeUOF();
        }
        [Test]
        public override void CreatePostGetInvalidModelError()
        {
            var sut = new TeacherController(_fakeUnitOfWork.Object);
            //Add error to ModelState
            sut.ModelState.AddModelError("FirstMidNameRequiredError", "Model is not valid, verify required fields");
            sut.WithCallTo(x => x.Create(new Teacher())).ShouldRenderDefaultView().WithModel<Teacher>().AndModelError("FirstMidNameRequiredError");
        }

        [Test]
        public override void CreatePostGetExceptionError()
        {
            //Add error to ModelState
            _fakeUnitOfWork.SetupGet(u => u.TeacherRepo).Throws(new Exception("ExceptionMsg"));
            var std = new Teacher { ID = invalidTeacherID };
            var sut = new TeacherController(_fakeUnitOfWork.Object);
            sut.WithCallTo(x => x.Create(std)).ShouldRenderDefaultView().WithModel<Teacher>().AndModelError("ExecptionError");
            InitializeUOF();
        }
        #endregion Create
        #region Edit
        [Test]
        public override void EditGetShouldReturnBadRequestResponse()
        {
            var sut = new TeacherController();
            sut.WithCallTo(x => x.Edit(null)).ShouldGiveHttpStatus(HttpStatusCode.BadRequest);
        }

        [Test]
        public override void EditGetShouldReturnNotFoundResponse()
        {
            _fakeRepo.Setup(r => r.GetById(It.IsAny<int>())).Returns((Teacher)null);
            var sut = new TeacherController(_fakeUnitOfWork.Object);
            sut.WithCallTo(x => x.Edit(It.IsAny<int>())).ShouldGiveHttpStatus(HttpStatusCode.NotFound);
            InitializeRepository();
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public override void EditGetShouldRenderDefaultView(int TeacherID)
        {
            configureRepository(TeacherID);
            var sut = new TeacherController(_fakeUnitOfWork.Object);
            sut.WithCallTo(x => x.Edit(TeacherID)).ShouldRenderDefaultView().WithModel<Teacher>().AndNoModelErrors();
            InitializeRepository();
        }

        /// <summary>
        /// Edit, Post
        /// </summary>
        [Test]
        public override void EditPostShouldReturnBadRequestResponse()
        {
            var sut = new TeacherController();
            sut.WithCallTo(x => x.EditPost(null)).ShouldGiveHttpStatus(HttpStatusCode.BadRequest);
        }

        [Test]
        public override void EditPostShouldReturnNullModelError()
        {
            var sut = new TeacherController(_fakeUnitOfWork.Object);
            sut.ControllerContext = CreateControllerContextObject();
            sut.ValueProvider = new FormCollection().ToValueProvider();
            sut.WithCallTo(x => x.EditPost(0)).ShouldRenderDefaultView().WithModel<Teacher>().AndModelError("NullModelError");
        }

        [TestCase(1)]
        public override void EditPostShouldReturnSaveChangesExceptionError(int TeacherID)
        {
            configureRepository(TeacherID);
            var sut = new TeacherController(_fakeUnitOfWork.Object);
            sut.WithCallTo(x => x.EditPost(TeacherID)).ShouldRenderDefaultView().WithModel<Teacher>().AndModelError("SaveChangesExceptionError");
            InitializeRepository();
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public override void EditPost_ShouldRedirectToIndex_Success(int TeacherID)
        {
            configureRepository(TeacherID);
            var sut = new TeacherController(_fakeUnitOfWork.Object);
            sut.ControllerContext = CreateControllerContextObject();
            sut.ValueProvider = new FormCollection().ToValueProvider();
            //In this case, index does not have parameters
            sut.WithCallTo(x => x.EditPost(TeacherID)).ShouldRedirectTo(x => x.Index);
            InitializeRepository();
        }
        #endregion Edit
        #region Delete
        [Test]
        public override void DeleteGet_ShouldReturnBadRequestResponse()
        {
            var sut = new TeacherController();
            sut.WithCallTo(x => x.Delete(null, false)).ShouldGiveHttpStatus(HttpStatusCode.BadRequest);
        }

        [TestCase(true)]
        [TestCase(false)]
        public override void DeleteGet_ShouldReturnNotFoundResponse(bool saveChangesError)
        {
            var sut = new TeacherController(_fakeUnitOfWork.Object);
            sut.WithCallTo(x => x.Delete(inexistentTeacherID, saveChangesError)).ShouldGiveHttpStatus(HttpStatusCode.NotFound);
        }

        [TestCase(1, true)]
        [TestCase(1, false)]
        [TestCase(2, true)]
        [TestCase(2, false)]
        public override void DeleteGet_ShouldReturnNotFoundResponse(int TeacherID, bool saveChangesError)
        {
            configureRepository(TeacherID);
            var sut = new TeacherController(_fakeUnitOfWork.Object);
            sut.WithCallTo(x => x.Delete(TeacherID, saveChangesError)).ShouldRenderDefaultView().WithModel<Teacher>().AndNoModelErrors();
            InitializeRepository();
        }

        /// <summary>
        /// Delete, POST, not in nov
        /// </summary>
        /// <param name="TeacherID"></param>
        /// <param name="saveChangesError"></param>
        [Test]
        public override void DeletePost_ShouldReturnBadRequestResponse()
        {
            var sut = new TeacherController();
            sut.WithCallTo(x => x.Delete_Post(null)).ShouldGiveHttpStatus(HttpStatusCode.BadRequest);
        }

        [Test]
        public override void DeletePost_ShouldReturnNotFoundResponse()
        {
            var sut = new TeacherController(_fakeUnitOfWork.Object);
            sut.WithCallTo(x => x.Delete_Post(inexistentTeacherID)).ShouldGiveHttpStatus(HttpStatusCode.NotFound);
        }

        [Test, Property("InitializeUOF", 1)]
        public override void DeletePost_ShouldRedirectToDeleteGetForException()
        {
            var sut = new TeacherController(_fakeUnitOfWork.Object);
            _fakeUnitOfWork.SetupGet(u => u.TeacherRepo).Throws(new Exception());
            // also works: _fakeUnitOfWork.SetupGet(u => u.TeacherRepo).Returns((Repository<Teacher>)null);
            sut.WithCallTo(x => x.Delete_Post(inexistentTeacherID)).ShouldRedirectTo<int?, bool?>(x => x.Delete);
            InitializeUOF();
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public override void DeletePost_ShouldRedirectToIndex_Success(int TeacherID)
        {
            configureRepository(TeacherID);
            var sut = new TeacherController(_fakeUnitOfWork.Object);
            _fakeRepo.Setup(r => r.Delete(It.IsAny<Teacher>())).Callback((Teacher Teacher) => { });
            _fakeUnitOfWork.Setup(u => u.Commit()).Callback(() => { });
            sut.WithCallTo(x => x.Delete_Post(TeacherID)).ShouldRedirectTo(x => x.Index);
            InitializeRepository();
            InitializeUOF();
        }
        #endregion
    }
}
