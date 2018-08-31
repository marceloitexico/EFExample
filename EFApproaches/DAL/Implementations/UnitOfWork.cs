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
        public  Repository<Student> studentRepo;
        private Repository<Enrollment> enrollmentRepo;
        private Repository<Course> courseRepo;
        private Repository<Teacher> teacherRepo;
        private bool disposed = false;
        public SchoolContext DbContext { get; set; }
        public UnitOfWork()
        {
            DbContext = new SchoolContext();
        }

        public UnitOfWork(SchoolContext ctx)
        {
            DbContext = ctx;
        }
        //Accessor for private student repository, creates repository if null
        public IRepository<Student> StudentRepo
        {
            get
            {
                if (studentRepo == null)
                {
                    studentRepo = new Repository<Student>(DbContext);
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
                    enrollmentRepo = new Repository<Enrollment>(DbContext);
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
                    courseRepo = new Repository<Course>(DbContext);
                }
                return courseRepo;
            }
        }

        //Accessor for private teacher repository, creates repository if null
        public IRepository<Teacher> TeacherRepo
        {
            get
            {
                if (teacherRepo == null)
                {
                    teacherRepo = new Repository<Teacher>(DbContext);
                }
                return teacherRepo;
            }
        }

        //Method to save all changes to repositories (Save the whole transaction)
        public void Commit()
        {
            DbContext.SaveChanges();
        }

        public void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    DbContext.Dispose();
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