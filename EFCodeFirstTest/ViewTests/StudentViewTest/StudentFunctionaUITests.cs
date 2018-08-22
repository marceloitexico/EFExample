using BankingSite.FunctionalUITests;
using BankingSite.FunctionalUITests.DemoHelperCode;
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
        [Test]
        public void ShouldAcceptLoanApplication()
        {
            int randomID = Utilities.GenerateRandomNumber();
            string randomFirstMidName = "Gecko" + randomID.ToString();
            BrowserHost.Driver.Navigate().GoToUrl(BrowserHost.RootUrl + @"Student\Create");

            var firstMidNameBox = BrowserHost.Driver.FindElement(By.Id("FirstMidName"));
            firstMidNameBox.SendKeys(randomFirstMidName);
            var lastNameBox = BrowserHost.Driver.FindElement(By.Id("LastName"));
            lastNameBox.SendKeys("Gecko_LastName");
            var emailAddressBox = BrowserHost.Driver.FindElement(By.Id("EmailAddress"));
            emailAddressBox.SendKeys("replaceableAddress@Gecko.com");
            var enrollmentDateBox = BrowserHost.Driver.FindElement(By.Id("EnrollmentDate"));
            enrollmentDateBox.SendKeys("2018/02/02");

            Utilities.Wait(200);
            var createButton = BrowserHost.Driver.FindElement(By.Id("createBtn"));
            createButton.Click();
            Utilities.Wait();
            var students = BrowserHost.Driver.FindElements(By.ClassName("firstMidNameClass"));
            bool studentExists = false;
            foreach (var item in students)
            {
                if (0 == randomFirstMidName.CompareTo(item.Text))
                {
                    studentExists = true;
                    break;
                }
            }
            Assert.That(true, Is.EqualTo(studentExists));
        }
    }
}
