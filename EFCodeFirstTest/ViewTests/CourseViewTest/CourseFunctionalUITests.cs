using BankingSite.FunctionalUITests;
using BankingSite.FunctionalUITests.DemoHelperCode;
using EFApproaches.DAL.Entities;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCodeFirstTest.ViewTests.CourseViewTest
{
    [TestFixture]
    public class CourseFunctionalUITests : FunctionalUITestsBase
    {
        private OpenQA.Selenium.IWebElement newCourseIDElement = null;
        private OpenQA.Selenium.IWebElement deleteCourseLink = null;
        private OpenQA.Selenium.IWebElement editCourseLink = null;
        private OpenQA.Selenium.IWebElement detailsCourseLink = null;

        /// <summary>
        ///This method creates a new teacher in the database (affects persistent data)
        /// </summary>
        [Test]
        public void ShouldCreateThenDeleteANewCourse()
        {
            //Create Course
            Course createdCourse = CreateCourse();
            string randomTitle = createdCourse.Title;
            var newCourseInIndexXPath = "//span[text()='" + createdCourse.Title + "']";
            Utilities.Wait(standardTimeBetweenPagesMS);
            //should exist in Courses list
            //-->DELETE var indexViewCreatedCourse = getNewCourseFromIndexView(newCourseInIndexXPath);
            newCourseIDElement = BrowserHost.Driver.FindElements(By.XPath(newCourseInIndexXPath)).FirstOrDefault();

            Assert.Multiple(() =>
            {
                Assert.That(newCourseIDElement, Is.Not.Null);
                if (null != newCourseIDElement)
                {
                    //--> get course details link view element
                    detailsCourseLink = newCourseIDElement.FindElements(By.XPath("ancestor::tr//descendant::a[@class='detailsCourse']")).FirstOrDefault();

                    //Assert for details of created teacher
                    SeeCoursesDetails(detailsCourseLink, createdCourse);
                    
                    //Edit teacher
                    editCourseLink = getIndexLinkElement(newCourseInIndexXPath, "editCourse");
                    var editedCourse = EditCourse(editCourseLink);
                    //update link with title 
                    newCourseInIndexXPath = "//span[text()='" + editedCourse.Title + "']";
                    //get link for details
                    detailsCourseLink = getIndexLinkElement(newCourseInIndexXPath, "detailsCourse");
                    //Assert for details of edited teacher
                    SeeCoursesDetails(detailsCourseLink, editedCourse);
                    
                    //Delete created teacher
                    deleteCourseLink = getIndexLinkElement(newCourseInIndexXPath, "deleteCourse");
                    DeleteCourse(deleteCourseLink);

                    //new teacher should not exist in teachers list
                    newCourseIDElement = BrowserHost.Driver.FindElements(By.XPath(newCourseInIndexXPath)).FirstOrDefault();
                    Assert.That(newCourseIDElement, Is.Null);
                    
                }
            });
        }

        private void DeleteCourse(IWebElement deleteCourseLink)
        {
            deleteCourseLink.Click();
            Utilities.Wait(standardTimeBetweenPagesMS);
            var confirmDeleteBtn = BrowserHost.Driver.FindElement(By.Id("confirmDeleteBtn"));
            confirmDeleteBtn.Click();
            Utilities.Wait(standardTimeBetweenPagesMS);
        }

        private Course EditCourse(IWebElement editCourseLink)
        {
            editCourseLink.Click();
            Utilities.Wait(standardTimeBetweenPagesMS);
            Course editCourseData = new Course
            {
                Title =  "EditedCourse" + Utilities.GenerateRandomNumber().ToString()  + "_Gecko",
                Credits = 44,
                CourseID = 2222
            };
            captureDataIntoCourseForm(editCourseData, true);
            var saveBtn = BrowserHost.Driver.FindElement(By.Id("saveBtn"));
            saveBtn.Click();
            Utilities.Wait(standardTimeBetweenPagesMS);
            return editCourseData;
        }

        private void SeeCoursesDetails(IWebElement detailsCourseLink, Course createdCourse)
        {
            detailsCourseLink.Click();
            Utilities.Wait(standardTimeBetweenPagesMS);
            var element = BrowserHost.Driver.FindElement(By.Id("titleSpan"));
            Assert.That(createdCourse.Title, Is.EqualTo(element.Text));
            element = BrowserHost.Driver.FindElement(By.Id("creditsSpan"));
            Assert.That(createdCourse.Credits.ToString(), Is.EqualTo(element.Text));
            var backToListLink = BrowserHost.Driver.FindElement(By.Id("backToListLink"));
            backToListLink.Click();
            Utilities.Wait(standardTimeBetweenPagesMS);
        }

        private Course CreateCourse()
        {
            int randomID = Utilities.GenerateRandomNumber();
            string randomTitle = "GeckoCourse" + randomID.ToString();
            BrowserHost.Driver.Navigate().GoToUrl(BrowserHost.RootUrl + @"Course\Create");
            Course newCourseData = new Course
            {
                CourseID = 1111,
                Title = randomTitle,
                Credits = 22
            };
            captureDataIntoCourseForm(newCourseData, true);
            Utilities.Wait(standardTimeToSeeData);
            var createButton = BrowserHost.Driver.FindElement(By.Id("createBtn"));
            createButton.Click();
            return newCourseData;
        }

        private void captureDataIntoCourseForm(Course newCourseData, bool isCreating )
        {
            captureDataIntoControl("Title", newCourseData.Title);
            captureDataIntoControl("Credits", newCourseData.Credits.ToString());
        }
    }
}
