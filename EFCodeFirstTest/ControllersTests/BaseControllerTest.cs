using EFApproaches.DAL.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EFCodeFirstTest.ControllersTests
{
    [TestFixture] 
    public abstract class BaseControllerTest
    {
        #region protected members
        protected internal Mock<IUnitOfWork> _fakeUnitOfWork = null;
        protected internal Mock<IContext> _fakeContext = null;
        #endregion
        #region Setup And TearDown
        protected abstract void InitializeOncePerRun();
        [SetUp]
        protected void InitializeBeforeEachTest()
        {
            TestContext.WriteLine("Method name: " + TestContext.CurrentContext.Test.MethodName);//works
            TestContext.Out.WriteLine("Method name: " + TestContext.CurrentContext.Test.MethodName);//also works
        }
        [TearDown]
        protected void CleanupAfterEachTest() {}
        protected abstract void InitializeContext();
        protected abstract void InitializeRepository();
        protected abstract void InitializeUOF();
        protected abstract Mock<IContext> generateFakeContextWithData();
        protected abstract void configureRepository(int studentID);
        #endregion
        #region protectedHelpMethods
        /// <summary>
        /// Create and Configure ControllerContext to be used in TryUpdateModel
        /// </summary>
        /// <returns></returns>
        protected ControllerContext CreateControllerContextObject()
        {
            var request = new Mock<HttpRequestBase>();
            request.Setup(r => r.HttpMethod).Returns("GET");
            var mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(c => c.Request).Returns(request.Object);
            var controllerContext = new ControllerContext(mockHttpContext.Object
            , new RouteData(), new Mock<ControllerBase>().Object);
            return controllerContext;
        }
        #endregion
        #region CRUD methods
        #region Index
        public abstract void ShouldRenderIndexViewWithModel();
        #endregion
        #region Details
        public abstract void DetailsShouldReturnBadRequestResultForNullID();
        public abstract void DetailsShouldReturnNotFoundResultForInexistentID(int objectID);
        public abstract void DetailsShouldReturnObjecttForValidID(int objectID);
        #endregion
        #region Create
        public abstract void CreateShouldRenderDefaultView();
        public abstract void CreatePostShouldRedirectToIndex_Success();
        public abstract void CreatePostGetInvalidModelError();
        public abstract void CreatePostGetExceptionError();
        #endregion
        #region Edit
        public abstract void EditGetShouldReturnBadRequestResponse();
        public abstract void EditGetShouldReturnNotFoundResponse();
        public abstract void EditGetShouldRenderDefaultView(int studentID);
        public abstract void EditPostShouldReturnBadRequestResponse();
        public abstract void EditPostShouldReturnNullModelError();
        public abstract void EditPostShouldReturnSaveChangesExceptionError(int studentID);
        public abstract void EditPost_ShouldRedirectToIndex_Success(int studentID);
        #endregion
        #region Delete
        public abstract void DeleteGet_ShouldReturnBadRequestResponse();
        public abstract void DeleteGet_ShouldReturnNotFoundResponse(bool saveChangesError);
        public abstract void DeleteGet_ShouldReturnNotFoundResponse(int objectID, bool saveChangesError);
        public abstract void DeletePost_ShouldReturnBadRequestResponse();
        public abstract void DeletePost_ShouldReturnNotFoundResponse();
        public abstract void DeletePost_ShouldRedirectToDeleteGetForException();
        public abstract void DeletePost_ShouldRedirectToIndex_Success(int studentID);
        #endregion
        #endregion
    }
}
