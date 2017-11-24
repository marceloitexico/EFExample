using EFApproaches.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace EFApproaches.DAL.Implementations
{
    public class SchoolContext : DbContext
    {
        public SchoolContext() : base("SchoolContext")//Connection string's name
        { }
        public DbSet <Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();//If you didn't do this, the generated tables in the database would be named Students, Courses, and Enrollments

        }
    }
}