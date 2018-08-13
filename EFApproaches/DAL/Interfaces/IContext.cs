using EFApproaches.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EFApproaches.DAL.Interfaces
{
    public interface IContext 
    {
        DbSet<Student> Students { get; set; }
        DbSet<Enrollment> Enrollments { get; set; }
        DbSet<Course> Courses { get; set; }
    }
}