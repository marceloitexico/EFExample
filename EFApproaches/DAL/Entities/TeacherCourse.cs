using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFApproaches.DAL.Entities
{
    public class TeacherCourse
    {
        public int ID { get; set; }
        public int CourseID { get; set; }
        public int TeacherID { get; set; }
        public int AsignedHoursForCourse { get; set; }
        public virtual Course Course { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}