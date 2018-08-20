using EFApproaches.Controllers;
using EFApproaches.DAL.Entities;
using EFApproaches.DAL.Interfaces;
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
    [TestFixture]
    public class CourseControllerTest : BaseControllerTest
    {
        #region private members
        private Mock<IRepository<Course>> _fakeRepo = null;
        private Mock<DbSet<Course>> _fakeDbSet = null;
        private static byte inexistentCourseID = 0;
        #endregion
        #region Setup And TearDown
        [OneTimeSetUp]
        protected override void InitializeOncePerRun()
        {
            var data = generateCoursesList();
            _fakeDbSet = new Mock<DbSet<Course>>().SetupData(data);
            //Console.WriteLine("Initial message");
            _fakeContext = generateFakeContextWithData();
            //Create a new Mock of repository
            _fakeRepo = new Mock<IRepository<Course>>();
            _fakeRepo.SetupGet<IEnumerable<Course>>(r => r.DataSet).Returns(_fakeContext.Object.Courses);
            //create fake Unit of Work
            _fakeUnitOfWork = new Mock<IUnitOfWork>();
            //configure unit of work to 
            _fakeUnitOfWork.SetupGet<IRepository<Course>>(u => u.CourseRepo).Returns(_fakeRepo.Object);
        }

        protected override Mock<IContext> generateFakeContextWithData()
        {
            var context = new Mock<IContext>();
            context.SetupGet(c => c.Courses).Returns(_fakeDbSet.Object);
            return context;
        }

        protected override void InitializeContext()
        {
            _fakeContext.Reset();
            _fakeContext.SetupGet(c => c.Courses).Returns(_fakeDbSet.Object);
        }

        protected override void InitializeRepository()
        {
            _fakeRepo.Reset();
            _fakeRepo.SetupGet<IEnumerable<Course>>(r => r.DataSet).Returns(_fakeContext.Object.Courses);
        }

        protected override void InitializeUOF()
        {
            _fakeUnitOfWork.Reset();
            _fakeUnitOfWork.SetupGet<IRepository<Course>>(u => u.CourseRepo).Returns(_fakeRepo.Object);
        }
        #endregion
        #region privateHelpMethods
        private List<Course> generateCoursesList()
        {
            var data = new List<Course>
            {
                new Course {CourseID = 1, Title = "Computers Architechture II", Credits = 8 },
                new Course {CourseID = 2, Title = "Artificial Intelligence", Credits = 7 },
                new Course {CourseID = 3, Title = "Compilers Theory", Credits = 10 },
                new Course {CourseID = 4, Title = "Operating Systems", Credits = 12 },
                new Course {CourseID = 5, Title = "Graphs Theory II", Credits = 12 },
            };
            return data;
        }

        [OneTimeTearDown]
        protected void CleanupOncePerRun()
        {
            _fakeContext.Reset();
            _fakeRepo.Reset();
            _fakeUnitOfWork.Reset();
        }
        /// <summary>
        /// Configure Fake Repository to return an course given an specific ID
        /// </summary>
        /// <param name="courseID"></param>
        protected override void configureRepository(int courseID)
        {
            try
            {
                _fakeRepo.Setup(r => r.GetById(courseID)).Returns(_fakeContext.Object.Courses.FirstOrDefault<Course>(c => c.CourseID == courseID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        #region Index
        [Test]
        public override void ShouldRenderIndexViewWithModel()
        {
            var sut = new CourseController(_fakeUnitOfWork.Object);
            var ds = _fakeUnitOfWork.Object.CourseRepo.DataSet;
            sut.WithCallTo(x => x.Index()).ShouldRenderDefaultView().WithModel<IEnumerable<Course>>(ds);
        }
        #endregion
        #region Details
        [Test]
        public override void DetailsShouldReturnBadRequestResultForNullID()
        {
            var sut = new CourseController();
            sut.WithCallTo(x => x.Details(null)).ShouldGiveHttpStatus(System.Net.HttpStatusCode.BadRequest);
        }

        [TestCase(-1)]
        [TestCase(-2)]
        public override void DetailsShouldReturnNotFoundResultForInexistentID(int courseID)
        {
            configureRepository(courseID);
            var sut = new CourseController(_fakeUnitOfWork.Object);
            sut.WithCallTo(x => x.Details(courseID)).ShouldGiveHttpStatus(System.Net.HttpStatusCode.NotFound);
            InitializeRepository();
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public override void DetailsShouldReturnObjecttForValidID(int courseID)
        {
            //set
            configureRepository(courseID);
            var sut = new CourseController(_fakeUnitOfWork.Object);
            //Action and Assert
            sut.WithCallTo(x => x.Details(courseID)).ShouldRenderDefaultView().WithModel<Course>().AndNoModelErrors();
            InitializeRepository();
        }
        #endregion
        #region Create
        [Test]
        public override void CreateShouldRenderDefaultView()
        {
            var sut = new CourseController();
            sut.WithCallTo(x => x.Create()).ShouldRenderDefaultView();
        }

        /// <summary>
        /// Create Post, first method
        /// </summary>
        [Test]
        public override void CreatePostGetInvalidModelError()
        {
            var sut = new CourseController(_fakeUnitOfWork.Object);
            //Add error to ModelState
            sut.ModelState.AddModelError("TitleRequiredError", "Model is not valid, verify required fields");
            sut.WithCallTo(x => x.Create(new Course())).ShouldRenderDefaultView().WithModel<Course>().AndModelError("TitleRequiredError");
        }

        [Test]
        public override void CreatePostGetExceptionError()
        {
            //Add error to ModelState
            _fakeUnitOfWork.SetupGet(u => u.CourseRepo).Throws(new Exception("ExceptionMsg"));
            var course = new Course { CourseID = 23 };
            var sut = new CourseController(_fakeUnitOfWork.Object);
            sut.WithCallTo(x => x.Create(course)).ShouldRenderDefaultView().WithModel<Course>().AndModelError("ExceptionError");
            InitializeUOF();
        }

        /// <summary>
        /// Validate Model
        /// No model errors
        /// return proper view
        /// </summary>
        [Test]
        public override void CreatePostShouldRedirectToIndex_Success()
        {
            var mockCourse = new Mock<Course>();
            _fakeRepo.Setup(r => r.Create(It.IsAny<Course>())).Callback((Course std) => { });
            _fakeUnitOfWork.Setup(u => u.Commit()).Callback(() => { });
            var sut = new CourseController(_fakeUnitOfWork.Object);
            sut.WithCallTo(x => x.Create(mockCourse.Object)).ShouldRedirectTo(x => x.Index);
            InitializeRepository();
            InitializeUOF();
        }
        #endregion
        #region edit
        [Test]
        public override void EditGetShouldReturnBadRequestResponse()
        {
            var sut = new CourseController();
            sut.WithCallTo(x => x.Edit(null)).ShouldGiveHttpStatus(HttpStatusCode.BadRequest);
        }

        [Test]
        public override void EditGetShouldReturnNotFoundResponse()
        {
            _fakeRepo.Setup(r => r.GetById(It.IsAny<int>())).Returns((Course)null);
            var sut = new CourseController(_fakeUnitOfWork.Object);
            sut.WithCallTo(x => x.Edit(It.IsAny<int>())).ShouldGiveHttpStatus(HttpStatusCode.NotFound);
            InitializeRepository();
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public override void EditGetShouldRenderDefaultView(int courseID)
        {
            configureRepository(courseID);
            var sut = new CourseController(_fakeUnitOfWork.Object);
            sut.WithCallTo(x => x.Edit(courseID)).ShouldRenderDefaultView().WithModel<Course>().AndNoModelErrors();
            InitializeRepository();
        }

        /// <summary>
        /// Edit, Post
        /// </summary>
        [Test]
        public override void EditPostShouldReturnBadRequestResponse()
        {
            var sut = new CourseController();
            sut.WithCallTo(x => x.EditPost(null)).ShouldGiveHttpStatus(HttpStatusCode.BadRequest);
        }

        [Test]
        public override void EditPostShouldReturnNullModelError()
        {
            var sut = new CourseController(_fakeUnitOfWork.Object);
            sut.ControllerContext = CreateControllerContextObject();
            sut.ValueProvider = new FormCollection().ToValueProvider();
            sut.WithCallTo(x => x.EditPost(0)).ShouldRenderDefaultView().WithModel<Course>().AndModelError("NullModelError");
        }

        [TestCase(1)]
        public override void EditPostShouldReturnSaveChangesExceptionError(int courseID)
        {
            configureRepository(courseID);
            var sut = new CourseController(_fakeUnitOfWork.Object);
            sut.WithCallTo(x => x.EditPost(courseID)).ShouldRenderDefaultView().WithModel<Course>().AndModelError("SaveChangesExceptionError");
            InitializeRepository();
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public override void EditPost_ShouldRedirectToIndex_Success(int courseID)
        {
            configureRepository(courseID);
            var sut = new CourseController(_fakeUnitOfWork.Object);
            sut.ControllerContext = CreateControllerContextObject();
            sut.ValueProvider = new FormCollection().ToValueProvider();
            //In this case, index does not have parameters
            sut.WithCallTo(x => x.EditPost(courseID)).ShouldRedirectTo(x => x.Index);
            InitializeRepository();
        }
        #endregion
        #region Delete
        [Test]
        public override void DeleteGet_ShouldReturnBadRequestResponse()
        {
            var sut = new CourseController();
            sut.WithCallTo(x => x.Delete(null, false)).ShouldGiveHttpStatus(HttpStatusCode.BadRequest);
        }

        [TestCase(true)]
        [TestCase(false)]
        public override void DeleteGet_ShouldReturnNotFoundResponse(bool saveChangesError)
        {
            var sut = new CourseController(_fakeUnitOfWork.Object);
            sut.WithCallTo(x => x.Delete(inexistentCourseID, saveChangesError)).ShouldGiveHttpStatus(HttpStatusCode.NotFound);
        }

        [TestCase(1, true)]
        [TestCase(1, false)]
        [TestCase(2, true)]
        [TestCase(2, false)]
        public override void DeleteGet_ShouldReturnNotFoundResponse(int courseID, bool saveChangesError)
        {
            configureRepository(courseID);
            var sut = new CourseController(_fakeUnitOfWork.Object);
            sut.WithCallTo(x => x.Delete(courseID, saveChangesError)).ShouldRenderDefaultView().WithModel<Course>().AndNoModelErrors();
            InitializeRepository();
        }

        /// <summary>
        /// Delete, POST, not in nov
        /// </summary>
        [Test]
        public override void DeletePost_ShouldReturnBadRequestResponse()
        {
            var sut = new CourseController();
            sut.WithCallTo(x => x.Delete_Post(null)).ShouldGiveHttpStatus(HttpStatusCode.BadRequest);
        }

        [Test]
        public override void DeletePost_ShouldReturnNotFoundResponse()
        {
            var sut = new CourseController(_fakeUnitOfWork.Object);
            sut.WithCallTo(x => x.Delete_Post(inexistentCourseID)).ShouldGiveHttpStatus(HttpStatusCode.NotFound);
        }

        [Test, Property("InitializeUOF", 1)]
        public override void DeletePost_ShouldRedirectToDeleteGetForException()
        {
            var sut = new CourseController(_fakeUnitOfWork.Object);
            _fakeUnitOfWork.SetupGet(u => u.CourseRepo).Throws(new Exception());
            sut.WithCallTo(x => x.Delete_Post(inexistentCourseID)).ShouldRedirectTo<int?, bool?>(x => x.Delete);
            InitializeUOF();
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public override void DeletePost_ShouldRedirectToIndex_Success(int objectID)
        {
            configureRepository(objectID);
            var sut = new CourseController(_fakeUnitOfWork.Object);
            _fakeRepo.Setup(r => r.Delete(It.IsAny<Course>())).Callback((Course course) => { });
            _fakeUnitOfWork.Setup(u => u.Commit()).Callback(() => { });
            sut.WithCallTo(x => x.Delete_Post(objectID)).ShouldRedirectTo(x => x.Index);
            InitializeRepository();
            InitializeUOF();
        }
        #endregion

    }
}
