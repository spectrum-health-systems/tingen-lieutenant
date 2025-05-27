/* ████████ ██ ███    ██  ██████  ███████ ███    ██
 *    ██    ██ ██ ██  ██ ██   ███ █████   ██ ██  ██
 *    ██    ██ ██   ████  ██████  ███████ ██   ████
 *
 * ██      ██ ███████ ██    ██ ████████ ███████ ███    ██  █████  ███    ██ ████████
 * ██      ██ █████   ██    ██    ██    █████   ██ ██  ██ ███████ ██ ██  ██    ██
 * ███████ ██ ███████  ██████     ██    ███████ ██   ████ ██   ██ ██   ████    ██
 *                                          TingenLieutenant.WebService.Deploy.cs
 */

// u250527_code
// u250527_documentation

using System.IO.Compression;
using System.Net;
using System.Text.Json;

using TingenLieutenant.Blueprint;

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
    public class Deploy
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
        public string Source { get; set; }

        public string SourceType { get; set; }

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
        public string StagePath { get; set; }

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

        public static void DeploymentProcess(string configPath, bool useCli)
        {
            Lieutenant.DisplayMessage(UserMessage.DeploymentProcess("start"), useCli);

            VerifyFramework(useCli);

            VerifyConfiguration(configPath, useCli);
            Deploy deployConfig = LoadConfigFile(configPath, useCli);

            VerifyStagePath(deployConfig.StagePath, useCli);
            VerifyDeployPath(deployConfig.DeployPath, useCli);

            RefreshStagePath(deployConfig.StagePath, useCli);
            RefreshDeployPath(deployConfig.DeployPath, useCli);

            deployConfig.SourceType = GetSourceType(deployConfig.Source, useCli);

            if (deployConfig.SourceType == "url")
            {
                VerifySourceUrl(deployConfig.Source, useCli);
                DownloadSourceUrl(deployConfig.Source, deployConfig.StagePath, useCli);
                deployConfig.StagePath = $@"{deployConfig.StagePath}\tingen-web-service-development\src";
            }
            else
            {
                VerifySourcePath(deployConfig.Source, useCli);
                deployConfig.StagePath = $@"{deployConfig.Source}\src";
            }

            DeployService(deployConfig.StagePath, deployConfig.DeployPath, useCli);



        }

        /// <summary>Ensures that the required framework directory structure exists.</summary>
        public static void VerifyFramework(bool useCli)
        {
            Lieutenant.DisplayMessage(UserMessage.DeploymentFramework("verifying"), useCli);

            if (!Directory.Exists("./AppData/"))
            {
                Lieutenant.DisplayMessage(UserMessage.DeploymentFramework("creating"), useCli);
                Directory.CreateDirectory("./AppData/"); // Might need just this.
                Lieutenant.DisplayMessage(UserMessage.DeploymentFramework("created"), useCli);
            }

            Lieutenant.DisplayMessage(UserMessage.DeploymentFramework("verified"), useCli);
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
        public static void VerifyConfiguration(string configPath, bool useCli)
        {
            Lieutenant.DisplayMessage(UserMessage.DeploymentConfiguration("verifying"), useCli);

            if (string.IsNullOrEmpty(configPath))
            {
                Lieutenant.DisplayMessage(UserMessage.DeploymentFramework("null-or-empty"), useCli);
                Environment.Exit(1);
            }
            else if (File.Exists(configPath))
            {
                Lieutenant.DisplayMessage(UserMessage.DeploymentConfiguration("found"), useCli);         
            }
            else
            {
                Lieutenant.DisplayMessage(UserMessage.DeploymentConfiguration("creating"), useCli);
                CreateDefaultConfigFile(configPath, useCli);
                Lieutenant.DisplayMessage(UserMessage.DeploymentConfiguration("created", configPath), useCli);
                Environment.Exit(1);
            }
        }

        /// <summary>Writes a default configuration file.</summary>
        public static void CreateDefaultConfigFile(string configPath, bool useCli)
        {
            Lieutenant.DisplayMessage(UserMessage.DeploymentConfiguration("building", configPath), useCli);
            Deploy defaultConfig = BuildDefaultConfig(configPath, useCli);

            var deployJson = JsonSerializer.Serialize(defaultConfig, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            Lieutenant.DisplayMessage(UserMessage.DeploymentConfiguration("writing", configPath), useCli);
            File.WriteAllText(configPath, deployJson);
        }

        /// <summary>Creates a default <see cref="Deploy"/> instance with preconfigured paths and repository settings.</summary>
        /// <remarks>
        ///     <para>
        ///         These are the default configuration values for a standard implementation of the Tingen Web Service.<br/>
        ///     </para>
        /// </remarks>
        /// <returns>A <see cref="Deploy"/> object initialized with default values for the repository path.</returns>
        public static Deploy BuildDefaultConfig(string configPath, bool useCli)
        {
            Lieutenant.DisplayMessage(UserMessage.DeploymentConfiguration("building"), useCli);

            return new Deploy
            {
                Source = "https://github.com/spectrum-health-systems/Tingen-WebService/archive/refs/heads/development.zip",
                StagePath       = @"C:\Tingen_Data\WebService\staging",
                DeployPath      = @"C:\Tingen\UAT"
            };
        }

        public static Deploy LoadConfigFile(string configPath, bool useCli)
        {
            Lieutenant.DisplayMessage(UserMessage.DeploymentConfiguration("loading"), useCli);
            var deployConfig = File.ReadAllText(configPath);

            return JsonSerializer.Deserialize<Deploy>(deployConfig);
        }

        public static string GetSourceType(string source, bool useCli)
        {
            if (source.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
            {
                Lieutenant.DisplayMessage(UserMessage.SourceType("Source is a URL..."), useCli);
                return "url";
            }
            else
            {
                Lieutenant.DisplayMessage(UserMessage.SourceType("Source is a directory path..."), useCli);
                return "path";
            }
        }

        /// <summary>Determines the status of the <see cref="Source"/>.</summary>
        /// <remarks>
        ///     <para>
        ///         The <paramref name="sourcePathOrUrl"/> can be a URL or a local directory path.<br/>
        ///         <br/>
        ///         If the RepostoryPath is invalid, the deployment process will not proceed.
        ///     </para>
        /// </remarks>
        /// <param name="sourcePathOrUrl">The repository path.</param>
        /// <returns>
        ///     The status of the repository path:
        ///     <list type="bullet">
        ///         <item>null-or-empty: The passed <paramref name="sourcePathOrUrl"/> is null/empty</item>
        ///         <item>invalid-url: The <paramref name="sourcePathOrUrl"/> is a URL, and is not formatted correctly</item>
        ///         <item>valid-url: The <paramref name="sourcePathOrUrl"/> is a URL, and is formatted correctly</item>   
        ///         <item>does-not-exist: The <paramref name="sourcePathOrUrl"/> is a local/shared drive, and does not exist</item>
        ///         <item>exists: The <paramref name="sourcePathOrUrl"/> is a local/shared drive, and does exist</item>
        ///     </list>
        /// </returns>
        public static void VerifySourceUrl(string sourceUrl, bool useCli)
        {
            Lieutenant.DisplayMessage(UserMessage.SourceUrl("verifying"), useCli);

            if (string.IsNullOrEmpty(sourceUrl))
            {
                Lieutenant.DisplayMessage(UserMessage.SourceUrl("null-or-empty"), useCli);
                Environment.Exit(1);
            }
            else
            {
                if (Uri.IsWellFormedUriString(sourceUrl, UriKind.Absolute))
                {
                    Lieutenant.DisplayMessage(UserMessage.SourceUrl("review-url"), useCli);
                }
                else
                {
                    Lieutenant.DisplayMessage(UserMessage.SourceUrl("invalid-url"), useCli);
                    Environment.Exit(1);
                }
            }
        }

        /// <summary>Determines the status of the <see cref="Source"/>.</summary>
        /// <remarks>
        ///     <para>
        ///         The <paramref name="sourcePathOrUrl"/> can be a URL or a local directory path.<br/>
        ///         <br/>
        ///         If the RepostoryPath is invalid, the deployment process will not proceed.
        ///     </para>
        /// </remarks>
        /// <param name="sourcePathOrUrl">The repository path.</param>
        /// <returns>
        ///     The status of the repository path:
        ///     <list type="bullet">
        ///         <item>null-or-empty: The passed <paramref name="sourcePathOrUrl"/> is null/empty</item>
        ///         <item>invalid-url: The <paramref name="sourcePathOrUrl"/> is a URL, and is not formatted correctly</item>
        ///         <item>valid-url: The <paramref name="sourcePathOrUrl"/> is a URL, and is formatted correctly</item>   
        ///         <item>does-not-exist: The <paramref name="sourcePathOrUrl"/> is a local/shared drive, and does not exist</item>
        ///         <item>exists: The <paramref name="sourcePathOrUrl"/> is a local/shared drive, and does exist</item>
        ///     </list>
        /// </returns>
        public static void VerifySourcePath(string sourcePath, bool useCli)
        {
            Lieutenant.DisplayMessage(UserMessage.SourcePath("verifying"), useCli);

            if (string.IsNullOrEmpty(sourcePath))
            {
                Lieutenant.DisplayMessage(UserMessage.SourcePath("null-or-empty"), useCli);
                Environment.Exit(1);
            }
            else
            {
                if (Directory.Exists(sourcePath))
                {
                    Lieutenant.DisplayMessage(UserMessage.SourcePath("found"), useCli);
                }
                else
                {
                    Lieutenant.DisplayMessage(UserMessage.SourcePath("not-found"), useCli);
                    Environment.Exit(1);
                }
            }
        }


        /// <summary>Determines the status of the <see cref="StagePath"/>.</summary>
        /// <remarks>
        ///     <para>
        ///         If the <paramref name="stagePath"/> is invalid, the deployment process will not proceed.
        ///     </para>
        /// </remarks>
        /// <param name="stagePath">The staging path.</param>
        /// <returns>
        ///     The status of the staging path:
        ///     <list type="bullet">
        ///         <item>null-or-empty: The passed <paramref name="stagePath"/> is null/empty</item>
        ///         <item>does-not-exist: The <paramref name="stagePath"/> does not exist</item>
        ///         <item>exists: The <paramref name="stagePath"/> does exist</item>
        ///     </list>
        /// </returns>
        public static void VerifyStagePath(string stagePath, bool useCli)
        {
            Lieutenant.DisplayMessage(UserMessage.StagePath("verifying"), useCli);

            if (string.IsNullOrEmpty(stagePath))
            {
                Lieutenant.DisplayMessage(UserMessage.StagePath("null-or-empty"), useCli);
                Environment.Exit(1);
            }
            else if (Directory.Exists(stagePath))
            {

                Lieutenant.DisplayMessage(UserMessage.StagePath("found"), useCli);

            }
            else
            {
                Lieutenant.DisplayMessage(UserMessage.StagePath("not-found"), useCli);
                Environment.Exit(1);
            }
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
        public static void VerifyDeployPath(string deployPath, bool useCli)
        {
            Lieutenant.DisplayMessage(UserMessage.DeployPath("verifying"), useCli);

            if (string.IsNullOrEmpty(deployPath))
            {
                Lieutenant.DisplayMessage(UserMessage.DeployPath("null-or-empty"), useCli);
                Environment.Exit(1);
            }
            else if (Directory.Exists(deployPath))
            {

                Lieutenant.DisplayMessage(UserMessage.DeployPath("found"), useCli);

            }
            else
            {
                Lieutenant.DisplayMessage(UserMessage.DeployPath("not-found"), useCli);
                Environment.Exit(1);
            }
        }


        /// <summary>Cleans and recreates the staging directory.</summary>
        /// <param name="stagePath">The root directory of the development deployment.</param>
        public static void RefreshStagePath(string stagePath, bool useCli)
        {
            Lieutenant.DisplayMessage(UserMessage.StagePath("refresh"), useCli);

            Directory.Delete($@"{stagePath}", true);
            Directory.CreateDirectory($@"{stagePath}");

            Lieutenant.DisplayMessage(UserMessage.StagePath("refreshed"), useCli);
        }

        /// <summary>Cleans and prepares the specified target.</summary>
        /// <param name="deployPath">The root directory to clean and initialize. Must be a valid directory path.</param>
        public static void RefreshDeployPath(string deployPath, bool useCli)
        {
            Lieutenant.DisplayMessage(UserMessage.DeployPath("refresh"), useCli);

            Directory.Delete($@"{deployPath}", true);
            Directory.CreateDirectory($@"{deployPath}");
            Directory.CreateDirectory($@"{deployPath}\bin");
            Directory.CreateDirectory($@"{deployPath}\bin\roslyn");
            Directory.CreateDirectory($@"{deployPath}\bin\AppData");
            Directory.CreateDirectory($@"{deployPath}\bin\AppData\Runtime");

            Lieutenant.DisplayMessage(UserMessage.DeployPath("refresh"), useCli);
        }

        /// <summary>Downloads and extracts a remote repository.</summary>
        /// <param name="repositoryPath">The URL of the remote repository archive to download.</param>
        /// <param name="stagingPath">The root directory where the repository archive will be staged.</param>
        public static void DownloadSourceUrl(string sourceUrl, string stagePath, bool useCli)
        {
            var client = new WebClient();

            Lieutenant.DisplayMessage(UserMessage.SourceUrl("download"), useCli);
            client.DownloadFile(sourceUrl, $@"{stagePath}\tingen-web-service.zip");
            Lieutenant.DisplayMessage(UserMessage.SourceUrl("downloaded"), useCli);

            Lieutenant.DisplayMessage(UserMessage.SourceUrl("extract"), useCli);
            ZipFile.ExtractToDirectory($@"{stagePath}\tingen-web-service.zip", $@"{stagePath}\");
            Lieutenant.DisplayMessage(UserMessage.SourceUrl("extracted"), useCli);
        }

        private static void DeployService(string stagePath, string deployPath, bool useCli)
        {
            // TODO - This should be cleaned up.

            Lieutenant.DisplayMessage(UserMessage.DeploymentProcess("deploying"), useCli);

            Lieutenant.DisplayMessage(UserMessage.DeploymentProcess("binpath", stagePath, deployPath), useCli);
            CopyDirectory($@"{stagePath}\bin\", $@"{deployPath}\bin\");

            Lieutenant.DisplayMessage(UserMessage.DeploymentProcess("appdatapath", stagePath, deployPath), useCli);
            CopyDirectory($@"{stagePath}\bin\AppData", $@"{deployPath}\bin\AppData");

            Lieutenant.DisplayMessage(UserMessage.DeploymentProcess("runtimepath", stagePath, deployPath), useCli);
            CopyDirectory($@"{stagePath}\bin\AppData\Runtime", $@"{deployPath}\bin\AppData\Runtime");

            Lieutenant.DisplayMessage(UserMessage.DeploymentProcess("roslyn", stagePath, deployPath), useCli);
            CopyDirectory($@"{stagePath}\bin\roslyn", $@"{deployPath}\bin\roslyn");

            Lieutenant.DisplayMessage(UserMessage.DeploymentProcess("servicepath", stagePath, deployPath), useCli);
            CopyDirectory($@"{stagePath}", $@"{deployPath}");
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