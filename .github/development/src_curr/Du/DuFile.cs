// u241217.1457_code
// u241217_documentation

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TingenLieutenant.Du
{
    public class DuFile
    {
        public static bool VerifyExists(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}
