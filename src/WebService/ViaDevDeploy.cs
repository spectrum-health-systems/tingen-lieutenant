/* ████████ ██ ███    ██  ██████  ███████ ███    ██
 *    ██    ██ ██ ██  ██ ██   ███ █████   ██ ██  ██
 *    ██    ██ ██   ████  ██████  ███████ ██   ████
 *
 * ██      ██ ███████ ██    ██ ████████ ███████ ███    ██  █████  ███    ██ ████████
 * ██      ██ █████   ██    ██    ██    █████   ██ ██  ██ ███████ ██ ██  ██    ██
 * ███████ ██ ███████  ██████     ██    ███████ ██   ████ ██   ██ ██   ████    ██
 *                                    TingenLieutenant.WebService.ViaDevDeploy.cs
 */

// u250521_code
// u250521_documentation

using TingenLieutenant.WebService;

namespace TingenLieutenant.WebServiceDeployer
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
        public static void DeployEnvironment(string configPath)
        {
            StartDeployment();
            VerifyConfigFileStatus(configPath);

            var deployConfig = LoadConfigFile(configPath);

            VerifyRepoPathStatus(deployConfig.RepositoryPath);
            VerifyStagingRoot(deployConfig.StagingPath);
            VerifyDeploymentRoot(deployConfig.DeployPath);

            PrepStaging(deployConfig.StagingPath);
            PrepTarget(deployConfig.DeployPath);

            bool repoIsRemote = deployConfig.RepositoryPath.EndsWith(".zip", StringComparison.OrdinalIgnoreCase);

            if (repoIsRemote)
            {
                GetRemoteRepostitory(deployConfig.RepositoryPath, deployConfig.StagingPath);
                deployConfig.StagingPath = $@"{deployConfig.StagingPath}\tingen-web-service-development\src";
            }
            else
            {
                Console.WriteLine($@"Deploying web service from [{deployConfig.RepositoryPath}] to [{deployConfig.StagingPath}]");

                Console.WriteLine($@"Copying [{deployConfig.RepositoryPath}\src\bin] to [{deployConfig.StagingPath}\bin]");
                DevelopmentDeploy.CopyDirectory($@"{deployConfig.RepositoryPath}\src\bin\", $@"{deployConfig.StagingPath}\bin\");

                Console.WriteLine($@"Copying [$@""{deployConfig.RepositoryPath}\src\bin\AppData] to [$@""{deployConfig.StagingPath}\bin\AppData]");
                DevelopmentDeploy.CopyDirectory($@"{deployConfig.RepositoryPath}\src\bin\AppData", $@"{deployConfig.StagingPath}\bin\AppData");

                Console.WriteLine($@"Copying [$@""{deployConfig.RepositoryPath}\src\bin\AppData\Runtime] to [$@""{deployConfig.StagingPath}\bin\AppData\Runtime]");
                DevelopmentDeploy.CopyDirectory($@"{deployConfig.RepositoryPath}\src\bin\AppData\Runtime", $@"{deployConfig.StagingPath}\bin\AppData\Runtime");

                Console.WriteLine($@"Copying [{deployConfig.RepositoryPath}\src\bin\roslyn] to {deployConfig.StagingPath}\bin\roslyn]");
                DevelopmentDeploy.CopyDirectory($@"{deployConfig.RepositoryPath}\src\bin\roslyn", $@"{deployConfig.StagingPath}\bin\roslyn");

                Console.WriteLine($@"Copying service files [{deployConfig.RepositoryPath}\src] to {deployConfig.StagingPath}]");
                DevelopmentDeploy.CopyDirectory($@"{deployConfig.RepositoryPath}\src", $@"{deployConfig.StagingPath}");

                //deployConfig.StagingPath = $@"{deployConfig.RepositoryPath}";
            }

            DeployService(deployConfig.StagingPath, deployConfig.DeployPath);
        }

        /// <summary>Start the deployment process.</summary>
        private static void StartDeployment()
        {
            var msgStartDevDeploy = $"====================={Environment.NewLine}" +
                                    $"Tingen DevDeploy v2.0{Environment.NewLine}" +
                                    $"=====================";

            Console.WriteLine(msgStartDevDeploy);
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
            Console.WriteLine($"Verifying configuration file [{configPath}]");

            switch (DevelopmentDeploy.GetConfigFileStatus(configPath))
            {
                case "null-or-empty":
                    Console.WriteLine("ERROR: %configPath% cannot be null or empty");
                    Environment.Exit(1);
                    break;

                case "does-not-exist":
                    Console.WriteLine("Configuration file not found.\n  Creating default configuration file");
                    DevelopmentDeploy.CreateDeploymentConfigFile(configPath);
                    Environment.Exit(1);
                    break;

                case "exists":
                    Console.WriteLine("Configuration file found");
                    break;

                default:
                    Console.WriteLine("ERROR: Unknown error occurred while verifying configuration file");
                    Environment.Exit(1);
                    break;
            }
        }

        /// <summary>Load the configuration.</summary>
        /// <param name="configPath">The configuration file path.</param>
        /// <returns>The configuration settings.</returns>
        private static DevelopmentDeploy LoadConfigFile(string configPath)
        {
            Console.WriteLine("Loading config file");

            return DevelopmentDeploy.LoadConfigFile(configPath);
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
            Console.WriteLine($"Verifying the repository path [{repositoryPath}]");

            var t = DevelopmentDeploy.GetRepositoryPathStatus(repositoryPath);

            switch (DevelopmentDeploy.GetRepositoryPathStatus(repositoryPath))
            {
                case "null-or-empty":
                    Console.WriteLine("ERROR: repositoryPath cannot be null or empty");
                    Environment.Exit(1);
                    break;

                case "invalid-url":
                    Console.WriteLine("ERROR: Repository path is not a valid URL");
                    Environment.Exit(1);
                    break;

                case "valid-url":
                    Console.WriteLine("Repository path seems valid, but please verify");
                    break;

                case "does-not-exist":
                    Console.WriteLine($"ERROR: Repository path does not exist");
                    Environment.Exit(1);
                    break;

                case "exists":
                    Console.WriteLine("Repository path is valid");
                    break;



                default:
                    Console.WriteLine($"ERROR: Unknown error occurred while verifying repository path");
                    Environment.Exit(1);
                    break;
            }
        }

        /// <summary>Verifies the status of the DevDeploy root.</summary>
        /// <param name="stagingRoot">The path to the DevDeploy root.</param>
        private static void VerifyStagingRoot(string stagingRoot)
        {
            Console.WriteLine($"Verifying the staging path [{stagingRoot}]");

            switch (DevelopmentDeploy.GetStagingPathStatus(stagingRoot))
            {
                case "null-or-empty":
                    Console.WriteLine("ERROR: stagingPath configuration value cannot be null or empty");
                    Environment.Exit(1);
                    break;

                case "does-not-exist":
                    Console.WriteLine($"ERROR: Staging path \"{stagingRoot}\" does not exist");
                    Environment.Exit(1);
                    break;

                case "exists":
                    Console.WriteLine("Staging path is valid");
                    break;

                default:
                    Console.WriteLine("ERROR: Unknown error occurred while verifying staging path");
                    Environment.Exit(1);
                    break;
            }
        }

        /// <summary>Verifies the status of the target root.</summary>
        /// <param name="deploymentRoot">The path to the target root.</param>
        private static void VerifyDeploymentRoot(string deploymentRoot)
        {
            Console.WriteLine($"Verifying the deployment path [{deploymentRoot}]");

            switch (DevelopmentDeploy.GetDeploymentRootStatus(deploymentRoot))
            {
                case "null-or-empty":
                    Console.WriteLine("ERROR: deploymentPath configuration value cannot be null or empty");
                    Environment.Exit(1);
                    break;

                case "does-not-exist":
                    Console.WriteLine($"ERROR: Deployment path \"{deploymentRoot}\" does not exist");
                    Environment.Exit(1);
                    break;

                case "exists":
                    Console.WriteLine("Deployment path is valid");
                    break;

                default:
                    Console.WriteLine("ERROR: Unknown error occurred while verifying deployment path");
                    Environment.Exit(1);
                    break;
            }
        }

        /// <summary>Prepares the staging area.</summary>
        /// <param name="stagingPath">The root directory where the staging area will be created.</param>
        private static void PrepStaging(string stagingPath)
        {
            Console.WriteLine($"Preparing staging path [{stagingPath}]");
            DevelopmentDeploy.CleanStagingRoot(stagingPath);
        }

        /// <summary>Prepares the specified target directory for deployment.</summary>
        /// <param name="deploymentPath">The root directory to prepare.</param>
        private static void PrepTarget(string deploymentPath)
        {
            Console.WriteLine($"Preparing deployment path [{deploymentPath}]");
            DevelopmentDeploy.CleanDeploymentRoot(deploymentPath);
        }

        /// <summary>Get a remote repository.</summary>
        /// <param name="repoPath">The path to the remote repository.</param>
        /// <param name="stagingRoot">The root directory where the repository will be extracted.</param>
        private static void GetRemoteRepostitory(string repoPath, string stagingRoot)
        {
            if (repoPath.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Downloading and extracting remote repository");
                DevelopmentDeploy.GetRemoteRepository(repoPath, stagingRoot);
            }
        }

        /// <summary>Deploys the service.</summary>
        /// <param name="stagingPath">The root directory of the development deployment.</param>
        /// <param name="deploymentpath">The root directory of the target deployment.</param>
        private static void DeployService(string stagingPath, string deploymentpath, bool isRemote = true)
        {
            Console.WriteLine($"1: {stagingPath} - {deploymentpath}");

            //if (!Directory.Exists($@"{deploymentpath}\bin"))
            //{
            //    Console.WriteLine($"Creating directory [{stagingPath} - {deploymentpath}.");
            //    Directory.CreateDirectory($@"{deploymentpath}\bin");
            //}

            Console.WriteLine($"2: {stagingPath} - {deploymentpath}");

            Console.WriteLine($@"Deploying web service from [{stagingPath}] to [{deploymentpath}]");

            Console.WriteLine($@"Copying [{stagingPath}\bin] to [{deploymentpath}\bin]");
            DevelopmentDeploy.CopyDirectory($@"{stagingPath}\bin\", $@"{deploymentpath}\bin\");

            Console.WriteLine($@"Copying [$@""{stagingPath}\bin\AppData] to [$@""{deploymentpath}\bin\AppData]");
            DevelopmentDeploy.CopyDirectory($@"{stagingPath}\bin\AppData", $@"{deploymentpath}\bin\AppData");

            Console.WriteLine($@"Copying [$@""{stagingPath}\bin\AppData\Runtime] to [$@""{deploymentpath}\bin\AppData\Runtime]");
            DevelopmentDeploy.CopyDirectory($@"{stagingPath}\bin\AppData\Runtime", $@"{deploymentpath}\bin\AppData\Runtime");

            Console.WriteLine($@"Copying [{stagingPath}\bin\roslyn] to {deploymentpath}\bin\roslyn]");
            DevelopmentDeploy.CopyDirectory($@"{stagingPath}\bin\roslyn", $@"{deploymentpath}\bin\roslyn");

            Console.WriteLine($@"Copying service files [{stagingPath} to {deploymentpath}]");
            DevelopmentDeploy.CopyDirectory($@"{stagingPath}", $@"{deploymentpath}");
        }
    }
}