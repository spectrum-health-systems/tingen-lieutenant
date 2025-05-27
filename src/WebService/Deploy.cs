/* ████████ ██ ███    ██  ██████  ███████ ███    ██
 *    ██    ██ ██ ██  ██ ██   ███ █████   ██ ██  ██
 *    ██    ██ ██   ████  ██████  ███████ ██   ████
 *
 * ██      ██ ███████ ██    ██ ████████ ███████ ███    ██  █████  ███    ██ ████████
 * ██      ██ █████   ██    ██    ██    █████   ██ ██  ██ ███████ ██ ██  ██    ██
 * ███████ ██ ███████  ██████     ██    ███████ ██   ████ ██   ██ ██   ████    ██
 *                                          TingenLieutenant.WebService.Deploy.cs
 */

// u250522_code
// u250527_documentation

using System.IO.Compression;
using System.Net;
using System.Text.Json;

namespace TingenLieutenant.WebService
{
    /// <summary>Logic for deploying the Tingen Web Service.</summary>
    /// <remarks>
    ///     <para>
    ///         Technically this class can be used to deploy any branch of the Tingen Web Service,<br/>
    ///         but it's intended use to deploy the development branch for testing purposes.<br/>
    ///         <br/>
    ///         These methods are used by:
    ///         <list type="bullet">
    ///             <item><see cref="TingenLieutenant.ViaDevDeploy"/> - Command line interfaces</item>
    ///             <item><see cref="TingenLieutenant.ViaCommander"/> - Graphical interfaces</item>
    ///         </list>
    ///     </para>
    /// </remarks>
    internal class Deploy
    {
        /// <summary>The location of the Tingen Web Service that will be deployed.</summary>
        /// <remarks>
        ///     <para>
        ///         This isn't necessarily a "path", since it can be either a directory or a URL.<br/>
        ///         <br/>
        ///         If the RepositoryPath is a URL:
        ///         <list type="bullet">
        ///             <item>It must point to a ".zip" file</item>
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

        /// <summary>The location where the Tingen Web Service is staged for deployment.</summary>
        /// <remarks>
        ///     <para>
        ///         The StagingPath:<br/>
        ///         <list type="bullet">
        ///             <item>Can be a local directory</item>
        ///             <item>Can be a network share/mapped drive</item>
        ///         </list>
        ///     </para>
        /// </remarks>
        /// <value>The default value is "<c>C:\Tingen_Data\DevDeploy</c>"</value>
        public string StagingPath { get; set; }

        /// <summary>The location where the Tingen Web Service is deployed.</summary>
        /// <remarks>
        ///     <para>
        ///     The DeployPath is the location where the repository will be deployed.<br/>
        ///     </para>
        ///     <para>
        ///         The DeployPath:<br/>
        ///         <list type="bullet">
        ///             <item>Can be a local directory</item>
        ///             <item>Can be a network share/mapped drive</item>
        ///         </list>
        ///     </para>
        /// </remarks>
        /// <value>The default value is "<c>C:\Tingen\UAT</c>"</value>
        public string DeployPath { get; set; }

        public static void VerifyFramework()
        {
            Directory.CreateDirectory("./AppData/");
        }

        /// <summary>Determines the status of the configuration file.</summary>
        /// <remarks>
        ///     <para>
        ///         If the <paramref name="configPath"/> is invalid, the deployment process will not proceed.
        ///     </para>
        /// </remarks>
        /// <param name="configPath">The configuration file path.</param>
        /// <returns>
        ///     The status of the configuration file:
        ///     <list type="bullet">
        ///         <item>null-or-empty: The passed <paramref name="configPath"/> is null/empty</item>
        ///         <item>does-not-exist: The config file does not exist at the <paramref name="configPath"/></item>
        ///         <item>exists: The config file does exist at the <paramref name="configPath"/></item>
        ///     </list>
        /// </returns>
        public static string ConfigFileStatus(string configPath)
        {
            return string.IsNullOrEmpty(configPath)
                ? "null-or-empty"
                : (!File.Exists(configPath))
                    ? "does-not-exist"
                    : "exists";
        }

        /// <summary>Writes a default configuration file.</summary>
        public static void CreateDefaultConfigFile(string configPath)
        {
            var deployJson = JsonSerializer.Serialize(BuildDefaultConfig(), new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(configPath, deployJson);
        }

        /// <summary>Determines the status of the <see cref="RepositoryPath"/>.</summary>
        /// <remarks>
        ///     <para>
        ///         The <paramref name="repositoryPath"/> can be a URL or a local directory path.<br/>
        ///         <br/>
        ///         If the RepostoryPath is invalid, the deployment process will not proceed.
        ///     </para>
        /// </remarks>
        /// <param name="repositoryPath">The repository path.</param>
        /// <returns>
        ///     The status of the repository path:
        ///     <list type="bullet">
        ///         <item>null-or-empty: The passed <paramref name="repositoryPath"/> is null/empty</item>
        ///         <item>invalid-url: The <paramref name="repositoryPath"/> is a URL, and is not formatted correctly</item>
        ///         <item>valid-url: The <paramref name="repositoryPath"/> is a URL, and is formatted correctly</item>   
        ///         <item>does-not-exist: The <paramref name="repositoryPath"/> is a local/shared drive, and does not exist</item>
        ///         <item>exists: The <paramref name="repositoryPath"/> is a local/shared drive, and does exist</item>
        ///     </list>
        /// </returns>
        public static string RepositoryPathStatus(string repositoryPath)
        {
            return string.IsNullOrEmpty(repositoryPath)
                ? "null-or-empty"
                : repositoryPath.EndsWith(".zip", StringComparison.OrdinalIgnoreCase)
                    ? (!Uri.IsWellFormedUriString(repositoryPath, UriKind.Absolute))
                        ? "invalid-url"
                        : "valid-url"
                    : (!Directory.Exists(repositoryPath))
                        ? "does-not-exist"
                        : "exists";
        }

        /// <summary>Determines the status of the <see cref="StagingPath"/>.</summary>
        /// <remarks>
        ///     <para>
        ///         If the <paramref name="stagingPath"/> is invalid, the deployment process will not proceed.
        ///     </para>
        /// </remarks>
        /// <param name="stagingPath">The staging path.</param>
        /// <returns>
        ///     The status of the staging path:
        ///     <list type="bullet">
        ///         <item>null-or-empty: The passed <paramref name="stagingPath"/> is null/empty</item>
        ///         <item>does-not-exist: The <paramref name="stagingPath"/> does not exist</item>
        ///         <item>exists: The <paramref name="stagingPath"/> does exist</item>
        ///     </list>
        /// </returns>
        public static string StagingPathStatus(string stagingPath)
        {
            return string.IsNullOrEmpty(stagingPath)
                ? "null-or-empty"
                : (!Directory.Exists(stagingPath))
                    ? "does-not-exist"
                    : "exists";
        }

        /// <summary>Determines the status of the <see cref="DeployPath"/>.</summary>
        /// <remarks>
        ///     <para>
        ///         If the <paramref name="deployPath"/> is invalid, the deployment process will not proceed.
        ///     </para>
        /// </remarks>
        /// <param name="deployPath">The deployment path.</param>
        /// <returns>
        ///     The status of the deployment path:
        ///     <list type="bullet">
        ///         <item>null-or-empty: The passed <paramref name="deployPath"/> is null/empty</item>
        ///         <item>does-not-exist: The <paramref name="deployPath"/> does not exist</item>
        ///         <item>exists: The <paramref name="deployPath"/> does exist</item>
        ///     </list>
        /// </returns>
        public static string DeployPathStatus(string deployPath)
        {
            return string.IsNullOrEmpty(deployPath)
                ? "null-or-empty"
                : (!Directory.Exists(deployPath))
                    ? "does-not-exist"
                    : "exists";
        }

        /// <summary>Creates a default <see cref="Deploy"/> instance with preconfigured paths and repository settings.</summary>
        /// <remarks>
        ///     <para>
        ///         These are the default configuration values for a standard implementation of the Tingen Web Service.<br/>
        ///     </para>
        /// </remarks>
        /// <returns>A <see cref="Deploy"/> object initialized with default values for the repository path.</returns>
        public static Deploy BuildDefaultConfig()
        {
            return new Deploy
            {
                RepositoryPath = "https://github.com/spectrum-health-systems/Tingen-WebService/archive/refs/heads/development.zip",
                StagingPath    = @"C:\Tingen_Data\WebService\staging",
                DeployPath     = @"C:\Tingen\UAT"
            };
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
        /// <param name="stagingPath">The root directory of the development deployment.</param>
        public static void CleanStagingPath(string stagingPath)
        {
            if (Directory.Exists($@"{stagingPath}"))
            {
                Directory.Delete($@"{stagingPath}", true);
            }

            Directory.CreateDirectory($@"{stagingPath}");
        }

        /// <summary>Cleans and prepares the specified target.</summary>
        /// <param name="deployPath">The root directory to clean and initialize. Must be a valid directory path.</param>
        public static void CleanDeployPath(string deployPath)
        {
            if (Directory.Exists($@"{deployPath}"))
            {
                Directory.Delete($@"{deployPath}", true);
            }

            Directory.CreateDirectory($@"{deployPath}");
            Directory.CreateDirectory($@"{deployPath}\bin");
            Directory.CreateDirectory($@"{deployPath}\bin\roslyn");
            Directory.CreateDirectory($@"{deployPath}\bin\AppData");
            Directory.CreateDirectory($@"{deployPath}\bin\AppData\Runtime");
        }

        /// <summary>Downloads and extracts a remote repository.</summary>
        /// <param name="repositoryPath">The URL of the remote repository archive to download.</param>
        /// <param name="stagingPath">The root directory where the repository archive will be staged.</param>
        public static void GetRemoteRepository(string repositoryPath, string stagingPath)
        {
            var client = new WebClient();
            client.DownloadFile(repositoryPath, $@"{stagingPath}\tingen-web-service.zip");
            ZipFile.ExtractToDirectory($@"{stagingPath}\tingen-web-service.zip", $@"{stagingPath}\");
        }

        /// <summary>Copies all files and subdirectories from the specified source directory to the target directory.</summary>
        /// <param name="sourcePath">The path of the directory to copy. Must be a valid, existing directory.</param>
        /// <param name="targetPath">The path of the target directory where the contents will be copied.</param>
        public static void CopyDirectory(string sourcePath, string targetPath)
        {
            DirectoryInfo srcDir = new DirectoryInfo(sourcePath);

            foreach (FileInfo file in srcDir.GetFiles())
            {
                file.CopyTo(Path.Combine(targetPath, file.Name), true);
            }
        }
    }
}