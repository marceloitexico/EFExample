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
                /*if (null != newCourseIDElement)
                {
                    //--> fill teacher index info
                    detailsCourseLink = newCourseIDElement.FindElements(By.XPath("ancestor::tr//descendant::a[@class='detailsCourse']")).FirstOrDefault();

                    //Assert for details of created teacher
                    SeeCoursesDetails(detailsCourseLink, createdCourse);

                    //Edit teacher
                    editCourseLink = getIndexLinkElement(newCourseInIndexXPath, "editCourse");
                    var editedCourse = EditCourse(editCourseLink, randomFirstMidName);
                    //update link with fullName 
                    newCourseInIndexXPath = "//span[text()='" + editedCourse.FullName + "']";
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
                }*/
            });
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
            if (true == isCreating)
            {
                captureDataIntoControl("Title", newCourseData.Title);
            }
            captureDataIntoControl("Credits", newCourseData.Credits.ToString());
        }

        private void captureDataIntoControl(string elementID, string data)
        {
            var element = BrowserHost.Driver.FindElement(By.Id(elementID));
            element.Clear();
            element.SendKeys(data);
        }


    }
}
