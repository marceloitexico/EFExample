using EFApproaches.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFApproaches.DAL.Entities
{
    public class Student : IStudent
    {
        public Student(){}
        public Student(string firstMidName, string lastName)
        {
            FirstMidName = firstMidName;
            LastName = lastName;
            FullName = firstMidName + " " + lastName;
        }
        public int ID { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FirstMidName { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime EnrollmentDate { get; set; }
        public  string EmailAddress { get; set; }
        public string FullName { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }

        public virtual void GenerateEmailFromName(string domain)
        {
            this.EmailAddress = this.LastName + "@" + domain;
        }

        public string getFullName()
        {
            return FirstMidName + " " + LastName;
        }
    }
}