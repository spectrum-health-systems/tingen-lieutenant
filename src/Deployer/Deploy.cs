/* ████████ ██ ███    ██  ██████  ███████ ███    ██
 *    ██    ██ ██ ██  ██ ██   ███ █████   ██ ██  ██
 *    ██    ██ ██   ████  ██████  ███████ ██   ████
 *
 * ██      ██ ███████ ██    ██ ████████ ███████ ███    ██  █████  ███    ██ ████████
 * ██      ██ █████   ██    ██    ██    █████   ██ ██  ██ ███████ ██ ██  ██    ██
 * ███████ ██ ███████  ██████     ██    ███████ ██   ████ ██   ██ ██   ████    ██
 *                                            TingenLieutenant.Deployer.Deploy.cs
 */

// u250521_code
// u250521_documentation

using System.IO.Compression;
using System.Net;
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
        ///         The RepositoryPath can either be a URL,or a directory.<br/>
        ///         <br/>
        ///         If the RepositoryPath is a URL:
        ///         <list type="bullet">
        ///             <item>It must point to a zip file</item>
        ///             <item>It must formatted correctly</item>
        ///         </list>
        ///         If the RepositoryPath is a directory:
        ///         <list type="bullet">
        ///             <item>It can be a local directory</item>
        ///             <item>It can be a network share/mapped drive</item>
        ///         </list>
        ///     </para>
        /// </remarks>
        /// <value>The default value is "<c>https://github.com/spectrum-health-systems/Tingen-WebService/archive/refs/heads/development.zip</c>"</value>
        public string RepositoryPath { get; set; }

        /// <summary>Root of the DevDeploy staging area.</summary>
        /// <remarks>
        ///     <para>
        ///     The DevDeployRoot is the location where the repository will be unzipped and staged for deployment.<br/>
        ///     </para>
        ///     <para>
        ///         The DevDeployRoot:<br/>
        ///         <list type="bullet">
        ///             <item>Can be a local directory</item>
        ///             <item>Can be a network share/mapped drive</item>
        ///         </list>
        ///     </para>
        /// </remarks>
        /// <value>The default value is "<c>C:\Tingen_Data\DevDeploy</c>"</value>
        public string DevDeployRoot { get; set; }

        /// <summary>Root of the service being deployed.</summary>
        /// <remarks>
        ///     <para>
        ///     The TargetRoot is the location where the repository will be deployed.<br/>
        ///     </para>
        ///     <para>
        ///         The TargetRoot:<br/>
        ///         <list type="bullet">
        ///             <item>Can be a local directory</item>
        ///             <item>Can be a network share/mapped drive</item>
        ///         </list>
        ///     </para>
        /// </remarks>
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

        /// <summary>Determines the status of the deployment root.</summary>
        /// <param name="devDeployRoot">The path to the development deployment root directory to check.</param>
        /// <returns>A string indicating the status of the deployment root.</returns>
        public static string GetDevDeployRootStatus(string devDeployRoot)
        {
            return string.IsNullOrEmpty(devDeployRoot)
                ? "null-or-empty"
                : (!Directory.Exists(devDeployRoot))
                    ? "does-not-exist"
                    : "exists";
        }


        /// <summary>Determines the status of the target root.</summary>
        /// <param name="targetRoot">The path to the target root directory to check. This can be a relative or absolute path.</param>
        /// <returns>A string representing the status of the target root.</returns>
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

        /// <summary>Cleans and recreates the staging directory.</summary>
        /// <param name="devDeployRoot">The root directory of the development deployment.</param>
        public static void CleanStaging(string devDeployRoot)
        {
            if (Directory.Exists($@"{devDeployRoot}\staging"))
            {
                Directory.Delete($@"{devDeployRoot}\staging", true);
            }

            Directory.CreateDirectory($@"{devDeployRoot}\staging");
        }

        /// <summary>Cleans and prepares the specified target.</summary>
        /// <param name="targetRoot">The root directory to clean and initialize. Must be a valid directory path.</param>
        public static void CleanTarget(string targetRoot)
        {
            if (Directory.Exists($@"{targetRoot}"))
            {
                Directory.Delete($@"{targetRoot}", true);
            }

            Directory.CreateDirectory($@"{targetRoot}");
            Directory.CreateDirectory($@"{targetRoot}\bin");
            Directory.CreateDirectory($@"{targetRoot}\bin\roslyn");
            Directory.CreateDirectory($@"{targetRoot}\bin\AppData");
            Directory.CreateDirectory($@"{targetRoot}\bin\Runtime");
        }

        /// <summary>Downloads and extracts a remote repository.</summary>
        /// <param name="repoPath">The URL of the remote repository archive to download.</param>
        /// <param name="devDeployRoot">The root directory where the repository archive will be staged.</param>
        public static void GetRemoteRepostitory(string repoPath, string devDeployRoot)
        {
            var client = new WebClient();
            client.DownloadFile(repoPath, $@"{devDeployRoot}\staging\webservice.zip");
            ZipFile.ExtractToDirectory($@"{devDeployRoot}\staging\webservice.zip", $@"{devDeployRoot}\staging\");
        }

        /// <summary>Copies all files and subdirectories from the specified source directory to the target directory.</summary>
        /// <param name="sourcePath">The path of the directory to copy. Must be a valid, existing directory.</param>
        /// <param name="targetPath">The path of the target directory where the contents will be copied.</param>
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

        /// <summary>Retrieves the subdirectories of the specified directory path.</summary>
        /// <param name="sourcePath">The path of the directory whose subdirectories are to be retrieved.</param>
        /// <returns>An array of <see cref="DirectoryInfo"/> objects representing the subdirectories of the specified directory.</returns>
        private static DirectoryInfo[] GetSubDirs(string sourcePath)
        {
            DirectoryInfo dirToCopy = new DirectoryInfo(sourcePath);

            return dirToCopy.GetDirectories();
        }

        /// <summary>Retrieves a list of service-related file names.</summary>
        /// <returns>A list of strings representing the names of service-related files.</returns>
        public static List<string> ListOfServiceFiles()
        {
            return
            [
                "TingenWebService.asmx",
                "TingenWebService.asmx.cs",
                "packages.config",
                "Web.config",
                "Web.Debug.config",
                "Web.Release.config"
            ];
        }
    }
}