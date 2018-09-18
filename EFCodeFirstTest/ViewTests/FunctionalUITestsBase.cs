using BankingSite.FunctionalUITests;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCodeFirstTest.ViewTests
{
    public abstract class FunctionalUITestsBase
    {
        protected internal static int standardTimeBetweenPagesMS = 4000;
        protected internal static int standardTimeToSeeData = 2000;

        protected internal void captureDataIntoControl(string elementID, string data)
        {
            var element = BrowserHost.Driver.FindElement(By.Id(elementID));
            element.Clear();
            element.SendKeys(data);
        }

        protected internal IWebElement getIndexLinkElement(string newCourseInIndexXPath, string elementClassName)
        {
            OpenQA.Selenium.IWebElement newTeacherIDElement = BrowserHost.Driver.FindElements(By.XPath(newCourseInIndexXPath)).FirstOrDefault();
            var webElement = newTeacherIDElement.FindElements(By.XPath("ancestor::tr//descendant::a[@class='" + elementClassName + "']")).FirstOrDefault();
            return webElement;
        }
    }
}
