using EFApproaches.DAL.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Web;
using EFCodeFirstTest;

namespace EFCodeFirst.UnitTest
{
    [TestFixture]
    public class StudentTest
    {
        [Test]
        public void GenerateEmailFromNameTest()
        {
            var sut = new Student();
            sut.LastName = "myTestLastName";
            string path = EFCodeFirstSettings.EFApproachesWebConfigFile;
            
            XDocument xdoc = XDocument.Load(path);
            var schoolDomain = xdoc.Element("configuration").Element("appSettings").Elements("add")
                            .Where(x => (string)x.Attribute("key") == "SchoolDomain")
                            .Single().Attribute("value");
            sut.GenerateEmailFromName(schoolDomain.Value);
            Assert.That(sut.EmailAddress,Is.EqualTo("myTestLastName@" +schoolDomain.Value));
        }

    }
}
