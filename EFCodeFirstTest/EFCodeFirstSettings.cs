using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EFCodeFirstTest
{
    public static class EFCodeFirstSettings
    {
        public static string getSchoolDomain()
        {
            string EFApproachesWebConfigFile = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\EFApproaches\\Web.config"));
            string path = EFApproachesWebConfigFile;
            XDocument xdoc = XDocument.Load(path);
            var schoolDomain = xdoc.Element("configuration").Element("appSettings").Elements("add")
                            .Where(x => (string)x.Attribute("key") == "SchoolDomain")
                            .Single().Attribute("value");
            return schoolDomain.Value;
        }
    }
}
