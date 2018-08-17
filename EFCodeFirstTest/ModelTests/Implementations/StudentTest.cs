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
using EFCodeFirstTest.ModelTests;
using EFCodeFirstTest.ModelTests.Interfaces;

namespace EFCodeFirst.UnitTest.Implementations
{
    [TestFixture]
    public class StudentTest : IModelTest
    {
        private string schoolDomain = "";

        [OneTimeSetUp]
        public void InitilizeOncePerRun()
        {
            Console.WriteLine("Initial message");
        }
        [OneTimeTearDown]
        public void CleanupOncePerRun()
        {
            Console.WriteLine("Final message");

        }
        /// <summary>
        /// This attribute is used inside a TestFixture to provide a common set of functions
        /// that are performed just before each test method is called.
        /// </summary>
        [SetUp]
        public void InitializeBeforeEachTest()
        {
            try
            {
                string name = TestContext.CurrentContext.Test.MethodName;
                if (0 == name.CompareTo("GenerateEmailFromNameTest"))
                {
                    if (true == string.IsNullOrEmpty(schoolDomain))
                    {
                        schoolDomain = getSchoolDomain();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [TearDown]
        public void CleanupAfterEachTest()
        {
            schoolDomain = "";
        }

        [Test]
        public void GenerateEmailFromNameTest([Random(0, 10000, 1)]int studentID)
        {
            try
            {
                var sut = new Student();
                sut.LastName = "myTestLastName" + studentID.ToString();
                
                sut.GenerateEmailFromName(schoolDomain);
                /*it fails if one of the asserts does not pass*/
                Assert.Multiple(
                    () => {
                        Assert.That(sut.EmailAddress, Is.EqualTo("myTestLastName" + studentID.ToString() + "@" + schoolDomain));
                    }
                );
            }
            catch (MultipleAssertException ex)
            {
                throw ex;
            }
            
        }

        [TestCase("Nathan", "Eldridge")]
        [TestCase("Samir", "Lakhani")]
        [TestCase("Camille", "Lozerone")]
        [TestCase("John", "Papa")]
        public void SetFullNameInConstructorTest(string firstMidName, string lastName)
        {
            try
            {
                var sut = new Student(firstMidName, lastName);
                Assert.That(sut.FullName, Is.EqualTo(firstMidName + " " + lastName));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region private methods
        private string getSchoolDomain()
        {
            string path = EFCodeFirstSettings.EFApproachesWebConfigFile;
            XDocument xdoc = XDocument.Load(path);
            var schoolDomain = xdoc.Element("configuration").Element("appSettings").Elements("add")
                            .Where(x => (string)x.Attribute("key") == "SchoolDomain")
                            .Single().Attribute("value");
            return schoolDomain.Value;
        }
        #endregion private methods
    }


}
