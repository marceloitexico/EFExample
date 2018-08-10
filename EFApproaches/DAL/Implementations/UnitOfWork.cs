using EFApproaches.DAL.Entities;
using EFApproaches.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFApproaches.DAL.Implementations
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        //Private members corresponding to each concrete repository
        private Repository<Student> studentRepo;
        private Repository<Enrollment> enrollmentRepo;
        private Repository<Course> courseRepo;
        private bool disposed = false;

        public UnitOfWork()
        {
            dbContext = new SchoolContext();
        }

        public UnitOfWork(SchoolContext ctx)
        {
            dbContext = ctx;
        }
        //Accessor for private student repository, creates repository if null
        public IRepository<Student> StudentRepo
        {
            get
            {
                if (studentRepo == null)
                {
                    studentRepo = new Repository<Student>(dbContext);
                }
                return studentRepo;
            }
        }

        //Accessor for private enrollment repository, creates repository if null
        public IRepository<Enrollment> EnrollmentRepo
        {
            get
            {
                if (enrollmentRepo == null)
                {
                    enrollmentRepo = new Repository<Enrollment>(dbContext);
                }
                return enrollmentRepo;
            }
        }

        //Accessor for private course repository, creates repository if null
        public IRepository<Course> CourseRepo
        {
            get
            {
                if (courseRepo == null)
                {
                    courseRepo = new Repository<Course>(dbContext);
                }
                return courseRepo;
            }
        }

        //Method to save all changes to repositories (Save the whole transaction)
        public void Commit()
        {
            dbContext.SaveChanges();
        }

        protected void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}