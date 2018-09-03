using ASP;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using HtmlAgilityPack;
using RazorGenerator.Testing;
using EFCodeFirstTest.Helpers;
using EFApproaches.DAL.Entities;

namespace EFCodeFirstTest.ViewTests.StudentViewTest
{
    [TestFixture]
    public class StudentViewUnitTests
    {
        [Test]
        public void ShouldRenderStudentsList()
        {
            var sut = new _Views_Student_Index_cshtml();
            List<Student> indexModel = DataHelper.GenerateStudentsList();
            string str = sut.Render(indexModel);
            bool containsObject1 = str.Contains("Nathan") && str.Contains("Eldridge") && str.Contains("neldridge@domain.com") && str.Contains("20/02/2018");
            bool containsObject2 = str.Contains("Samir") && str.Contains("Lakhani") && str.Contains("slakhani@domain.com") && str.Contains("21/03/2018");
            bool containsObject3 = str.Contains("Camille") && str.Contains("Lozerone") && str.Contains("clozerone@domain.com") && str.Contains("22/04/2018");
            bool containsObject4 = str.Contains("John") && str.Contains("Papa") && str.Contains("jpapa@domain.com") && str.Contains("23/05/2018");
            Assert.Multiple(() =>
            {
                Assert.That(true == containsObject1, "does not contain student 1");
                Assert.That(true == containsObject2, "does not contain student 2");
                Assert.That(true == containsObject3, "does not contain student 3");
                Assert.That(true == containsObject4, "does not contain student 4");
            });
            /*       HtmlNode node = html.DocumentNode.Element("table");
               var hrNodes = node.Elements("tr");
               Assert.AreEqual(5, hrNodes.Count<HtmlNode>());*/
        }

        [Test]
        public void ShouldRenderViewBagMessage()
        {
            var sut = new _Views_Student_Index_cshtml();
            sut.ViewBag.Message = "This is a test message from ViewBag";
            List<Student> indexModel = DataHelper.GenerateStudentsList();
            HtmlDocument html = sut.RenderAsHtml(indexModel);
            var viewBagRenderedMessage = html.GetElementbyId("viewBagMsgContainer").InnerText;
            Assert.That(viewBagRenderedMessage, Is.EqualTo("This is a test message from ViewBag"),"ViewBag Customized Message");
        }

        [Test]
        public void VerifyCompliesMinumumDesirableAmountOfStudents()
        {
            var sut = new _Views_Student_Index_cshtml();
            List<Student> indexModel = DataHelper.GenerateStudentsList();
            HtmlDocument html = sut.RenderAsHtml(indexModel);
            var isMinimumCompliedMessageRendered = html.GetElementbyId("MoreThanTwoStudentsMessage") != null;
            Assert.That(isMinimumCompliedMessageRendered, Is.True);
        }
        [Test]
        public void VerifyDoesNotComplyMinumumDesirableAmountOfStudents() 
        {
            var sut = new _Views_Student_Index_cshtml();
            List<Student> indexModel = DataHelper.GenerateStudentsList();
            indexModel.RemoveAt(0);
            HtmlDocument html = sut.RenderAsHtml(indexModel);
            //var message = html.GetElementbyId("amountMessagesContainer").InnerText;
            var isMinimumNotCompliedMessageRendered = html.GetElementbyId("MaxThreeStudentsMessage") != null;
            Assert.That(isMinimumNotCompliedMessageRendered,Is.True);
        }

        [Test]
        [Ignore("Just to check content of Field using DisplayNameFor helper")]
        public void failedTestByDisplayNameFor()
        {
            var sut = new _Views_Student_Index_cshtml();
            List<Student> indexModel = DataHelper.GenerateStudentsList();
            indexModel.RemoveAt(0);
            indexModel.RemoveAt(0);
            indexModel.RemoveAt(0);
            HtmlDocument html = sut.RenderAsHtml(indexModel);
            var firstMidNameValue = html.GetElementbyId("firstMidNameValue").InnerText;
            Assert.That(firstMidNameValue, Is.EqualTo("John"));

        }

        /*
        public void failedTestByDisplayNameFor()
        {
            var sut = new _Views_Student_Index_cshtml();
            List<Student> indexModel = DataHelper.GenerateStudentsList();
            var indexModelCount = indexModel.Count;
            for (int stdCount = 0 ; stdCount < (indexModelCount - 1); stdCount++)
            {
                indexModel.RemoveAt(0);
            }
            HtmlDocument html = sut.RenderAsHtml(indexModel);
            var firstMidNameValue = html.GetElementbyId("firstMidNameValue").InnerText;
            Assert.That(firstMidNameValue, Is.EqualTo("John"));
        }*/
    }
}
