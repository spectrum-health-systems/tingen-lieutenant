/* TingenLieutenant.WebService.Deploy.cs
 * u250625_code
 * u250625_documentation
 */

using System.IO.Compression;
using System.Net;
using System.Text.Json;

using TingenLieutenant.Blueprint;

namespace TingenLieutenant.DevDeploy
{
    /// <summary>Logic for deploying the Tingen Web Service.</summary>
    /// <remarks>
    ///     This class is intended to deploy the <i>development branch</i> of the Tingen<br/>
    ///     Web Service to the server that hosts the Tingen Web Service.<br/>
    ///     <br/>
    ///     You can modify the configuration file to deploy any branch to any location,<br/>
    ///     but performance and security are not guaranteed.
    /// </remarks>
    public class DeployWsvc
    {
        /// <summary>The location where the existing Tingen Web Service is archived.</summary>
        /// <remarks>
        ///     The ArchivePath:<br/>
        ///     <list type="bullet">
        ///         <item>Can be a local directory</item>
        ///         <item>Can be a network share/mapped drive</item>
        ///     </list>
        /// </remarks>
        /// <value>The default value is "<c>C:\Tingen_Data\DevDeploy\Archive</c>"</value>
        public string ArchivePath { get; set; }

        /// <summary>The location of the Tingen Web Service that will be deployed.</summary>
        /// <remarks>
        ///     This isn't necessarily a "path", since it can be either a directory or a URL.<br/>
        ///     <br/>
        ///     If the RepositoryPath is a URL:
        ///     <list type="bullet">
        ///         <item>It must point to a ".zip" file</item>
        ///         <item>It must formatted correctly</item>
        ///     </list>
        ///     If the RepositoryPath is a directory:
        ///     <list type="bullet">
        ///         <item>It can be a local directory</item>
        ///         <item>It can be a network share/mapped drive</item>
        ///     </list>
        /// </remarks>
        /// <value>The default value is "<c>https://github.com/spectrum-health-systems/Tingen-WebService/archive/refs/heads/development.zip</c>"</value>
        public string Source { get; set; }

        /// <summary>The type of source.</summary>
        /// <remarks>
        ///     The source type can be either:<br/>
        ///     <list type="bullet">
        ///         <item><c>path</c> - A local directory</item>
        ///         <item><c>url</c> - A URL</item>
        ///     </list>
        /// </remarks>
        /// <value>Either "<c>path</c>" or "<c>url</c>"</value>
        public string SourceType { get; set; }

        /// <summary>The location where the Tingen Web Service is staged for deployment.</summary>
        /// <remarks>
        ///     The StagingPath:<br/>
        ///     <list type="bullet">
        ///         <item>Can be a local directory</item>
        ///         <item>Can be a network share/mapped drive</item>
        ///     </list>
        /// </remarks>
        /// <value>The default value is "<c>C:\Tingen_Data\DevDeploy\Stage</c>"</value>
        public string StagePath { get; set; }

        /// <summary>The location where the Tingen Web Service is deployed.</summary>
        /// <remarks>
        ///     The DeployPath:<br/>
        ///     <list type="bullet">
        ///         <item>Can be a local directory</item>
        ///         <item>Can be a network share/mapped drive</item>
        ///     </list>
        /// </remarks>
        /// <value>The default value is "<c>C:\Tingen\UAT</c>"</value>
        public string DeployPath { get; set; }

        /// <summary>Handles the deployment process from start to finish.</summary>
        /// <param name="configPath">The path to the configuration file.</param>
        /// <param name="useCli">Determines if the message will be displayed on the CLI or the GUI.</param>
        public static void DeploymentProcess(string configPath, string arg, bool useCli)
        {
            Lieutenant.DisplayMessage(UserMessage.DeploymentProcess("start"), useCli);

            VerifyFramework(useCli);

            VerifyConfiguration(configPath, useCli);

            DeployWsvc deployConfig = LoadConfigFile(configPath, useCli);

            if (arg == "-a")
            {
                VerifyArchivePath(deployConfig.ArchivePath, useCli);
                ArchiveExistingService(deployConfig.ArchivePath, deployConfig.DeployPath, useCli);
            }

            VerifyStagePath(deployConfig.StagePath, useCli);
            RefreshStagePath(deployConfig.StagePath, useCli);

            VerifyDeployPath(deployConfig.DeployPath, useCli);
            RefreshDeployPath(deployConfig.DeployPath, useCli);

            deployConfig.SourceType = GetSourceType(deployConfig.Source, useCli);

            switch (deployConfig.SourceType)
            {
                case "url":
                    VerifySourceUrl(deployConfig.Source, useCli);
                    DownloadSourceUrl(deployConfig.Source, deployConfig.StagePath, useCli);
                    ExtractSourceUrl(deployConfig.StagePath, useCli);
                    deployConfig.StagePath = $@"{deployConfig.StagePath}\tingen-web-service-development\src";
                    break;

                default:
                    VerifySourcePath(deployConfig.Source, useCli);
                    deployConfig.StagePath = $@"{deployConfig.Source}\src";
                    break;
            }

            DeployService(deployConfig.StagePath, deployConfig.DeployPath, useCli);

            Lieutenant.DisplayMessage(UserMessage.DeploymentProcess("complete"), useCli);

            ExitDeployment(useCli);
        }

        /// <summary> Verifies the deployment framework directory, and creates it if it does not exist.</summary>
        /// <remarks>Currently, this method only checks for the existence of the <c>./AppData/</c> directory.</remarks>
        /// <param name="useCli">Determines if the message will be displayed on the CLI or the GUI.</param>
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
        /// <remarks>If the <paramref name="configPath"/> is invalid, the deployment process will not proceed.</remarks>
        /// <param name="configPath">The path to the configuration file.</param>
        /// <param name="useCli">Determines if the message will be displayed on the CLI or the GUI.</param>
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
                Lieutenant.DisplayMessage(UserMessage.DeploymentConfiguration("null-or-empty"), useCli);
                ExitDeployment(useCli);
            }

            if (File.Exists(configPath))
            {
                Lieutenant.DisplayMessage(UserMessage.DeploymentConfiguration("found"), useCli);
            }
            else
            {
                CreateDefaultConfigFile(configPath, useCli);
                Lieutenant.DisplayMessage(UserMessage.DeploymentProcess("restart"), useCli);
                ExitDeployment(useCli);
            }
        }

        /// <summary> Creates a default deployment configuration file at the specified path.</summary>
        /// <param name="configPath">The path to the configuration file.</param>
        /// <param name="useCli">Determines if the message will be displayed on the CLI or the GUI.</param>
        public static void CreateDefaultConfigFile(string configPath, bool useCli)
        {
            Lieutenant.DisplayMessage(UserMessage.DeploymentConfiguration("creating"), useCli);
            DeployWsvc defaultConfig = BuildDefaultConfig(configPath, useCli);
            Lieutenant.DisplayMessage(UserMessage.DeploymentConfiguration("created", configPath), useCli);

            var deployJson = JsonSerializer.Serialize(defaultConfig, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            Lieutenant.DisplayMessage(UserMessage.DeploymentConfiguration("writing", configPath), useCli);
            File.WriteAllText(configPath, deployJson);
        }

        /// <summary>Builds a default deployment configuration using the specified configuration path.</summary>
        /// <param name="configPath">The path to the configuration file.</param>
        /// <param name="useCli">Determines if the message will be displayed on the CLI or the GUI.</param>
        /// <returns>A <see cref="DeployWsvc"/> object containing the default deployment settings.</returns>
        public static DeployWsvc BuildDefaultConfig(string configPath, bool useCli)
        {
            Lieutenant.DisplayMessage(UserMessage.DeploymentConfiguration("building", configPath), useCli);

            return new DeployWsvc
            {
                Source      = "https://github.com/spectrum-health-systems/Tingen-WebService/archive/refs/heads/development.zip",
                SourceType  = "",
                StagePath   = @"C:\Tingen_Data\DevDeploy\Stage",
                DeployPath  = @"C:\Tingen\UAT",
                ArchivePath = @"C:\Tingen_Data\DevDeploy\Archive"
            };
        }

        /// <summary>Loads a deployment configuration file and deserializes its contents into a <see cref="DeployWsvc"/> object.</summary>
        /// <param name="configPath">The path to the configuration file.</param>
        /// <param name="useCli">Determines if the message will be displayed on the CLI or the GUI.</param>
        /// <returns>A <see cref="DeployWsvc"/> object representing the deserialized contents of the configuration file.</returns>
        public static DeployWsvc LoadConfigFile(string configPath, bool useCli)
        {
            Lieutenant.DisplayMessage(UserMessage.DeploymentConfiguration("loading"), useCli);
            var deployConfig = File.ReadAllText(configPath);
            Lieutenant.DisplayMessage(UserMessage.DeploymentConfiguration("loaded"), useCli);

            return JsonSerializer.Deserialize<DeployWsvc>(deployConfig);
        }

        /// <summary>Determines the status of the <see cref="ArchivePath"/>.</summary>
        /// <remarks>If the <paramref name="archivePath"/> is invalid, the deployment process will not proceed.</remarks>
        /// <param name="archivePath">The location where the existing Tingen Web Service is archived prior to deployment.</param>
        /// <param name="useCli">Determines if the message will be displayed on the CLI or the GUI.</param>
        /// <returns>
        ///     The status of the archive path:
        ///     <list type="bullet">
        ///         <item>null-or-empty: The passed <paramref name="archivePath"/> is null/empty</item>
        ///         <item>does-not-exist: The <paramref name="archivePath"/> does not exist</item>
        ///         <item>exists: The <paramref name="archivePath"/> does exist</item>
        ///     </list>
        /// </returns>
        public static void VerifyArchivePath(string archivePath, bool useCli)
        {
            Lieutenant.DisplayMessage(UserMessage.ArchivePath("verifying"), useCli);

            if (string.IsNullOrEmpty(archivePath))
            {
                Lieutenant.DisplayMessage(UserMessage.ArchivePath("null-or-empty"), useCli);
                ExitDeployment(useCli);
            }

            if (Directory.Exists(archivePath))
            {
                Lieutenant.DisplayMessage(UserMessage.ArchivePath("found"), useCli);
            }
            else
            {
                Lieutenant.DisplayMessage(UserMessage.ArchivePath("not-found"), useCli);
                ExitDeployment(useCli);
            }
        }

        /// <summary>Archives the existing service before deploying.</summary>
        /// <param name="archivePath">The location where the existing Tingen Web Service is archived.</param>
        /// <param name="deployPath">The location where the Tingen Web Service is deployed.</param>
        /// <param name="useCli">Determines if the message will be displayed on the CLI or the GUI.</param>
        public static void ArchiveExistingService(string archivePath, string deployPath, bool useCli)
        {
            Lieutenant.DisplayMessage(UserMessage.DeploymentProcess("archiving"), useCli);

            var dateTime = DateTime.Now.ToString("yyMMdd-HHmmss");
            ZipFile.CreateFromDirectory(deployPath, $@"{archivePath}\{dateTime}.zip");

            Lieutenant.DisplayMessage(UserMessage.DeploymentProcess("archived"), useCli);
        }

        /// <summary>Determines the type of the given source based on its format.</summary>
        /// <param name="source">The location of the Tingen Web Service that will be deployed.</param>
        /// <param name="useCli">Determines if the message will be displayed on the CLI or the GUI.</param>
        /// <returns>A string representing the type of the source.</returns>
        public static string GetSourceType(string source, bool useCli)
        {
            if (source.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
            {
                Lieutenant.DisplayMessage(UserMessage.SourceType("url"), useCli);
                return "url";
            }
            else
            {
                Lieutenant.DisplayMessage(UserMessage.SourceType("path"), useCli);
                return "path";
            }
        }

        /// <summary>Determines the status of the <see cref="sourceUrl"/>.</summary>
        /// <remarks>If the sourceUrl is invalid, the deployment process will not proceed.</remarks>
        /// <param name="sourceUrl">The URL of the Tingen Web Service that will be deployed.</param>
        /// <param name="useCli">Determines if the message will be displayed on the CLI or the GUI.</param>
        /// <returns>
        ///     The status of the repository path:
        ///     <list type="bullet">
        ///         <item>null-or-empty: The passed <paramref name="sourceUrl"/> is null/empty</item>
        ///         <item>invalid-url: The <paramref name="sourceUrl"/> is a URL, and is not formatted correctly</item>
        ///         <item>valid-url: The <paramref name="sourceUrl"/> is a URL, and is formatted correctly</item>
        ///     </list>
        /// </returns>
        public static void VerifySourceUrl(string sourceUrl, bool useCli)
        {
            Lieutenant.DisplayMessage(UserMessage.SourceUrl("verifying"), useCli);

            if (string.IsNullOrEmpty(sourceUrl))
            {
                Lieutenant.DisplayMessage(UserMessage.SourceUrl("null-or-empty"), useCli);
                ExitDeployment(useCli);
            }

            if (Uri.IsWellFormedUriString(sourceUrl, UriKind.Absolute))
            {
                Lieutenant.DisplayMessage(UserMessage.SourceUrl("review-url"), useCli);
            }
            else
            {
                Lieutenant.DisplayMessage(UserMessage.SourceUrl("invalid-url"), useCli);
                ExitDeployment(useCli);
            }
        }

        /// <summary>Determines the status of the <see cref="sourcePath"/>.</summary>
        /// <remarks>If the sourcePath is invalid, the deployment process will not proceed.</remarks>
        /// <param name="sourcePath">The path of the Tingen Web Service that will be deployed.</param>
        /// <param name="useCli">Determines if the message will be displayed on the CLI or the GUI.</param>
        /// <returns>
        ///     The status of the repository path:
        ///     <list type="bullet">
        ///         <item>null-or-empty: The passed <paramref name="sourcePath"/> is null/empty</item>
        ///         <item>does-not-exist: The <paramref name="sourcePath"/> is a local/shared drive, and does not exist</item>
        ///         <item>exists: The <paramref name="sourcePath"/> is a local/shared drive, and does exist</item>
        ///     </list>
        /// </returns>
        public static void VerifySourcePath(string sourcePath, bool useCli)
        {
            Lieutenant.DisplayMessage(UserMessage.SourcePath("verifying"), useCli);

            if (string.IsNullOrEmpty(sourcePath))
            {
                Lieutenant.DisplayMessage(UserMessage.SourcePath("null-or-empty"), useCli);
                ExitDeployment(useCli);
            }

            if (Directory.Exists(sourcePath))
            {
                Lieutenant.DisplayMessage(UserMessage.SourcePath("found"), useCli);
            }
            else
            {
                Lieutenant.DisplayMessage(UserMessage.SourcePath("not-found"), useCli);
                ExitDeployment(useCli);
            }
        }

        /// <summary>Determines the status of the <see cref="StagePath"/>.</summary>
        /// <remarks>If the <paramref name="StagePath"/> is invalid, the deployment process will not proceed.</remarks>
        /// <param name="StagePath">The location where the Tingen Web Service is staged for deployment.</param>
        /// <param name="useCli">Determines if the message will be displayed on the CLI or the GUI.</param>
        /// <returns>
        ///     The status of the staging path:
        ///     <list type="bullet">
        ///         <item>null-or-empty: The passed <paramref name="StagePath"/> is null/empty</item>
        ///         <item>does-not-exist: The <paramref name="StagePath"/> does not exist</item>
        ///         <item>exists: The <paramref name="StagePath"/> does exist</item>
        ///     </list>
        /// </returns>
        public static void VerifyStagePath(string stagePath, bool useCli)
        {
            Lieutenant.DisplayMessage(UserMessage.StagePath("verifying"), useCli);

            if (string.IsNullOrEmpty(stagePath))
            {
                Lieutenant.DisplayMessage(UserMessage.StagePath("null-or-empty"), useCli);
                ExitDeployment(useCli);
            }

            if (Directory.Exists(stagePath))
            {
                Lieutenant.DisplayMessage(UserMessage.StagePath("found"), useCli);
            }
            else
            {
                Lieutenant.DisplayMessage(UserMessage.StagePath("not-found"), useCli);
                ExitDeployment(useCli);
            }
        }

        /// <summary>Determines the status of the <see cref="DeployPath"/>.</summary>
        /// <remarks>
        ///     If the <paramref name="deployPath"/> is invalid, the deployment process will not proceed.
        /// </remarks>
        /// <param name="deployPath">The location where the Tingen Web Service is deployed.</param>
        /// <param name="useCli">Determines if the message will be displayed on the CLI or the GUI.</param>
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
                ExitDeployment(useCli);
            }

            if (Directory.Exists(deployPath))
            {
                Lieutenant.DisplayMessage(UserMessage.DeployPath("found"), useCli);
            }
            else
            {
                Lieutenant.DisplayMessage(UserMessage.DeployPath("not-found"), useCli);
                ExitDeployment(useCli);
            }
        }

        /// <summary>Cleans and recreates the staging directory.</summary>
        /// <param name="stagePath">The location where the Tingen Web Service is staged for deployment.</param>
        /// <param name="useCli">Determines if the message will be displayed on the CLI or the GUI.</param>
        public static void RefreshStagePath(string stagePath, bool useCli)
        {
            Lieutenant.DisplayMessage(UserMessage.StagePath("refresh"), useCli);

            Directory.Delete($@"{stagePath}", true);
            Directory.CreateDirectory($@"{stagePath}");

            Lieutenant.DisplayMessage(UserMessage.StagePath("refreshed"), useCli);
        }

        /// <summary>Cleans and prepares the specified target.</summary>
        /// <param name="deployPath">The location where the Tingen Web Service is deployed.</param>
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
        /// <param name="sourceUrl">The URL of the Tingen Web Service that will be deployed.</param>
        /// <param name="stagePath">The location where the Tingen Web Service is staged for deployment.</param>
        /// <param name="useCli">Determines if the message will be displayed on the CLI or the GUI.</param>
        public static void DownloadSourceUrl(string sourceUrl, string stagePath, bool useCli)
        {
            var client = new WebClient();

            Lieutenant.DisplayMessage(UserMessage.SourceUrl("download"), useCli);
            client.DownloadFile(sourceUrl, $@"{stagePath}\tingen-web-service.zip");
            Lieutenant.DisplayMessage(UserMessage.SourceUrl("downloaded"), useCli);
        }

        public static void ExtractSourceUrl(string stagePath, bool useCli)
        {
            Lieutenant.DisplayMessage(UserMessage.SourceUrl("extract"), useCli);
            ZipFile.ExtractToDirectory($@"{stagePath}\tingen-web-service.zip", $@"{stagePath}\");
            Lieutenant.DisplayMessage(UserMessage.SourceUrl("extracted"), useCli);
        }

        /// <summary>Deploys a service by copying files from the staging path to the deployment path.</summary>
        /// <param name="stagePath">The location where the Tingen Web Service is staged for deployment.</param>
        /// <param name="deployPath">The location where the Tingen Web Service is deployed.</param>
        /// <param name="useCli">Determines if the message will be displayed on the CLI or the GUI.</param>
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
        /// <param name="stagePath">The location where the Tingen Web Service is staged for deployment.</param>
        /// <param name="deployPath">The location where the Tingen Web Service is deployed.</param>
        public static void CopyDirectory(string stagePath, string deployPath)
        {
            DirectoryInfo srcDir = new DirectoryInfo(stagePath);

            foreach (FileInfo file in srcDir.GetFiles())
            {
                file.CopyTo(Path.Combine(deployPath, file.Name), true);
            }
        }

        /// <summary>Terminates the deployment process and exits the application.</summary>
        /// <param name="useCli">Determines if the message will be displayed on the CLI or the GUI.</param>
        public static void ExitDeployment(bool useCli)
        {
            Lieutenant.DisplayMessage(UserMessage.DeploymentProcess("exit"), useCli);
            Environment.Exit(0);
        }
    }
}