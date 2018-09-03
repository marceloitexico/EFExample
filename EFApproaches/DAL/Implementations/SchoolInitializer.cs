using EFApproaches.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFApproaches.DAL.Implementations
{
    /// <summary>
    /// initializer class, causes a database to be created when needed and loads test data
    /// into the new database.
    /// </summary>
    public class SchoolInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<SchoolContext>
    {
        protected override void Seed(SchoolContext context)
        {
           var students = new List<Student>
            {
            new Student{FirstMidName="Carson",LastName="Alexander",  EnrollmentDate=DateTime.Parse("2005-09-01"),EmailAddress="carson@school.com"},
            new Student{FirstMidName="Meredith",LastName="Alonso",EnrollmentDate=DateTime.Parse("2002-09-01"),EmailAddress="meredith@school.com"},
            new Student{FirstMidName="Arturo",LastName="Anand",EnrollmentDate=DateTime.Parse("2003-09-01")},
            new Student{FirstMidName="Gytis",LastName="Barzdukas",EnrollmentDate=DateTime.Parse("2002-09-01")},
            new Student{FirstMidName="Yan",LastName="Li",EnrollmentDate=DateTime.Parse("2002-09-01")},
            new Student{FirstMidName="Peggy",LastName="Justice",EnrollmentDate=DateTime.Parse("2001-09-01")},
            new Student{FirstMidName="Laura",LastName="Norman",EnrollmentDate=DateTime.Parse("2003-09-01")},
            new Student{FirstMidName="Nino",LastName="Olivetto",EnrollmentDate=DateTime.Parse("2005-09-01")}
            };

            students.ForEach(s => context.Students.Add(s));
            context.SaveChanges();
            var courses = new List<Course>
            {
            new Course{CourseID=1050,Title="Chemistry",Credits=3,},
            new Course{CourseID=4022,Title="Microeconomics",Credits=3,},
            new Course{CourseID=4041,Title="Macroeconomics",Credits=3,},
            new Course{CourseID=1045,Title="Calculus",Credits=4,},
            new Course{CourseID=3141,Title="Trigonometry",Credits=4,},
            new Course{CourseID=2021,Title="Composition",Credits=3,},
            new Course{CourseID=2042,Title="Literature",Credits=4,}
            };
            courses.ForEach(s => context.Courses.Add(s));
            context.SaveChanges();

            context.Enrollments.Add(new Enrollment{ StudentID = 1,CourseID = 1050,Grade = Grade.A});
            context.Enrollments.Add(new Enrollment { StudentID = 1, CourseID = 2021, Grade = Grade.A });
            context.Enrollments.Add(new Enrollment{StudentID=1,CourseID=4022,Grade=Grade.C});
            context.Enrollments.Add(new Enrollment{StudentID=1,CourseID=4041,Grade=Grade.B});
            context.Enrollments.Add(new Enrollment{StudentID=2,CourseID=1045,Grade=Grade.B});
            context.Enrollments.Add(new Enrollment{StudentID=2,CourseID=3141,Grade=Grade.F});
            context.Enrollments.Add(new Enrollment{StudentID=2,CourseID=2021,Grade=Grade.F});
            context.Enrollments.Add(new Enrollment { StudentID = 3, CourseID = 1050 });
            context.Enrollments.Add(new Enrollment{StudentID=4,CourseID=1050});
            context.Enrollments.Add(new Enrollment { StudentID = 4, CourseID = 4022, Grade = Grade.F });
            context.Enrollments.Add(new Enrollment{StudentID=5,CourseID=4041,Grade=Grade.C});
            context.Enrollments.Add(new Enrollment { StudentID = 6, CourseID = 1045 });
            context.Enrollments.Add(new Enrollment { StudentID = 7, CourseID = 3141, Grade = Grade.A });
            
            //enrollments.ForEach(s => context.Enrollments.Add(s));
            context.SaveChanges();

            var teachers = new List<Teacher>
            {
                new Teacher {FirstMidName="Peterson",LastName="Mary",EmailAddress="mpeterson@school.com"},
                new Teacher {FirstMidName="Manning",LastName="Brian",EmailAddress="bmanning@school.com"},
                new Teacher {FirstMidName="Carr",LastName="Katherine",EmailAddress="ckarr@school.com"},
                new Teacher {FirstMidName="Springer",LastName="Wanda",EmailAddress="wspringer@school.com"},
                new Teacher {FirstMidName="Black",LastName="Kelly",EmailAddress="kblack@school.com"}
            };
            teachers.ForEach(t => context.Teachers.Add(t));
            context.SaveChanges();

            var teacherCourses = new List<TeacherCourse>
            {
                new TeacherCourse {TeacherID = 1, CourseID = 3141 },
                new TeacherCourse {TeacherID = 1, CourseID = 1045 },
                new TeacherCourse {TeacherID = 2, CourseID = 4041 },
                new TeacherCourse {TeacherID = 2, CourseID = 4022 },
                new TeacherCourse {TeacherID = 2, CourseID = 1050 },
                new TeacherCourse {TeacherID = 3, CourseID = 2021 },
                new TeacherCourse {TeacherID = 3, CourseID = 1045 },
                new TeacherCourse {TeacherID = 3, CourseID = 3141 },
                new TeacherCourse {TeacherID = 3, CourseID = 1050 },
                new TeacherCourse {TeacherID = 4, CourseID = 4041 },
                new TeacherCourse {TeacherID = 4, CourseID = 4022 },
                new TeacherCourse {TeacherID = 4, CourseID = 1050 },
                new TeacherCourse {TeacherID = 4, CourseID = 2021 },
                new TeacherCourse {TeacherID = 4, CourseID = 1045 }
            };
            teacherCourses.ForEach(tc => context.TeacherCourses.Add(tc));
            context.SaveChanges();
        }
    }
}