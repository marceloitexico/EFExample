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
        private static int minimunHoursForFullTime = 20;
        private static int minimumFullTimeTeachers = 3;
        public _Views_Teacher_Index_cshtml TeacherIndexView
        {
            get {
                if (teacherIndexView == null)
                {
                    teacherIndexView = new _Views_Teacher_Index_cshtml();
                }
                return teacherIndexView;
            }
        }

        [Test]
        public void ShouldRenderTeachersList()
        {
            // var sut = new _Views_Teacher_Index_cshtml();
            List<Teacher> indexModel = DataHelper.GenerateTeachersList();
            HtmlDocument html = TeacherIndexView.RenderAsHtml(indexModel);

            var titleValueEls = html.DocumentNode.Descendants("span").Where(n => n.Attributes.Contains("class") && n.Attributes["class"].Value.Contains("titleValue"));
            var emailAddressValueEls = html.DocumentNode.Descendants("span").Where(n => n.Attributes.Contains("class") && n.Attributes["class"].Value.Contains("emailAddressValue"));
            var fullNameValueEls = html.DocumentNode.Descendants("span").Where(n => n.Attributes.Contains("class") && n.Attributes["class"].Value.Contains("fullNameValue"));
            var hoursPerWeekValueEls = html.DocumentNode.Descendants("span").Where(n => n.Attributes.Contains("class") && n.Attributes["class"].Value.Contains("hoursPerWeekValue"));
            Assert.Multiple(() =>
            {
                int teacherIndex = 0;
                //in this case, the Inner text should have the exact expected value.
                foreach (var modelItem in indexModel)
                {
                    if (modelItem.FullName != null)
                    {
                        Assert.That(fullNameValueEls.ElementAt(teacherIndex).InnerText, Is.EqualTo(modelItem.FullName), "does not match Full Name of teacher " + modelItem.FullName);
                    }
                    if (modelItem.Title != null)
                    {
                        Assert.That(titleValueEls.ElementAt(teacherIndex).InnerText, Is.EqualTo(modelItem.Title), "does not match the Title of teacher " + modelItem.FullName);
                    }
                    if (modelItem.EmailAddress != null)
                    {
                        Assert.That(emailAddressValueEls.ElementAt(teacherIndex).InnerText, Is.EqualTo(modelItem.EmailAddress), "does not match Email address of teacher " + modelItem.FullName);
                    }
                    if (modelItem.HoursPerWeek != null)
                    {
                        Assert.That(hoursPerWeekValueEls.ElementAt(teacherIndex).InnerText, Is.EqualTo(modelItem.HoursPerWeek.ToString()), "does not match Hour Per Week of teacher " + modelItem.FullName);
                    }
                    teacherIndex++;
                }
            });
        }

        [Test]
        public void VerifyCompliesMinimumFullTimeTeachers()
        {
            var sut = TeacherIndexView;
            List<Teacher> indexModel = DataHelper.GenerateTeachersList();
            //int FulltimeTeachers = indexModel.FindAll(t => t.HoursPerWeek > minimunHoursForFullTime).Count();
            int FulltimeTeachers = (from t in indexModel
                                    where t.HoursPerWeek >= minimunHoursForFullTime
                                    select t).Count();
            sut.ViewBag.FullTimeTeachers = indexModel.FindAll(t => t.HoursPerWeek >= minimunHoursForFullTime).Count();
            sut.ViewBag.PartTimeTeachers = indexModel.FindAll(t => t.HoursPerWeek < minimunHoursForFullTime).Count();
            HtmlDocument html = sut.RenderAsHtml(indexModel);
            var isMinimumCompliedMessageRendered = html.GetElementbyId("MoreThanTwoStudentsMessage") != null;
            var fullTimeTeachersString = html.GetElementbyId("fullTimeTeachersContainer").InnerText;
            int fullTimeTeachersFromView = 0;
            Assert.Multiple(() =>
            {
                Assert.That(int.TryParse(fullTimeTeachersString, out fullTimeTeachersFromView), Is.EqualTo(true));
                Assert.That(fullTimeTeachersFromView, Is.GreaterThanOrEqualTo(minimumFullTimeTeachers), "Amount of minimum full time teacher was not found");
            });
        }

        [Test]
        public void VerifyDoesNotComplyMinimumFullTimeTeachers()
        {
            var sut = TeacherIndexView;
            List<Teacher> indexModel = DataHelper.GenerateTeachersList();
            indexModel.RemoveAll(t => t.HoursPerWeek >= minimunHoursForFullTime);
            int FulltimeTeachers = (from t in indexModel
                                    where t.HoursPerWeek >= minimunHoursForFullTime
                                    select t).Count();

            sut.ViewBag.FullTimeTeachers = indexModel.FindAll(t => t.HoursPerWeek >= minimunHoursForFullTime).Count();
            sut.ViewBag.PartTimeTeachers = indexModel.FindAll(t => t.HoursPerWeek < minimunHoursForFullTime).Count();
            HtmlDocument html = sut.RenderAsHtml(indexModel);
            var isMinimumCompliedMessageRendered = html.GetElementbyId("MoreThanTwoStudentsMessage") != null;
            var fullTimeTeachersString = html.GetElementbyId("fullTimeTeachersContainer").InnerText;
            int fullTimeTeachersFromView = 0;
            Assert.That(int.TryParse(fullTimeTeachersString, out fullTimeTeachersFromView), Is.EqualTo(true));
            Assert.That(fullTimeTeachersFromView, Is.LessThan(minimumFullTimeTeachers), "Amount of minimum full time teacher was not found");
        }

        //test viewbag message, change it to pass the minimumhours for be a full time 
        [Test]
        public void ShouldRenderViewBagMessage()
        {
            string testMessage = "Message from ViewBag in Teacher Index View";
            //var sut = new _Views_Teacher_Index_cshtml();
            TeacherIndexView.ViewBag.Message = testMessage;
            List<Teacher> indexModel = DataHelper.GenerateTeachersList();
            HtmlDocument html = TeacherIndexView.RenderAsHtml(indexModel);
            var viewBagRenderedMessage = html.GetElementbyId("viewBagMsgContainer").InnerText;
            Assert.That(viewBagRenderedMessage, Is.EqualTo(testMessage), "ViewBag Messages do not match");
        }
    }
}
