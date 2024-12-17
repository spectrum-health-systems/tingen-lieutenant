using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TingenLieutenant
{
    public class AsmxFiles
    {

        public static string GetAsmxVersion(string asmxFile)
        {
            string asmxVersion = "Unknown";

            if (System.IO.File.Exists(asmxFile))
            {
                using (StreamReader reader = new StreamReader(asmxFile))
                {
                    asmxVersion = reader.ReadLine() ?? "";
                }

                asmxVersion = asmxVersion.Replace("//", "");
                asmxVersion = asmxVersion.Replace("=", "");

                asmxVersion = asmxVersion.Trim();
            }

            return asmxVersion;

        }
    }
}
