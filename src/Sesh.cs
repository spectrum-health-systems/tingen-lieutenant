// u241217.1143_code
// u241217_documentation

using System.IO;
using TingenLieutenant.Du;

namespace TingenLieutenant
{
    public class Sesh
    {
        public string SessionRoot { get; set; }
        public string RemoteRoot { get; set; }
        public string MainBranchUrl { get;set; }
        public string DevelopmentBranchUrl { get; set; }

        public static Sesh LoadConfiguration(string filePath)
        {
            VerifyConfigFileExists(filePath);

            return DuJson.ImportFromLocalFile<Sesh>(filePath);
        }

        public static void ResetSessionData(string sessionRoot)
        {
            if (Directory.Exists(sessionRoot))
            {
                Directory.Delete(sessionRoot, true);
            }

            Directory.CreateDirectory(sessionRoot);
        }

        private static void VerifyConfigFileExists(string filePath)
        {
            if (!File.Exists(filePath))
            {
                CreateNewConfigFile(filePath);
            }
        }

        private static void CreateNewConfigFile(string filePath)
        {
            var configuration = BuildDefault();

            DuJson.ExportToLocalFile<Sesh>(configuration, filePath);
        }

        private static Sesh BuildDefault()
        {
            return new Sesh
            {
                SessionRoot          = @".\AppData\Session",
                RemoteRoot           = @"C:\TingenData",
                MainBranchUrl        = "https://raw.githubusercontent.com/spectrum-health-systems/Tingen-Development/refs/heads/main/src/Tingen_development.asmx.cs",
                DevelopmentBranchUrl = "https://raw.githubusercontent.com/spectrum-health-systems/Tingen-Development/refs/heads/development/src/Tingen_development.asmx.cs"
            };
        }
    }
}