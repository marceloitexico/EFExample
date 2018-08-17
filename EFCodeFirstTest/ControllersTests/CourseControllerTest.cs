using EFApproaches.DAL.Entities;
using EFApproaches.DAL.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCodeFirstTest.ControllersTests
{
    [TestFixture]
    public class CourseControllerTest
    {
        #region private members
        private Mock<IContext> _fakeContext = null;
        private Mock<IRepository<Course>> _fakeRepo = null;
        private Mock<IUnitOfWork> _fakeUnitOfWork = null;
        private Mock<DbSet<Course>> _fakeDbSet = null;
        private static byte inexistentCourseID = 0;
        #endregion


    }
}
