using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
namespace EFApproaches.DAL.Entities
{
    
    public class Course
    {
        public int CourseID { get; set; }
        [Required]
        public string Title { get; set; }
        public int Credits { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }


        public string titleAndCredits()
        {
            if (string.IsNullOrEmpty(Title) == true)
            {
                return "";
            }
            return Title + ", credits:  " + Credits.ToString();
        }
    }
}