/* ████████ ██ ███    ██  ██████  ███████ ███    ██
 *    ██    ██ ██ ██  ██ ██   ███ █████   ██ ██  ██
 *    ██    ██ ██   ████  ██████  ███████ ██   ████
 *
 * ██      ██ ███████ ██    ██ ████████ ███████ ███    ██  █████  ███    ██ ████████
 * ██      ██ █████   ██    ██    ██    █████   ██ ██  ██ ███████ ██ ██  ██    ██
 * ███████ ██ ███████  ██████     ██    ███████ ██   ████ ██   ██ ██   ████    ██
 *                                            TingenLieutenant.Deployer.Deploy.cs
 */

// u250520_code
// u250520_documentation

using System.Text.Json;

namespace TingenLieutenant.Deployer
{
    /// <summary>Deployment logic for Tingen projects.</summary>
    /// <remarks>
    ///     <para>
    ///         This class contains the logic for deploying Tingen projects.<br/>
    ///         <br/>
    ///         These methods are not called directly, but are used by the <see cref="ViaDevDeploy"/> and <see cref="ViaCommander"/> classes to perform deployment tasks.
    ///     </para>
    /// </remarks>
    internal class Deploy
    {
        /// <summary>Location of the repository to be deployed.</summary>
        /// <remarks>
        ///     <para>
        ///         The location can be a URL or a local directory path.<br/>
        ///     </para>
        /// </remarks>
        /// <value>The default value is "<c>https://github.com/spectrum-health-systems/Tingen-WebService/archive/refs/heads/development.zip</c>"</value>
        public string RepositoryPath { get; set; }

        /// <summary>Root of the DevDeploy staging area.</summary>
        /// <value>The default value is "<c>C:\Tingen_Data\DevDeploy</c>"</value>
        public string DevDeployRoot { get; set; }

        /// <summary>Root of the service being deployed.</summary>
        /// <value>The default value is "<c>C:\Tingen\UAT</c>"</value>
        public string TargetRoot { get; set; }

        /// <summary>Determines the status of the configuration file.</summary>
        /// <param name="configPath">The file path of the configuration file.</param>
        /// <returns>A string indicating the status of the configuration file.</returns>
        public static string GetConfigFileStatus(string configPath)
        {
            return string.IsNullOrEmpty(configPath)
                ? "null-or-empty"
                : (!File.Exists(configPath))
                    ? "does-not-exist"
                    : "exists";
        }

        /// <summary>Creates a default configuration file for deployment.</summary>
        public static void CreateConfigFile(string configPath)
        {
            Deploy deployConfig = BuildDefaultConfig();
            WriteDevDeployConfig(configPath,deployConfig);
        }

        /// <summary>Determines the status of the repository path.</summary>
        /// <remarks>
        ///     <para>
        ///         The <see cref="RepositoryPath"/> can be a URL or a local directory path.<br/>
        ///         <br/>
        ///         If the RepostoryPath is invalid, the deployment process will not proceed.
        ///     </para>
        /// </remarks>
        /// <param name="repoPath">The repository path to evaluate. This can be a file path or a URL.</param>
        /// <returns>A string representing the status of the repository path.</returns>
        public static string GetRepoPathStatus(string repoPath)
        {
            return string.IsNullOrEmpty(repoPath)
                ? "null-or-empty"
                : repoPath.EndsWith(".zip", StringComparison.OrdinalIgnoreCase)
                    ? (!Uri.IsWellFormedUriString(repoPath, UriKind.Absolute))
                        ? "invalid-url"
                        : "valid-url"
                    : (!Directory.Exists(repoPath))
                        ? "does-not-exist"
                        : "exists";
        }

        public static string GetDevDeployRootStatus(string devDeployRoot)
        {
            return string.IsNullOrEmpty(devDeployRoot)
                ? "null-or-empty"
                : (!Directory.Exists(devDeployRoot))
                    ? "does-not-exist"
                    : "exists";
        }

        public static string GetTargetRootStatus(string targetRoot)
        {
            return string.IsNullOrEmpty(targetRoot)
                ? "null-or-empty"
                : (!Directory.Exists(targetRoot))
                    ? "does-not-exist"
                    : "exists";
        }

        /// <summary>Creates a default <see cref="Deploy"/> instance with preconfigured paths and repository settings.</summary>
        /// <returns>A <see cref="Deploy"/> object initialized with default values for the repository path.</returns>
        public static Deploy BuildDefaultConfig()
        {
            return new Deploy
            {
                RepositoryPath = "https://github.com/spectrum-health-systems/Tingen-WebService/archive/refs/heads/development.zip",
                DevDeployRoot  = @"C:\Tingen_Data\DevDeploy",
                TargetRoot     = @"C:\Tingen\UAT"
            };
        }

        /// <summary>Writes the deployment configuration for development to a file.</summary>
        /// <param name="deployConfig">The deployment configuration object to serialize and write to the file.</param>
        public static void WriteDevDeployConfig(string configPath, Deploy deployConfig)
        {
            var deployJson = JsonSerializer.Serialize(deployConfig, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(configPath, deployJson);
        }

        /// <summary>Loads the configuration file.</summary>
        /// <param name="configPath">The path to the configuration file.</param>
        /// <returns>An instance of <see cref="Deploy"/> representing the deserialized configuration.</returns>
        public static Deploy LoadConfigFile(string configPath)
        {
            var deployConfig = File.ReadAllText(configPath);
            return JsonSerializer.Deserialize<Deploy>(deployConfig);
        }


        public static void CopyDirectory(string sourcePath, string targetPath)
        {
            DirectoryInfo dirToCopy = new DirectoryInfo(sourcePath);
            DirectoryInfo[] subDirsToCopy = GetSubDirs(sourcePath); // redundant

            foreach (FileInfo file in dirToCopy.GetFiles())
            {
                _=file.CopyTo(Path.Combine(targetPath, file.Name));
            }

            foreach (var (subDir, newTargetDir) in from DirectoryInfo subDir in subDirsToCopy
                                                   let newTargetDir = Path.Combine(targetPath, subDir.Name)
                                                   select (subDir, newTargetDir))
            {
                CopyDirectory(subDir.FullName, newTargetDir);
            }
        }

        private static DirectoryInfo[] GetSubDirs(string sourcePath)
        {
            DirectoryInfo dirToCopy = new DirectoryInfo(sourcePath);

            return dirToCopy.GetDirectories();
        }
    }
}