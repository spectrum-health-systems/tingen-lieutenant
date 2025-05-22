/* ████████ ██ ███    ██  ██████  ███████ ███    ██
 *    ██    ██ ██ ██  ██ ██   ███ █████   ██ ██  ██
 *    ██    ██ ██   ████  ██████  ███████ ██   ████
 *
 * ██      ██ ███████ ██    ██ ████████ ███████ ███    ██  █████  ███    ██ ████████
 * ██      ██ █████   ██    ██    ██    █████   ██ ██  ██ ███████ ██ ██  ██    ██
 * ███████ ██ ███████  ██████     ██    ███████ ██   ████ ██   ██ ██   ████    ██
 *                                    TingenLieutenant.WebService.ViaDevDeploy.cs
 */

// u250522_code
// u250522_documentation

using TingenLieutenant.WebService;

namespace TingenLieutenant.WebService
{
    /// <summary>Deploys the Tingen Web Service via Tingen-DevDeploy.</summary>
    /// <remarks>
    ///     <para>
    ///     Since Tingen-DevDeploy is a command line tool, this class is used to deploy<br/>
    ///     the Tingen Web Service and provide user information via the console.
    ///     </para>
    /// </remarks>
    public class ViaDevDeploy
    {
        /// <summary>Deploys the Tingen Web Service.</summary>
        /// <param name="configPath">The configuration file path.</param>
        public static void DeployWebService(string configPath)
        {
            Deploy.VerifyFramework();
            StartDeployment();
            VerifyConfigFileStatus(configPath);

            var deployConfig = LoadConfigFile(configPath);

            VerifyRepoPathStatus(deployConfig.RepositoryPath);

            if (deployConfig.RepositoryPath.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
            {
                VerifyStagingRoot(deployConfig.StagingPath);
                PrepStaging(deployConfig.StagingPath);
                GetRemoteRepostitory(deployConfig.RepositoryPath, deployConfig.StagingPath);
                deployConfig.StagingPath = $@"{deployConfig.StagingPath}\tingen-web-service-development\src";
            }
            else
            {
                deployConfig.StagingPath = $@"{deployConfig.RepositoryPath}\src";
                VerifyStagingRoot(deployConfig.StagingPath);
            }

            VerifyDeploymentPath(deployConfig.DeployPath);
            PrepTarget(deployConfig.DeployPath);

            DeployService(deployConfig.StagingPath, deployConfig.DeployPath);
        }

        /// <summary>Start the deployment process.</summary>
        private static void StartDeployment()
        {
            Console.WriteLine($"====================={Environment.NewLine}" +
                              $"Tingen DevDeploy v2.0{Environment.NewLine}" +
                              $"====================={Environment.NewLine}");
        }

        /// <summary>Verifies the status of the configuration file.</summary>
        /// <remarks>
        ///     <para>
        ///         If a configuration file does not exist, it will be created with the<br/>
        ///         default settings that will work with a standard installation of the<br/>
        ///         Tingen Web Service.
        ///     </para>
        /// </remarks>
        /// <param name="configPath">The configuration file path.</param>
        private static void VerifyConfigFileStatus(string configPath)
        {
            Console.WriteLine($"[ CONFIGURATION ]{Environment.NewLine}" +
                              $"  Path: \"{configPath}\"");

            switch (Deploy.ConfigFileStatus(configPath))
            {
                case "null-or-empty":
                    Console.WriteLine("Status: (Error) %configPath% is null or empty");
                    Environment.Exit(1);
                    break;

                case "does-not-exist":
                    Console.WriteLine("Status: Not found, creating default configuration.");
                    Deploy.CreateDefaultConfigFile(configPath);
                    Environment.Exit(1);
                    break;

                case "exists":
                    Console.WriteLine("Status: Valid");
                    break;

                default:
                    Console.WriteLine("Status: (Error) An unknown error occurred");
                    Environment.Exit(1);
                    break;
            }
        }

        /// <summary>Load the configuration.</summary>
        /// <param name="configPath">The configuration file path.</param>
        /// <returns>The configuration settings.</returns>
        private static Deploy LoadConfigFile(string configPath)
        {
            Console.WriteLine("Status: Loading");

            return Deploy.LoadConfigFile(configPath);
        }

        /// <summary>Verifies the status of the repository path.</summary>
        /// <remarks>
        ///     <note type="security" title="A note about URLs">
        ///         Technically only the <i>formatting</i> of a repository URL is verified,<br/>
        ///         so it is possible that the URL is valid but does not exist.<br/>
        ///         <br/>
        ///         Instead of figuring out how to verify the URL exists, we'll just display<br/>
        ///         a message to the user and let them verify it themselves. Maybe in the future<br/>
        ///         we'll take care of this programmatically.
        ///     </note>
        ///     <para>
        ///         The <see cref="Deploy.RepositoryPath"/> can either be a URL or a directory.<br/>
        ///     </para>
        /// </remarks>
        /// <param name="repositoryPath">The path or URL of the repository location to verify.</param>
        private static void VerifyRepoPathStatus(string repositoryPath)
        {
            Console.WriteLine(Environment.NewLine +
                              $"[ REPOSITORY ]{Environment.NewLine}" +
                              $"  Path: \"{repositoryPath}\"");

            switch (Deploy.RepositoryPathStatus(repositoryPath))
            {
                case "null-or-empty":
                    Console.WriteLine("Status: (Error) %repositoryPath% is null or empty");
                    Environment.Exit(1);
                    break;

                case "invalid-url":
                    Console.WriteLine($"Status: (Error) \"{repositoryPath}\" is not a valid URL");
                    Environment.Exit(1);
                    break;

                case "valid-url":
                    Console.WriteLine("Status: Repository path seems valid, but please verify");
                    break;

                case "does-not-exist":
                    Console.WriteLine($"Status: (Error) \"{repositoryPath}\" does not exist");
                    Environment.Exit(1);
                    break;

                case "exists":
                    Console.WriteLine("Status: Valid");
                    break;

                default:
                    Console.WriteLine("Status: (Error) An unknown error occurred");
                    Environment.Exit(1);
                    break;
            }
        }

        /// <summary>Verifies the status of the staging path.</summary>
        /// <param name="stagingPath">The staging path.</param>
        private static void VerifyStagingRoot(string stagingPath)
        {
            Console.WriteLine(Environment.NewLine +
                              $"[ STAGING ]{Environment.NewLine}" +
                              $"  Path: \"{stagingPath}\"");

            switch (Deploy.StagingPathStatus(stagingPath))
            {
                case "null-or-empty":
                    Console.WriteLine("Status: (Error) %stagingPath% is null or empty");
                    Environment.Exit(1);
                    break;

                case "does-not-exist":
                    Console.WriteLine($"Status: (Error) \"{stagingPath}\" does not exist");
                    Environment.Exit(1);
                    break;

                case "exists":
                    Console.WriteLine("Status: Valid");
                    break;

                default:
                    Console.WriteLine("Status: (Error) An unknown error occurred");
                    Environment.Exit(1);
                    break;
            }
        }

        /// <summary>Prepares the staging area.</summary>
        /// <param name="stagingPath">The root directory where the staging area will be created.</param>
        private static void PrepStaging(string stagingPath)
        {
            Console.WriteLine("Status: Preparing");
            Deploy.CleanStagingPath(stagingPath);
        }

        /// <summary>Verifies the status of the target root.</summary>
        /// <param name="deployPath">The path to the target root.</param>
        private static void VerifyDeploymentPath(string deployPath)
        {
            Console.WriteLine(Environment.NewLine +
                              $"[ Deployment ]{Environment.NewLine}" +
                              $"  Path: \"{deployPath}\"");

            switch (Deploy.DeployPathStatus(deployPath))
            {
                case "null-or-empty":
                    Console.WriteLine("Status: (Error) %deploymentPath% is null or empty");
                    Environment.Exit(1);
                    break;

                case "does-not-exist":
                    Console.WriteLine($"Status: (Error) \"{deployPath}\" does not exist");
                    Environment.Exit(1);
                    break;

                case "exists":
                    Console.WriteLine("Status: Valid");
                    break;

                default:
                    Console.WriteLine("Status: (Error) An unknown error occurred");
                    Environment.Exit(1);
                    break;
            }
        }

        /// <summary>Prepares the specified target directory for deployment.</summary>
        /// <param name="deployPath">The root directory to prepare.</param>
        private static void PrepTarget(string deployPath)
        {
            var msgPrepDeployment = "Status: Preparing";

            Console.WriteLine(msgPrepDeployment);
            Deploy.CleanDeployPath(deployPath);
        }

        /// <summary>Get a remote repository.</summary>
        /// <param name="repositoryPath">The path to the remote repository.</param>
        /// <param name="stagingPath">The root directory where the repository will be extracted.</param>
        private static void GetRemoteRepostitory(string repositoryPath, string stagingPath)
        {
            if (repositoryPath.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Status: Downloading and extracting remote repository...please wait...");
                Deploy.GetRemoteRepository(repositoryPath, stagingPath);
            }
        }

        /// <summary>Deploys the service.</summary>
        /// <param name="stagingPath">The root directory of the development deployment.</param>
        /// <param name="deployPath">The root directory of the target deployment.</param>
        private static void DeployService(string stagingPath, string deployPath)
        {
            // TODO - This should be cleaned up.

            var msgDeployService = Environment.NewLine +
                               $"[ Tingen Web Service ]{Environment.NewLine}" +
                               $"Status: Deploying";

            Console.WriteLine(msgDeployService);

            var msgDeployBinPath = $"  From: \"{stagingPath}\\bin\"{Environment.NewLine}" +
                                   $"    To: \"{deployPath}\\bin\"";

            Console.WriteLine(msgDeployBinPath);
            Deploy.CopyDirectory($@"{stagingPath}\bin\", $@"{deployPath}\bin\");

            var msgDeployAppDataPath = Environment.NewLine +
                                   $"  From: \"{stagingPath}\\bin\\AppData\"{Environment.NewLine}" +
                                   $"    To: \"{deployPath}\\bin\\AppData\"";
            Console.WriteLine(msgDeployAppDataPath);
            Deploy.CopyDirectory($@"{stagingPath}\bin\AppData", $@"{deployPath}\bin\AppData");

            var msgDeployAppDataRuntimePath = Environment.NewLine +
                                   $"  From: \"{stagingPath}\\bin\\AppData\\Runtime\"{Environment.NewLine}" +
                                   $"    To: \"{deployPath}\\bin\\AppData\\Runtime\"";

            Console.WriteLine(msgDeployAppDataRuntimePath);
            Deploy.CopyDirectory($@"{stagingPath}\bin\AppData\Runtime", $@"{deployPath}\bin\AppData\Runtime");

            var msgDeployRoslynPath = Environment.NewLine +
                                   $"  From: \"{stagingPath}\\bin\\roslyn\"{Environment.NewLine}" +
                                   $"    To: \"{deployPath}\\bin\\roslyn\"";
            Console.WriteLine(msgDeployRoslynPath);
            Deploy.CopyDirectory($@"{stagingPath}\bin\roslyn", $@"{deployPath}\bin\roslyn");

            var msgDeployServiceFilesPath = Environment.NewLine +
                                   $"  From: \"{stagingPath}{Environment.NewLine}" +
                                   $"    To: \"{deployPath}";
            Console.WriteLine(msgDeployServiceFilesPath);
            Deploy.CopyDirectory($@"{stagingPath}", $@"{deployPath}");
        }
    }
}