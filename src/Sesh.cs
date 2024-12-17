// u241217.1143_code
// u241217_documentation

using System.IO;
using TingenLieutenant.Du;

namespace TingenLieutenant
{
    public class Sesh
    {
        /// <summary>DOC </summary>
        public string LocalRoot { get; set; }

        /// <summary>DOC </summary>
        public string DataRoot { get; set; }

        /// <summary>DOC </summary>
        public string RemoteRoot { get; set; }

        /// <summary>DOC</summary>
        /// <param name="configFilePath"></param>
        public static Sesh LoadConfiguration(string configFilePath)
        {
            VerifyConfigFileExists(configFilePath);

            return DuJson.ImportFromLocalFile<Sesh>(configFilePath);
        }

        /// <summary>DOC</summary>
        /// <param name="seshDataDirectory"></param>
        public static void ResetSessionData(string seshDataDirectory)
        {
            if (Directory.Exists(seshDataDirectory))
            {
                Directory.Delete(seshDataDirectory, true);
            }

            Directory.CreateDirectory(seshDataDirectory);
        }

        /// <summary>DOC</summary>
        /// <param name="configFilePath"></param>
        private static void VerifyConfigFileExists(string configFilePath)
        {
            if (!File.Exists(configFilePath))
            {
                CreateNewConfigFile(configFilePath);
            }
        }

        /// <summary>DOC</summary>
        /// <param name="configFilePath"></param>
        private static void CreateNewConfigFile(string configFilePath)
        {
            var configuration = BuildDefaultSession();

            DuJson.ExportToLocalFile<Sesh>(configuration, configFilePath);
        }

        /// <summary>DOC</summary>
        private static Sesh BuildDefaultSession()
        {
            Sesh defaultSession = new Sesh
            {
                LocalRoot = @".\AppData",
                RemoteRoot = @"C:\TingenData"
            };

            defaultSession.DataRoot = $@"{defaultSession.LocalRoot}\Session";

            return defaultSession;
        }
    }
}
