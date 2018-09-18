﻿using ASP;
using EFApproaches.DAL.Entities;
using EFCodeFirstTest.Helpers;
using HtmlAgilityPack;
using NUnit.Framework;
using RazorGenerator.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCodeFirstTest.ViewTests.CourseViewTest
{
    [TestFixture]
    public class CourseViewUnitTest
    {
        private _Views_Course_Index_cshtml courseIndexView = null;
        private _Views_Course_Details_cshtml courseDetailsView = null;
        public _Views_Course_Index_cshtml CourseIndexView
        {
            get
            {
                if (courseIndexView == null)
                {
                    courseIndexView = new _Views_Course_Index_cshtml();
                }
                return courseIndexView;
            }
        }

        public _Views_Course_Details_cshtml CourseDetailsView
        {
            get
            {
                if (courseDetailsView == null)
                {
                    courseDetailsView = new _Views_Course_Details_cshtml();
                }
                return courseDetailsView;
            }
        }

        [Test]
        public void ShouldRenderCoursesList()
        {
            List<Course> indexModel = DataHelper.GenerateCoursesList();
            HtmlDocument html = CourseIndexView.RenderAsHtml(indexModel);
            var titleValueEls = html.DocumentNode.Descendants("span").Where(n => n.Attributes.Contains("class") && n.Attributes["class"].Value.Contains("titleValue"));
            var creditsValueEls = html.DocumentNode.Descendants("span").Where(n => n.Attributes.Contains("class") && n.Attributes["class"].Value.Contains("creditsValue"));
            Assert.Multiple(() =>
            {
                int courseIndex = 0;
                //in this case, assert that Inner Text contains the expected value (not exactly)
                foreach (var modelItem in indexModel)
                {
                    if (modelItem.Title != null)
                    {
                        Assert.That(titleValueEls.ElementAt(courseIndex).InnerText, Contains.Substring(modelItem.Title), "the Inner Text of View Element does not contain the title value for course: " + modelItem.Title);
                    }
                    if (modelItem.Credits != null)
                    {
                        Assert.That(creditsValueEls.ElementAt(courseIndex).InnerText, Contains.Substring(modelItem.Credits.ToString()), "the Inner Text of View Element does not contain the credits value for course: " + modelItem.Title);
                    }
                    courseIndex++;
                }
            });
        }
        [Test]
        public void CourseShouldHaveMinimunAmountOfStudentsToStayOpen()
        {
            Course courseWithEnrollments = DataHelper.GenerateCourseWithTenStudents();
            var sut = CourseDetailsView;
            sut.ViewBag.MinimumStudentsToStayOpen = courseWithEnrollments.Enrollments.Count;
            HtmlDocument html = sut.RenderAsHtml(courseWithEnrollments);
            var isMinimumStudentsMessageRendered = html.GetElementbyId("MinimumStudentsMessageContainer"); 
            Assert.That(isMinimumStudentsMessageRendered, Is.Not.Null);
        }

        [Test]
        public void CourseShouldNotComplyMinimunToStayOpen()
        {
            Course courseWithEnrollments = DataHelper.GenerateCourseWithTenStudents();
            var sut = CourseDetailsView;
            sut.ViewBag.MinimumStudentsToStayOpen = courseWithEnrollments.Enrollments.Count + 1;
            HtmlDocument html = sut.RenderAsHtml(courseWithEnrollments);
            var notMinimumStudentsMessageRendered = html.GetElementbyId("NotMinimumStudentsMessageContainer");
            Assert.That(notMinimumStudentsMessageRendered, Is.Not.Null);
        }

        [Test]
        [Ignore("test code coverage")]
        public void ThereShouldNotStudentsEnrolledIncourse()
        {
            Course courseWithoutEnrollments = new Course{ CourseID = 1, Title = "Computers Architechture II", Credits = 8 };
            courseWithoutEnrollments.Enrollments = new List<Enrollment>();
            var sut = CourseDetailsView;
            HtmlDocument html = sut.RenderAsHtml(courseWithoutEnrollments);
            var noStudentsEnrolledMessageEl = html.DocumentNode.InnerHtml.Contains(": There are no students enrolled in this course");
            Assert.That(noStudentsEnrolledMessageEl, Is.True);
        }
    }
}