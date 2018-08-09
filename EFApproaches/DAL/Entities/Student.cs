using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFApproaches.DAL.Entities
{
    public class Student
    {
        public Student(){}
        public Student(string firstMidName, string lastName)
        {
            FirstMidName = firstMidName;
            LastName = lastName;
            FullName = firstMidName + " " + lastName;
        }
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string EmailAddress { get; set; }
        public string FullName { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }

        public void GenerateEmailFromName(string domain)
        {
            this.EmailAddress = this.LastName + "@" + domain;
        }

        public string getFullName()
        {
            return FirstMidName + " " + LastName;
        }
    }
}