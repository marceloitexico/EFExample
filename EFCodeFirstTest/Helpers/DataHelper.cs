using EFApproaches.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCodeFirstTest.Helpers
{
    public static class DataHelper
    {
        public static int StudentsAmount = 4;
        public static int TeachersAmount = 5;
        /// <summary>
        ///generate list of data in memory
        /// </summary>
        /// <returns></returns>
        public static List<Student> GenerateStudentsList()
        {
            var data = new List<Student>
            {
                new Student {ID = 1, FirstMidName = "Nathan", LastName = "Eldridge", EmailAddress  = "neldridge@domain.com",  EnrollmentDate = new DateTime(2018,2,20)},
                new Student {ID = 2, FirstMidName = "Samir", LastName = "Lakhani", EmailAddress  = "slakhani@domain.com" ,  EnrollmentDate = new DateTime(2018,3,21)},
                new Student {ID = 3, FirstMidName = "Camille", LastName = "Lozerone", EmailAddress  = "clozerone@domain.com",  EnrollmentDate = new DateTime(2018,4,22) },
                new Student {ID = 4, FirstMidName = "John", LastName = "Papa", EmailAddress  = "jpapa@domain.com",  EnrollmentDate = new DateTime(2018,5,23) },
                new Student {ID = 5, FirstMidName = "Peg", LastName = "Bevers", EmailAddress  = "pvebers@domain.com",  EnrollmentDate = new DateTime(2018,2,20)},
                new Student {ID = 6, FirstMidName = "Junko", LastName = "Mcintyre", EmailAddress  = "jmcintyre@domain.com" ,  EnrollmentDate = new DateTime(2018,3,21)},
                new Student {ID = 7, FirstMidName = "Melvin", LastName = "Janney", EmailAddress  = "mjanney@domain.com",  EnrollmentDate = new DateTime(2018,4,22) },
                new Student {ID = 8, FirstMidName = "Santiago", LastName = "Dragon", EmailAddress  = "sdragon@domain.com",  EnrollmentDate = new DateTime(2018,5,23) },
                new Student {ID = 9, FirstMidName = "Heide", LastName = "Herring", EmailAddress  = "hherring@domain.com",  EnrollmentDate = new DateTime(2018,4,22) },
                new Student {ID = 10, FirstMidName = "Marleen", LastName = "Guzzi", EmailAddress  = "mguzzi@domain.com",  EnrollmentDate = new DateTime(2018,5,23) },
            };
            return data;
        }

        public static List<Course> GenerateCoursesList()
        {
            var data = new List<Course>
            {
                new Course {CourseID = 1, Title = "Computers Architechture II", Credits = 8 },
                new Course {CourseID = 2, Title = "Artificial Intelligence", Credits = 7 },
                new Course {CourseID = 3, Title = "Compilers Theory", Credits = 10 },
                new Course {CourseID = 4, Title = "Operating Systems", Credits = 12 },
                new Course {CourseID = 5, Title = "Graphs Theory II", Credits = 12 },
            };
            return data;
        }

        public static Course GenerateCourseWithTenStudents()
        {
            Course course = new Course { CourseID = 1, Title = "Computers Architechture II", Credits = 8 };
            var studentList = GenerateStudentsList();
            var data = new List<Enrollment>();
            foreach (var std in studentList)
            {
                var newEnrollment = new Enrollment { ID = 1, CourseID = course.CourseID, Grade = Grade.B, StudentID = std.ID, Student = std };
                data.Add(newEnrollment);
            }
            course.Enrollments = data;
            return course;
        }
        public static List<Teacher> GenerateTeachersList()
        { 
            var data = new List<Teacher>
            {
                new Teacher {ID = 1, FirstMidName = "Diana", LastName = "Golson", EmailAddress  = "dgolson@domain.com", Title = "Computer Science Engineer", HoursPerWeek= 25},
                new Teacher {ID = 2, FirstMidName = "Natisha", LastName = "Gracey", EmailAddress  = "ngracey@domain.com", HoursPerWeek= 15},
                new Teacher {ID = 3, FirstMidName = "Marvella", LastName = "Shapiro", EmailAddress  = "mshapiro@domain.com", HoursPerWeek= 10},
                new Teacher {ID = 4, FirstMidName = "Damaris", LastName = "Foshee", EmailAddress  = "dfoshee@domain.com", HoursPerWeek= 30 },
                new Teacher {ID = 5, FirstMidName = "Selene", LastName = "Frazier", EmailAddress  = "sfrazier@domain.com" , HoursPerWeek= 32}
            };
            return data;
        }
    }
}
