// u241217.1342_code
// u241217_documentation

namespace TingenLieutenant
{
    public class AsmxFile
    {
        public static string GetVersion(string filePath)
        {
            string asmxVersion = "Unknown";

            if (File.Exists(filePath))
            {
                using (StreamReader reader = new(filePath))
                {
                    asmxVersion = reader.ReadLine() ?? "";
                }

                asmxVersion = asmxVersion.Replace("//", "");
                asmxVersion = asmxVersion.Replace("=", "");
            }

            return asmxVersion.Trim();
        }
    }
}