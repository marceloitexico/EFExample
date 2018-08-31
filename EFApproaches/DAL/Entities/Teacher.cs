using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFApproaches.DAL.Entities
{
    public class Teacher
    {
        public int ID { get; set; }
        [Required]
        public string FirstMidName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Title { get; set; }
        public string EmailAddress { get; set; }
        public string FullName { get { return FirstMidName + " " + LastName; } }
        public int Age { get; }
        public virtual ICollection<TeacherCourse> Courses { get; set; }
        public Teacher() { }
        public Teacher(string firstMidName, string lastName)
        {
            FirstMidName = firstMidName;
            LastName = lastName;
        }
    }
}