using EFApproaches.DAL.Entities;
using EFApproaches.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace EFApproaches.DAL.Implementations
{
    public class SchoolContext : DbContext, IContext
    {
        /// <summary>
        /// : base(Connection string's name)
        /// </summary>
        public SchoolContext() : base("SchoolContext"){ }
        public DbSet <Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<TeacherCourse> TeacherCourses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //If you didn't do this, the generated tables in the database would be named Students, Courses, and Enrollments
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}