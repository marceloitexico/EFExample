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
    public class StudentFunctionaUITests : FunctionalUITestsBase
    {
        private static int standardTimeBetweenPagesMS = 4000;
        /// <summary>
        ///This method creates a new student in the database (affects persistent data)
        /// </summary>
        [Test]
        public void ShouldCreateThenDeleteANewStudent()
        {
            //Create Student
            Student createdStudent = CreateStudent();
            string randomFirstMidName = createdStudent.FirstMidName;
            var newStudentInIndexXPath = "//span[text()='" + randomFirstMidName + "']";
            Utilities.Wait(standardTimeBetweenPagesMS);
            //should exist in students list
            //-->DELETE var indexViewCreatedStudent = getNewStudentFromIndexView(newStudentInIndexXPath);
            newStudentIDElement = BrowserHost.Driver.FindElements(By.XPath(newStudentInIndexXPath)).FirstOrDefault();

            Assert.Multiple(() =>
            {
               Assert.That(newStudentIDElement, Is.Not.Null);
               if (null != newStudentIDElement)
               {
                    //--> fill student index info
                    detailsStudentLink = newStudentIDElement.FindElements(By.XPath("ancestor::tr//descendant::a[@class='detailsStudent']")).FirstOrDefault();
                    
                    //Assert for details of created student
                    SeeStudentsDetails(detailsStudentLink, createdStudent);
                    
                    //Edit student
                    editStudentLink = getIndexLinkElement(newStudentInIndexXPath, "editStudent");
                    var editedStudent = EditStudent(editStudentLink, randomFirstMidName);
                    
                    //get link for details
                    detailsStudentLink = getIndexLinkElement(newStudentInIndexXPath, "detailsStudent");
                    //Assert for details of edited student
                    SeeStudentsDetails(detailsStudentLink, editedStudent);

                    //Delete created student
                    deleteStudentLink = getIndexLinkElement(newStudentInIndexXPath, "deleteStudent");
                    DeleteStudent(deleteStudentLink);

                    //new student should not exist in students list
                    newStudentIDElement = BrowserHost.Driver.FindElements(By.XPath(newStudentInIndexXPath)).FirstOrDefault();
                    Assert.That(newStudentIDElement, Is.Null);
               }
           });
        }

        private OpenQA.Selenium.IWebElement getIndexLinkElement(string newStudentInIndexXPath, string elementClassName)
        {
            OpenQA.Selenium.IWebElement newStudentIDElement = BrowserHost.Driver.FindElements(By.XPath(newStudentInIndexXPath)).FirstOrDefault();
            var webElement =  newStudentIDElement.FindElements(By.XPath("ancestor::tr//descendant::a[@class='" + elementClassName + "']")).FirstOrDefault();
            return webElement;
        }

        [Obsolete("this method is not necessary",false)]
        public Student getNewStudentFromIndexView(string studentLocationInIndexXPath)
        {
         //  -lastName
            var newStudentIDElement = BrowserHost.Driver.FindElements(By.XPath(studentLocationInIndexXPath)).FirstOrDefault();

            var studentFirstName = newStudentIDElement.FindElements(By.XPath("ancestor::tr//descendant::span[@class='firstNameClass']")).FirstOrDefault();
            var studentLastName = newStudentIDElement.FindElements(By.XPath("ancestor::tr//descendant::span[@class='lastNameClass']")).FirstOrDefault();
            var studentEmailAddress = newStudentIDElement.FindElements(By.XPath("ancestor::tr//descendant::span[@class='emailAddressClass']")).FirstOrDefault();
            var studentEnrollmentDate = newStudentIDElement.FindElements(By.XPath("ancestor::tr//descendant::span[@class='enrollmentDateClass']")).FirstOrDefault();
            Student viewStudent = new Student
            {
                FirstMidName = studentFirstName.Text,
                LastName = studentLastName.Text,
                EmailAddress = studentEmailAddress.Text,
                EnrollmentDate = Convert.ToDateTime(studentEnrollmentDate.Text)
            };
            return viewStudent;
        }

        public Student CreateStudent()
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
            newStudentData.GenerateEmailFromName(EFCodeFirstSettings.getSchoolDomain());
            captureDataIntoStudentForm(newStudentData,true);
            Utilities.Wait(200);
            var createButton = BrowserHost.Driver.FindElement(By.Id("createBtn"));
            createButton.Click();
            return newStudentData;
        }

        public void captureDataIntoStudentForm(Student studentData, bool isCreating = false)
        {
            if (true == isCreating)
            {
                captureDataIntoControl("FirstMidName", studentData.FirstMidName);
            }
            else
            {
                captureDataIntoControl("EmailAddress", studentData.EmailAddress);
            }
            captureDataIntoControl("LastName",studentData.LastName);
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

        private void SeeStudentsDetails(OpenQA.Selenium.IWebElement detailsStudentLink, Student newStudentData)
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
            Assert.That(newStudentData.EnrollmentDate.ToShortDateString(), Is.EqualTo(element.Text));

            var backToListLink = BrowserHost.Driver.FindElement(By.Id("backToListLink"));
            backToListLink.Click();
            Utilities.Wait(standardTimeBetweenPagesMS);
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
