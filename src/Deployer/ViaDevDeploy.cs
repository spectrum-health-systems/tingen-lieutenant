/* ████████ ██ ███    ██  ██████  ███████ ███    ██
 *    ██    ██ ██ ██  ██ ██   ███ █████   ██ ██  ██
 *    ██    ██ ██   ████  ██████  ███████ ██   ████
 *
 * ██      ██ ███████ ██    ██ ████████ ███████ ███    ██  █████  ███    ██ ████████
 * ██      ██ █████   ██    ██    ██    █████   ██ ██  ██ ███████ ██ ██  ██    ██
 * ███████ ██ ███████  ██████     ██    ███████ ██   ████ ██   ██ ██   ████    ██
 *                                      TingenLieutenant.Deployer.ViaDevDeploy.cs
 */

// u250521_code
// u250521_documentation

using System.IO.Compression;
using System.Net;

namespace TingenLieutenant.Deployer
{
    /// <summary>Deploys via Tingen-DevDeploy.</summary>
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

            VerifyRepoLocationStatus(deployConfig.RepositoryPath);
            VerifyDevDeployRoot(deployConfig.DevDeployRoot);
            VerifyTargetRoot(deployConfig.TargetRoot);

            PrepStaging(deployConfig.DevDeployRoot);
            PrepTarget(deployConfig.TargetRoot);

            GetRemoteRepostitory(deployConfig.RepositoryPath, deployConfig.DevDeployRoot);

            DeployService(deployConfig.DevDeployRoot, deployConfig.TargetRoot);
        }

        /// <summary>Start the deployment process.</summary>
        private static void StartDeployment()
        {
            var msgStartDevDeploy = $"Tingen DevDeploy v2.0{Environment.NewLine}" +
                                    $"====================={Environment.NewLine}";

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
            Console.WriteLine($"Verifying config file at \"{configPath}\".");

            switch (Deploy.GetConfigFileStatus(configPath))
            {
                case "null-or-empty":
                    Console.WriteLine("ERROR: Config path cannot be null or empty.");
                    Environment.Exit(1);
                    break;

                case "does-not-exist":
                    Console.WriteLine("Config file not found.\nCreating default config file.");
                    Deploy.CreateConfigFile(configPath);
                    Environment.Exit(1);
                    break;

                case "exists":
                    Console.WriteLine("Config file found.");
                    break;

                default:
                    Console.WriteLine("ERROR: Unknown error occurred while verifying config file.");
                    Environment.Exit(1);
                    break;
            }
        }

        /// <summary>Load the configuration.</summary>
        /// <param name="configPath">The configuration file path.</param>
        /// <returns>The configuration settings.</returns>
        private static Deploy LoadConfigFile(string configPath)
        {
            Console.WriteLine("Loading config file.");

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
        /// <param name="repoPath">The path or URL of the repository location to verify.</param>
        private static void VerifyRepoLocationStatus(string repoPath)
        {
            Console.WriteLine($"Verifying the repository path \"{repoPath}\".");

            switch (Deploy.GetRepoPathStatus(repoPath))
            {
                case "null-or-empty":
                    Console.WriteLine("ERROR: Repository path cannot be null or empty.");
                    Environment.Exit(1);
                    break;

                case "invalid-url":
                    Console.WriteLine("ERROR: Repository path is not a valid URL.");
                    Environment.Exit(1);
                    break;

                case "does-not-exist":
                    Console.WriteLine($@"ERROR: Repository path {repoPath} does not exist");
                    Environment.Exit(1);
                    break;

                case "valid-path":
                    Console.WriteLine("Repository path is valid.");
                    break;

                case "valid-url":
                    Console.WriteLine("Repository path seems valid, but please verify.");
                    break;

                default:
                    Console.WriteLine("ERROR: Unknown error occurred while verifying repository path.");
                    Environment.Exit(1);
                    break;
            }
        }

        /// <summary>Verifies the status of the DevDeploy root.</summary>
        /// <param name="devDeployRoot">The path to the DevDeploy root.</param>
        private static void VerifyDevDeployRoot(string devDeployRoot)
        {
            Console.WriteLine($"Verifying the DevDeploy root \"{devDeployRoot}\".");

            switch (Deploy.GetDevDeployRootStatus(devDeployRoot))
            {
                case "null-or-empty":
                    Console.WriteLine("ERROR: DevDeploy root cannot be null or empty.");
                    Environment.Exit(1);
                    break;

                case "does-not-exist":
                    Console.WriteLine($"ERROR: DevDeploy root \"{devDeployRoot}\" does not exist");
                    Environment.Exit(1);
                    break;

                case "exists":
                    Console.WriteLine("DevDeploy root is valid.");
                    break;

                default:
                    Console.WriteLine("ERROR: Unknown error occurred while verifying DevDeploy root.");
                    Environment.Exit(1);
                    break;
            }
        }

        /// <summary>Verifies the status of the target root.</summary>
        /// <param name="targetRoot">The path to the target root.</param>
        private static void VerifyTargetRoot(string targetRoot)
        {
            Console.WriteLine($"Verifying the target root \"{targetRoot}\".");

            switch (Deploy.GetTargetRootStatus(targetRoot))
            {
                case "null-or-empty":
                    Console.WriteLine("ERROR: Target root cannot be null or empty.");
                    Environment.Exit(1);
                    break;

                case "does-not-exist":
                    Console.WriteLine($"ERROR: Target root \"{targetRoot}\" does not exist");
                    Environment.Exit(1);
                    break;

                case "exists":
                    Console.WriteLine("Target root is valid.");
                    break;

                default:
                    Console.WriteLine("ERROR: Unknown error occurred while verifying target root.");
                    Environment.Exit(1);
                    break;
            }
        }

        /// <summary>Prepares the staging area.</summary>
        /// <param name="devDeployRoot">The root directory where the staging area will be created.</param>
        private static void PrepStaging(string devDeployRoot)
        {
            Console.WriteLine($"Preparing staging area at \"{devDeployRoot}\".");
            Deploy.CleanStaging(devDeployRoot);
        }

        /// <summary>Prepares the specified target directory for deployment.</summary>
        /// <param name="targetRoot">The root directory to prepare.</param>
        private static void PrepTarget(string targetRoot)
        {
            Console.WriteLine($"Preparing target at \"{targetRoot}\".");
            Deploy.CleanTarget(targetRoot);
        }

        private static void GetRemoteRepostitory(string repoPath, string devDeployRoot) // TODO Move this to Deploy.cs
        {
            if (repoPath.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Downloading and extracting remote repository.");
                Deploy.GetRemoteRepostitory(repoPath, devDeployRoot);
            }
        }

        private static void DeployService(string devDeployRoot, string targetRoot)
        {


            Deploy.CopyDirectory($@"{devDeployRoot}\staging\tingen-web-service-development\src\bin", $@"{targetRoot}\bin");

            foreach (string serviceFile in Deploy.ListOfServiceFiles())
            {
                File.Copy($@"{devDeployRoot}\staging\tingen-web-service-development\src\{serviceFile}", $@"{targetRoot}\{serviceFile}");
            }
        }
    }
}