// u250129_code
// u250129_documentation

using System.IO;

using TingenLieutenant.Du;

namespace TingenLieutenant.Configuration
{
    public class LieutenantConfig
    {
        public string ServerUnc { get; set; }
        public string ServiceDataRoot { get; set; }

        /// <summary> Load the Tingen Lieutenant configuration file.</summary>
        /// <param name="configFilePath">The path to the Tingen Lieutenant configuration file.</param>
        /// <returns>The Tingen Lieutenant configuration object.</returns>
        internal static LieutenantConfig Load(string configFilePath)
        {
            if (!File.Exists(configFilePath))
            {
                Create(configFilePath);
            }

            return DuJson.ImportFromLocalFile<LieutenantConfig>(configFilePath);
        }

        /// <summary>Create a default Tingen Lieutenant configuration file.</summary>
        /// <param name="configFilePath">The path to the Tingen Lieutenant configuration file.</param>
        private static void Create(string configFilePath)
        {
            var defaultConfig = Build();

            DuJson.ExportToLocalFile<LieutenantConfig>(defaultConfig, configFilePath);
        }

        /// <summary>Build the default Tingen Lieutenant configuration object.</summary>
        /// <returns>A default Tingen Lieutenant configuration object.</returns>
        private static LieutenantConfig Build()
        {
            return new LieutenantConfig
            {
                ServerUnc   = "YOUR-SERVER-UNC-HERE",
                ServiceDataRoot = "TingenData"
            };
        }
    }
}
