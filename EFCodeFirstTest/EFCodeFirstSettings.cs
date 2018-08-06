using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCodeFirstTest
{
    public static class EFCodeFirstSettings
    {
        public static string EFApproachesWebConfigFile = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\EFApproaches\\Web.config"));
    }
}
