using ASP;
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

namespace EFCodeFirstTest.ViewTests.TeacherViewTest
{
    [TestFixture]
    public class TeacherViewUnitTest
    {
        private _Views_Teacher_Index_cshtml teacherIndexView = null;
        public _Views_Teacher_Index_cshtml TeacherIndexView
        {
            get {
                if (teacherIndexView == null)
                {
                    teacherIndexView = new _Views_Teacher_Index_cshtml();
                }
            return teacherIndexView;
            } }

        [Test]
        public void ShouldRenderTeachersList()
        {
           // var sut = new _Views_Teacher_Index_cshtml();
            List<Teacher> indexModel = DataHelper.GenerateTeachersList();
            HtmlDocument html = TeacherIndexView.RenderAsHtml(indexModel);
            var firstMidNameValueEls = html.DocumentNode.Descendants("span").Where(n => n.Attributes.Contains("class") && n.Attributes["class"].Value.Contains("firstNameValue"));
            var lastNameValueEls = html.DocumentNode.Descendants("span").Where(n => n.Attributes.Contains("class") && n.Attributes["class"].Value.Contains("lastNameValue"));
            var titleValueEls = html.DocumentNode.Descendants("span").Where(n => n.Attributes.Contains("class") && n.Attributes["class"].Value.Contains("titleValue"));
            var emailAddressValueEls = html.DocumentNode.Descendants("span").Where(n => n.Attributes.Contains("class") && n.Attributes["class"].Value.Contains("emailAddressValue"));
            var fullNameValueEls = html.DocumentNode.Descendants("span").Where(n => n.Attributes.Contains("class") && n.Attributes["class"].Value.Contains("fullNameValue"));

            Assert.Multiple(() =>
            {
                int teacherNumber = 1;
                foreach (var modelItem in indexModel)
                {
                    if (modelItem.FirstMidName != null)
                    {
                        Assert.That(firstMidNameValueEls.ElementAt(teacherNumber-1).InnerText, Is.EqualTo(modelItem.FirstMidName), "does not contain First/Middle Name of teacher " + teacherNumber.ToString());
                    }
                    if (modelItem.LastName != null)
                    {
                        Assert.That(lastNameValueEls.ElementAt(teacherNumber - 1).InnerText, Is.EqualTo(modelItem.LastName), "does not contain Last Name of teacher " + teacherNumber.ToString());
                    }
                    if (modelItem.Title != null)
                    {
                        Assert.That(titleValueEls.ElementAt(teacherNumber - 1).InnerText, Is.EqualTo(modelItem.Title), "does not contain Title of teacher " + teacherNumber.ToString());
                    }
                    if (modelItem.EmailAddress != null)
                    {
                        Assert.That(emailAddressValueEls.ElementAt(teacherNumber - 1).InnerText, Is.EqualTo(modelItem.EmailAddress), "does not contain Email address of teacher " + teacherNumber.ToString());
                    }
                    if (modelItem.FullName != null)
                    {
                        Assert.That(fullNameValueEls.ElementAt(teacherNumber - 1).InnerText, Is.EqualTo(modelItem.FullName), "does not contain Full Name of teacher " + teacherNumber.ToString());
                    }
                    teacherNumber++;
                }
            });
        }

        //
        [Test]
        
        public void ShouldRenderViewBagMessage()
        {
            string testMessage = "Message from ViewBag in Teacher Index View";
            //var sut = new _Views_Teacher_Index_cshtml();
            TeacherIndexView.ViewBag.Message = testMessage;
            List<Teacher> indexModel = DataHelper.GenerateTeachersList();
            TeacherIndexView.ViewBag.FullTimeTeachers = indexModel.FindAll(t => t.HoursPerWeek > 20);
            TeacherIndexView.ViewBag.PartTimeTeachers = indexModel.FindAll(t => t.HoursPerWeek < 20);
            HtmlDocument html = TeacherIndexView.RenderAsHtml(indexModel);
            var viewBagRenderedMessage = html.GetElementbyId("viewBagMsgContainer").InnerText;
            Assert.That(viewBagRenderedMessage, Is.EqualTo(testMessage), "ViewBag Messages do not match");
        }
    }
}
