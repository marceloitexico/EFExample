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
    public class CourseControllerTest : BaseControllerTest
    {
        #region private members
        private Mock<IRepository<Course>> _fakeRepo = null;
        private Mock<DbSet<Course>> _fakeDbSet = null;
        private static byte inexistentCourseID = 0;
        #endregion
        #region index

        #endregion
        #region Setup And TearDown
        [OneTimeSetUp]
        public override void InitializeOncePerRun()
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

        public override Mock<IContext> generateFakeContextWithData()
        {
            var context = new Mock<IContext>();
            //context.Setup(sc => sc.Set<Student>()).Returns(set.Object); 
            context.SetupGet(c => c.Courses).Returns(_fakeDbSet.Object);
            return context;
        }
        #endregion
    }
}
