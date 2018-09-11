using BankingSite.FunctionalUITests;
using BankingSite.FunctionalUITests.DemoHelperCode;
using EFApproaches.DAL.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.Seleno.PageObjects.Locators;

namespace EFCodeFirstTest.ViewTests.TeacherViewTest
{
    [TestFixture]
    public class TeacherFunctionaUITests : FunctionalUITestsBase
    {
        private OpenQA.Selenium.IWebElement newTeacherIDElement = null;
        private OpenQA.Selenium.IWebElement deleteTeacherLink = null;
        private OpenQA.Selenium.IWebElement editTeacherLink = null;
        private OpenQA.Selenium.IWebElement detailsTeacherLink = null;
        /// <summary>
        ///This method creates a new teacher in the database (affects persistent data)
        /// </summary>
        [Test]
        public void ShouldCreateThenDeleteANewTeacher()
        {
            //Create Teacher
            Teacher createdTeacher = CreateTeacher();
            string randomFirstMidName = createdTeacher.FirstMidName;
            var newTeacherInIndexXPath = "//span[text()='" + createdTeacher.FullName + "']";
            Utilities.Wait(standardTimeBetweenPagesMS);
            //should exist in Teachers list
            //-->DELETE var indexViewCreatedTeacher = getNewTeacherFromIndexView(newTeacherInIndexXPath);
            newTeacherIDElement = BrowserHost.Driver.FindElements(By.XPath(newTeacherInIndexXPath)).FirstOrDefault();

            Assert.Multiple(() =>
            {
               Assert.That(newTeacherIDElement, Is.Not.Null);
               if (null != newTeacherIDElement)
               {
                    //--> fill teacher index info
                    detailsTeacherLink = newTeacherIDElement.FindElements(By.XPath("ancestor::tr//descendant::a[@class='detailsTeacher']")).FirstOrDefault();
                    
                    //Assert for details of created teacher
                    SeeTeachersDetails(detailsTeacherLink, createdTeacher);
                    
                    //Edit teacher
                    editTeacherLink = getIndexLinkElement(newTeacherInIndexXPath, "editTeacher");
                    var editedTeacher = EditTeacher(editTeacherLink, randomFirstMidName);
                    //update link with fullName 
                    newTeacherInIndexXPath = "//span[text()='" + editedTeacher.FullName + "']";
                    //get link for details
                    detailsTeacherLink = getIndexLinkElement(newTeacherInIndexXPath, "detailsTeacher");
                    //Assert for details of edited teacher
                    SeeTeachersDetails(detailsTeacherLink, editedTeacher);

                    //Delete created teacher
                    deleteTeacherLink = getIndexLinkElement(newTeacherInIndexXPath, "deleteTeacher");
                    DeleteTeacher(deleteTeacherLink);

                    //new teacher should not exist in teachers list
                    newTeacherIDElement = BrowserHost.Driver.FindElements(By.XPath(newTeacherInIndexXPath)).FirstOrDefault();
                    Assert.That(newTeacherIDElement, Is.Null);
               }
           });
        }

        private OpenQA.Selenium.IWebElement getIndexLinkElement(string newTeacherInIndexXPath, string elementClassName)
        {
            OpenQA.Selenium.IWebElement newTeacherIDElement = BrowserHost.Driver.FindElements(By.XPath(newTeacherInIndexXPath)).FirstOrDefault();
            var webElement =  newTeacherIDElement.FindElements(By.XPath("ancestor::tr//descendant::a[@class='" + elementClassName + "']")).FirstOrDefault();
            return webElement;
        }

        public Teacher CreateTeacher()
        {
            int randomID = Utilities.GenerateRandomNumber();
            string randomFirstMidName = "Gecko" + randomID.ToString();
            BrowserHost.Driver.Navigate().GoToUrl(BrowserHost.RootUrl + @"Teacher\Create");
            Teacher newTeacherData = new Teacher
            {
                FirstMidName = randomFirstMidName,
                LastName = "Gecko_LastName",
                Title = "Gecko_Title",
                EmailAddress = "geckoEmailAddress@Gecko.com",
                HoursPerWeek = 25
            };
            captureDataIntoTeacherForm(newTeacherData,true);
            Utilities.Wait(standardTimeToSeeData);
            var createButton = BrowserHost.Driver.FindElement(By.Id("createBtn"));
            createButton.Click();
            return newTeacherData;
        }

        public void captureDataIntoTeacherForm(Teacher teacherData, bool isCreating = false)
        {
            if (true == isCreating)
            {
                captureDataIntoControl("FirstMidName", teacherData.FirstMidName);
            }
            captureDataIntoControl("LastName", teacherData.LastName);
            captureDataIntoControl("EmailAddress", teacherData.EmailAddress);
            captureDataIntoControl("Title", teacherData.Title);
            captureDataIntoControl("HoursPerWeek", teacherData.HoursPerWeek.ToString());
        }

        private void captureDataIntoControl(string elementID, string data)
        {
            var element = BrowserHost.Driver.FindElement(By.Id(elementID));
            element.Clear();
            element.SendKeys(data);
        }

        public void DeleteTeacher(OpenQA.Selenium.IWebElement deleteTeacherLink)
        {
            deleteTeacherLink.Click();
            Utilities.Wait(standardTimeBetweenPagesMS);
            //delete teacher
            var confirmDeleteBtn = BrowserHost.Driver.FindElement(By.Id("confirmDeleteBtn"));
            confirmDeleteBtn.Click();
            Utilities.Wait(standardTimeBetweenPagesMS);
        }

        private void SeeTeachersDetails(OpenQA.Selenium.IWebElement detailsTeacherLink, Teacher newTeacherData)
        {
            detailsTeacherLink.Click();
            Utilities.Wait(standardTimeBetweenPagesMS);

            var element = BrowserHost.Driver.FindElement(By.Id("fullNameSpan"));
            Assert.That(newTeacherData.FullName, Is.EqualTo(element.Text));

            element = BrowserHost.Driver.FindElement(By.Id("titleSpan"));
            Assert.That(newTeacherData.Title, Is.EqualTo(element.Text));

            element = BrowserHost.Driver.FindElement(By.Id("emailAddressSpan"));
            Assert.That(newTeacherData.EmailAddress, Is.EqualTo(element.Text));

            element = BrowserHost.Driver.FindElement(By.Id("hoursPerWeekSpan"));
            Assert.That(newTeacherData.HoursPerWeek.ToString(), Is.EqualTo(element.Text));

            var backToListLink = BrowserHost.Driver.FindElement(By.Id("backToListLink"));
            backToListLink.Click();
            Utilities.Wait(standardTimeBetweenPagesMS);
        }

        public Teacher EditTeacher(OpenQA.Selenium.IWebElement editTeacherLink, string randomFirstMidName)
        {
            editTeacherLink.Click();
            Utilities.Wait(standardTimeBetweenPagesMS);
            Teacher editTeacherData = new Teacher
            {
                FirstMidName = randomFirstMidName,
                LastName = "Gecko_EditedLastName",
                EmailAddress = "editedReplaceableAddress@Gecko.com",
                Title = "Computer Science Engineer",
                HoursPerWeek = 22
            };
            captureDataIntoTeacherForm(editTeacherData);
            var saveBtn = BrowserHost.Driver.FindElement(By.Id("saveBtn"));
            saveBtn.Click();
            Utilities.Wait(standardTimeBetweenPagesMS);
            return editTeacherData;
        }
    }
}
