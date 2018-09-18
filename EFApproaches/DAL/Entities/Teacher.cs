using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFApproaches.DAL.Entities
{
    public class Teacher
    {
        public int ID { get; set; }
        [Required]
        [DisplayName("First/Mid Name")]
        public string FirstMidName { get; set; }
        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("Title")]
        public string Title { get; set; }
        [DisplayName("Email Address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string EmailAddress { get; set; }
        [DisplayName("Full Name")]
        public string FullName { get { return FirstMidName + " " + LastName; } }
        [DisplayName("Hours Per Week")]
        public int HoursPerWeek { get; set; }
        public virtual ICollection<TeacherCourse> Courses { get; set; }
        public Teacher() { }
        public Teacher(string firstMidName, string lastName)
        {
            FirstMidName = firstMidName;
            LastName = lastName;
        }
    }
}