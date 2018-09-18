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
            new Student{FirstMidName="Arturo",LastName="Anand",EnrollmentDate=DateTime.Parse("2003-09-01"),EmailAddress="aanand@school.com"},
            new Student{FirstMidName="Gytis",LastName="Barzdukas",EnrollmentDate=DateTime.Parse("2002-09-01"),EmailAddress="gbarzdukas@school.com"},
            new Student{FirstMidName="Yan",LastName="Li",EnrollmentDate=DateTime.Parse("2002-09-01"),EmailAddress="yli@school.com"},
            new Student{FirstMidName="Peggy",LastName="Justice",EnrollmentDate=DateTime.Parse("2001-09-01"),EmailAddress="pjustice@school.com"},
            new Student{FirstMidName="Laura",LastName="Norman",EnrollmentDate=DateTime.Parse("2003-09-01"),EmailAddress="lnorman@school.com"},
            new Student{FirstMidName="Shanelle",LastName="Wolak",EnrollmentDate=DateTime.Parse("2005-09-01"),EmailAddress="swolak@school.com"},
            new Student{FirstMidName="Crystle",LastName="Bale",EnrollmentDate=DateTime.Parse("2005-08-01"),EmailAddress="cbale@school.com"},
            new Student{FirstMidName="Ronnie",LastName="Furey",EnrollmentDate=DateTime.Parse("2005-07-01"),EmailAddress="rfurey@school.com"}
            };

            students.ForEach(s => context.Students.Add(s));
            context.SaveChanges();
            var courses = new List<Course>
            {
            new Course{Title="Chemistry",Credits=3,},
            new Course{Title="Microeconomics",Credits=3,},
            new Course{Title="Macroeconomics",Credits=3,},
            new Course{Title="Calculus",Credits=4,},
            new Course{Title="Trigonometry",Credits=4,},
            new Course{Title="Composition",Credits=3,},
            new Course{Title="Literature",Credits=4,}
            };
            courses.ForEach(s => context.Courses.Add(s));
            context.SaveChanges();

            context.Enrollments.Add(new Enrollment{ StudentID = 1,CourseID = 1,Grade = Grade.A});
            context.Enrollments.Add(new Enrollment { StudentID = 1, CourseID = 6, Grade = Grade.A });
            context.Enrollments.Add(new Enrollment{StudentID=1,CourseID=2,Grade=Grade.C});
            context.Enrollments.Add(new Enrollment{StudentID=1,CourseID=3,Grade=Grade.B});
            context.Enrollments.Add(new Enrollment{StudentID=2,CourseID=4,Grade=Grade.B});
            context.Enrollments.Add(new Enrollment{StudentID=2,CourseID=6,Grade=Grade.F});
            context.Enrollments.Add(new Enrollment { StudentID = 3, CourseID = 1, Grade = Grade.C });
            context.Enrollments.Add(new Enrollment{StudentID=4,CourseID=1, Grade = Grade.B });
            context.Enrollments.Add(new Enrollment { StudentID = 4, CourseID = 2, Grade = Grade.F });
            context.Enrollments.Add(new Enrollment{StudentID=5,CourseID=3,Grade=Grade.C});
            context.Enrollments.Add(new Enrollment { StudentID = 6, CourseID = 4, Grade = Grade.C });

            context.Enrollments.Add(new Enrollment { StudentID = 1, CourseID = 5, Grade = Grade.B });
            context.Enrollments.Add(new Enrollment { StudentID = 2, CourseID = 5, Grade = Grade.F });
            context.Enrollments.Add(new Enrollment { StudentID = 3, CourseID = 5, Grade = Grade.B });
            context.Enrollments.Add(new Enrollment { StudentID = 4, CourseID = 5, Grade = Grade.F });
            context.Enrollments.Add(new Enrollment { StudentID = 5, CourseID = 5, Grade = Grade.A });
            context.Enrollments.Add(new Enrollment { StudentID = 6, CourseID = 5, Grade = Grade.F });
            context.Enrollments.Add(new Enrollment { StudentID = 7, CourseID = 5, Grade = Grade.A });
            context.Enrollments.Add(new Enrollment { StudentID = 8, CourseID = 5, Grade = Grade.B });
            context.Enrollments.Add(new Enrollment { StudentID = 9, CourseID = 5, Grade = Grade.F });
            context.Enrollments.Add(new Enrollment { StudentID = 10, CourseID = 5, Grade = Grade.F });

            //enrollments.ForEach(s => context.Enrollments.Add(s));
            context.SaveChanges();

            var teachers = new List<Teacher>
            {
                new Teacher {FirstMidName="Peterson",LastName="Mary",EmailAddress="mpeterson@school.com", Title= "Doctor of Laws", HoursPerWeek =  20},
                new Teacher {FirstMidName="Manning",LastName="Brian",EmailAddress="bmanning@school.com", Title= "Doctor of Pharmacy", HoursPerWeek =  10},
                new Teacher {FirstMidName="Carr",LastName="Katherine",EmailAddress="ckarr@school.com", Title= "Licentiate of theology", HoursPerWeek =  25},
                new Teacher {FirstMidName="Springer",LastName="Wanda",EmailAddress="wspringer@school.com", Title= "Doctor of Philosophy", HoursPerWeek =  15},
                new Teacher {FirstMidName="Black",LastName="Kelly",EmailAddress="kblack@school.com", Title= "Doctor of Natural Sciences", HoursPerWeek =  30},
                new Teacher {FirstMidName="Johnson",LastName="Arianna",EmailAddress="ajohnson@school.com", Title= "Doctor of Medicine", HoursPerWeek =  19},
                new Teacher {FirstMidName="Kramer",LastName="John",EmailAddress="kblack@school.com", Title= "Veterinary Doctor", HoursPerWeek =  28}
            };
            teachers.ForEach(t => context.Teachers.Add(t));
            context.SaveChanges();

            var teacherCourses = new List<TeacherCourse>
            {
                new TeacherCourse {TeacherID = 1, CourseID = 5 },
                new TeacherCourse {TeacherID = 1, CourseID = 4 },
                new TeacherCourse {TeacherID = 2, CourseID = 3 },
                new TeacherCourse {TeacherID = 2, CourseID = 2 },
                new TeacherCourse {TeacherID = 2, CourseID = 1 },
                new TeacherCourse {TeacherID = 3, CourseID = 6 },
                new TeacherCourse {TeacherID = 3, CourseID = 4 },
                new TeacherCourse {TeacherID = 3, CourseID = 5 },
                new TeacherCourse {TeacherID = 3, CourseID = 1 },
                new TeacherCourse {TeacherID = 4, CourseID = 3 },
                new TeacherCourse {TeacherID = 4, CourseID = 2 },
                new TeacherCourse {TeacherID = 4, CourseID = 1 },
                new TeacherCourse {TeacherID = 4, CourseID = 6 },
                new TeacherCourse {TeacherID = 4, CourseID = 4 },
                new TeacherCourse {TeacherID = 5, CourseID = 3 },
                new TeacherCourse {TeacherID = 5, CourseID = 2 },
                new TeacherCourse {TeacherID = 5, CourseID = 1 },
                new TeacherCourse {TeacherID = 5, CourseID = 6 },
                new TeacherCourse {TeacherID = 6, CourseID = 4 },
                new TeacherCourse {TeacherID = 6, CourseID = 3 },
                new TeacherCourse {TeacherID = 6, CourseID = 2 },
                new TeacherCourse {TeacherID = 6, CourseID = 1 },
                new TeacherCourse {TeacherID = 6, CourseID = 6 },
                new TeacherCourse {TeacherID = 7, CourseID = 1 },
                new TeacherCourse {TeacherID = 7, CourseID = 2 },
                new TeacherCourse {TeacherID = 7, CourseID = 3 },
                new TeacherCourse {TeacherID = 7, CourseID = 4 },
                new TeacherCourse {TeacherID = 7, CourseID = 5 },
                new TeacherCourse {TeacherID = 7, CourseID = 6 },
                new TeacherCourse {TeacherID = 7, CourseID = 7 },
            };
            teacherCourses.ForEach(tc => context.TeacherCourses.Add(tc));
            context.SaveChanges();
        }
    }
}