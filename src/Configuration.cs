// u250128_code
// u250128_documentation

using System.IO;

using TingenLieutenant.Du;

namespace TingenLieutenant
{
    /// <summary>Configuration-related stuff for the Tingen Lieutenant application.</summary>
    class Configuration
    {
        public string ServerUnc { get; set; }
        public string ServiceDataRoot { get; set; }

        /// <summary> Load the Tingen Lieutenant configuration file.</summary>
        /// <param name="configFilePath">The path to the Tingen Lieutenant configuration file.</param>
        /// <returns>The Tingen Lieutenant configuration object.</returns>
        internal static Configuration Load(string configFilePath)
        {
            if (!File.Exists(configFilePath))
            {
                Create(configFilePath);
            }

            return DuJson.ImportFromLocalFile<Configuration>(configFilePath);
        }

        /// <summary>Create a default Tingen Lieutenant configuration file.</summary>
        /// <param name="configFilePath">The path to the Tingen Lieutenant configuration file.</param>
        private static void Create(string configFilePath)
        {
            var defaultConfig = Build();

            DuJson.ExportToLocalFile<Configuration>(defaultConfig, configFilePath);
        }

        /// <summary>Build the default Tingen Lieutenant configuration object.</summary>
        /// <returns>A default Tingen Lieutenant configuration object.</returns>
        private static Configuration Build()
        {
            return new Configuration
            {
                ServerUnc   = "YOUR-SERVER-UNC-HERE",
                ServiceDataRoot = "TingenData"
            };
        }
    }
}