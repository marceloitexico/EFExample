using EFApproaches.DAL.Entities;
using EFApproaches.DAL.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFApproaches.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        SchoolContext DbContext { get; set; }
        IRepository<Student> StudentRepo { get; }
        IRepository<Course> CourseRepo { get; }
        void Commit();
        void Dispose();
    }
}