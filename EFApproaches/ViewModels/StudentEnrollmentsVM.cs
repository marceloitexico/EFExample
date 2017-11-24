using EFApproaches.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFApproaches.ViewModels
{
    public class StudentEnrollmentsVM
    {
        public Student StudentVM { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }
    }
}