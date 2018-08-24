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

namespace EFCodeFirstTest.ViewTests.StudentViewTest
{
    [TestFixture]
    public class StudentFunctionaUITests
    {
        private static int standardTimeBetweenPagesMS = 4000;
        /// <summary>
        ///This method creates a new student in the database (affects persistent data)
        /// </summary>
        [Test]
        public void ShouldCreateThenDeleteANewStudent()
        {
            OpenQA.Selenium.IWebElement newStudent = null;
            OpenQA.Selenium.IWebElement deleteStudentLink = null;
            OpenQA.Selenium.IWebElement editStudentLink = null;
            OpenQA.Selenium.IWebElement detailsStudentLink = null;
            //Create Student
            string randomFirstMidName = CreateStudent();
            var newStudentInIndexXPath = "//span[text()='" + randomFirstMidName + "']";

            Utilities.Wait(standardTimeBetweenPagesMS);
           
            //should exist in students list
             newStudent = BrowserHost.Driver.FindElements(By.XPath(newStudentInIndexXPath)).FirstOrDefault();
            Assert.Multiple(() =>
            {
               Assert.That(newStudent, Is.Not.Null);

               if (null != newStudent)
               {
                    //see details of student
                    Student detailsStudent = new Student();
                    //--> fill student index info
                   detailsStudentLink = newStudent.FindElements(By.XPath("ancestor::tr//descendant::a[@class='detailsStudent']")).FirstOrDefault();
                    SeeDetailsStudent(detailsStudentLink, detailsStudent);
                    //Edit student
                    newStudent = BrowserHost.Driver.FindElements(By.XPath(newStudentInIndexXPath)).FirstOrDefault();
                   editStudentLink = newStudent.FindElements(By.XPath("ancestor::tr//descendant::a[@class='editStudent']")).FirstOrDefault();
                   var student = EditStudent(editStudentLink, randomFirstMidName);

                    //Verify new values
                    //  -lastName
                    newStudent = BrowserHost.Driver.FindElements(By.XPath(newStudentInIndexXPath)).FirstOrDefault();
                    var editedLastName = newStudent.FindElements(By.XPath("ancestor::tr//descendant::span[@class='lastNameClass']")).FirstOrDefault();
                   Assert.That(student.LastName, Is.EqualTo(editedLastName.Text));
                   //  -enrollmentDate
                   var editedEnrollmentDate = newStudent.FindElements(By.XPath("ancestor::tr//descendant::span[@class='enrollmentDateClass']")).FirstOrDefault();
                   Assert.That(student.EnrollmentDate.ToShortDateString(), Is.EqualTo(editedEnrollmentDate.Text));

                    //Delete created student
                   deleteStudentLink = newStudent.FindElements(By.XPath("ancestor::tr//descendant::a[@class='deleteStudent']")).FirstOrDefault();
                   DeleteStudent(deleteStudentLink);

                   //new student should not exist in students list
                   newStudent = BrowserHost.Driver.FindElements(By.XPath(newStudentInIndexXPath)).FirstOrDefault();
                   Assert.That(newStudent, Is.Null);
               }
           });
        }

       

        public string CreateStudent()
        {
            int randomID = Utilities.GenerateRandomNumber();
            string randomFirstMidName = "Gecko" + randomID.ToString();
            BrowserHost.Driver.Navigate().GoToUrl(BrowserHost.RootUrl + @"Student\Create");
            Student newStudentData = new Student
            {
                FirstMidName = randomFirstMidName,
                LastName = "Gecko_LastName",
                EmailAddress = "replaceableAddress@Gecko.com",
                EnrollmentDate = new DateTime(2018,02,02)
            };
            captureDataIntoStudentForm(newStudentData,true);

            Utilities.Wait(200);
            var createButton = BrowserHost.Driver.FindElement(By.Id("createBtn"));
            createButton.Click();
            return randomFirstMidName;
        }

        public void captureDataIntoStudentForm(Student studentData, bool isCreating = false)
        {
            if (true == isCreating)
            {
                captureDataIntoControl("FirstMidName", studentData.FirstMidName);
            }
            captureDataIntoControl("LastName",studentData.LastName);
            captureDataIntoControl("EmailAddress", studentData.EmailAddress);
            captureDataIntoControl("EnrollmentDate", studentData.EnrollmentDate.ToShortDateString());
        }

        private void captureDataIntoControl(string elementID, string data)
        {
            var element = BrowserHost.Driver.FindElement(By.Id(elementID));
            element.Clear();
            element.SendKeys(data);
        }

        public void DeleteStudent(OpenQA.Selenium.IWebElement deleteStudentLink)
        {
            deleteStudentLink.Click();
            Utilities.Wait(standardTimeBetweenPagesMS);
            //delete student
            var confirmDeleteBtn = BrowserHost.Driver.FindElement(By.Id("confirmDeleteBtn"));
            confirmDeleteBtn.Click();
            Utilities.Wait(standardTimeBetweenPagesMS);
        }

        private void SeeDetailsStudent(OpenQA.Selenium.IWebElement detailsStudentLink, Student newStudentData)
        {
            detailsStudentLink.Click();
            Utilities.Wait(standardTimeBetweenPagesMS);

            var element = BrowserHost.Driver.FindElement(By.Id("firstMidNameSpan"));
            Assert.That(newStudentData.FirstMidName, Is.EqualTo(element.Text));

            element = BrowserHost.Driver.FindElement(By.Id("lastNameSpan"));
            Assert.That(newStudentData.LastName, Is.EqualTo(element.Text));

            element = BrowserHost.Driver.FindElement(By.Id("emailAddressSpan"));
            Assert.That(newStudentData.EmailAddress, Is.EqualTo(element.Text));

            element = BrowserHost.Driver.FindElement(By.Id("enrollmentDateSpan"));
            Assert.That(newStudentData.EnrollmentDate, Is.EqualTo(element.Text));
        }

        public Student EditStudent(OpenQA.Selenium.IWebElement editStudentLink, string randomFirstMidName)
        {
            editStudentLink.Click();
            Utilities.Wait(standardTimeBetweenPagesMS);
            Student editStudentData = new Student
            {
                FirstMidName = randomFirstMidName,
                LastName = "Gecko_EditedLastName",
                EmailAddress = "editedReplaceableAddress@Gecko.com",
                EnrollmentDate = new DateTime(2222, 12, 22)
            };
            captureDataIntoStudentForm(editStudentData);
            var saveBtn = BrowserHost.Driver.FindElement(By.Id("saveBtn"));
            saveBtn.Click();
            Utilities.Wait(standardTimeBetweenPagesMS);
            return editStudentData;
        }

    }
}
