/* ████████ ██ ███    ██  ██████  ███████ ███    ██
 *    ██    ██ ██ ██  ██ ██   ███ █████   ██ ██  ██
 *    ██    ██ ██   ████  ██████  ███████ ██   ████
 *
 * ██      ██ ███████ ██    ██ ████████ ███████ ███    ██  █████  ███    ██ ████████
 * ██      ██ █████   ██    ██    ██    █████   ██ ██  ██ ███████ ██ ██  ██    ██
 * ███████ ██ ███████  ██████     ██    ███████ ██   ████ ██   ██ ██   ████    ██
 *                                      TingenLieutenant.Deployer.ViaDevDeploy.cs
 */

// u250520_code
// u250520_documentation

using System.IO.Compression;
using System.Net;

namespace TingenLieutenant.Deployer
{
    /// <summary>Deploys via Tingen-DevDeploy.</summary>
    public class ViaDevDeploy
    {

        public static void DeployEnvironment(string configPath)
        {
            StartDeployment();

            VerifyConfigFileStatus(configPath);

            var deployConfig = LoadConfigFile(configPath);

            VerifyRepoLocationStatus(deployConfig.RepositoryPath);

            VerifyDevDeployRoot(deployConfig.DevDeployRoot);

            VerifyTargetRoot(deployConfig.TargetRoot);

            PrepStaging(deployConfig.DevDeployRoot);

            GetRemoteRepostitory(deployConfig.RepositoryPath, deployConfig.DevDeployRoot);

            PrepTarget(deployConfig.TargetRoot);
        }

        /// <summary>Start the deployment process.</summary>
        private static void StartDeployment()
        {
            var msgStartDevDeploy = $"Tingen DevDeploy v2.0{Environment.NewLine}" +
                                    $"====================={Environment.NewLine}";

            Console.WriteLine(msgStartDevDeploy);
        }

        /// <summary>Verifies the status of the configuration file.</summary>
        /// <param name="configPath">The file path of the configuration file.</param>
        private static void VerifyConfigFileStatus(string configPath)
        {
            Console.WriteLine($"Verifying config file at \"{configPath}\".");

            if (Deploy.GetConfigFileStatus(configPath) == "null-or-empty")
            {
                Console.WriteLine("ERROR: Config path cannot be null or empty.");

                Environment.Exit(1);
            }
            else if (Deploy.GetConfigFileStatus(configPath) == "does-not-exist")
            {
                Console.WriteLine("Config file not found.\nCreating default config file.");

                Deploy.CreateConfigFile(configPath);

                Environment.Exit(1);
            }
            else if (Deploy.GetConfigFileStatus(configPath) == "exists")
            {
                Console.WriteLine("Config file found.");
            }
            else
            {
                Console.WriteLine("ERROR: Unknown error occurred while verifying config file.");

                Environment.Exit(1);
            }
        }

        /// <summary>Load the configuration.</summary>
        /// <param name="configPath">The file path of the configuration file.</param>
        /// <returns>The configuration settings.</returns>
        private static Deploy LoadConfigFile(string configPath)
        {
            Console.WriteLine("Loading config file.");

            return Deploy.LoadConfigFile(configPath);
        }

        /// <summary>Verifies the status of the repository location.</summary>
        /// <param name="repoPath">The path or URL of the repository location to verify.</param>
        private static void VerifyRepoLocationStatus(string repoPath)
        {
            Console.WriteLine($"Verifying the repository path \"{repoPath}\".");

            string repoPathStatus = Deploy.GetRepoPathStatus(repoPath);

            if (repoPathStatus == "null-or-empty")
            {
                Console.WriteLine("ERROR: Repository path cannot be null or empty.");

                Environment.Exit(1);
            }
            else if (repoPathStatus == "invalid-url")
            {
                Console.WriteLine("ERROR: Repository path is not a valid URL.");

                Environment.Exit(1);
            }
            else if (repoPathStatus == "does-not-exist")
            {
                Console.WriteLine($@"ERROR: Repository path {repoPath} does not exist");

                Environment.Exit(1);
            }
            else if (repoPathStatus == "valid-path")
            {
                Console.WriteLine("Repository path is valid.");
            }
            else if (repoPathStatus == "valid-url")
            {
                Console.WriteLine("Repository path seems valid, but please verify.");
            }
            else
            {
                Console.WriteLine("ERROR: Unknown error occurred while verifying repository path.");

                Environment.Exit(1);
            }
        }

        private static void VerifyDevDeployRoot(string devDeployRoot)
        {
            Console.WriteLine($"Verifying the DevDeploy root \"{devDeployRoot}\".");

            string devDeployRootStatus = Deploy.GetDevDeployRootStatus(devDeployRoot);

            if (devDeployRootStatus == "null-or-empty")
            {
                Console.WriteLine("ERROR: DevDeploy root cannot be null or empty.");

                Environment.Exit(1);
            }
            else if (devDeployRootStatus == "does-not-exist")
            {
                Console.WriteLine($"ERROR: DevDeploy root \"{devDeployRoot}\" does not exist");

                Environment.Exit(1);
            }
            else if (devDeployRootStatus == "exists")
            {
                Console.WriteLine("DevDeploy root is valid.");
            }
            else
            {
                Console.WriteLine("ERROR: Unknown error occurred while verifying DevDeploy root.");

                Environment.Exit(1);
            }
        }

        private static void VerifyTargetRoot(string targetRoot)
        {
            Console.WriteLine($"Verifying the target root \"{targetRoot}\".");

            string targetRootStatus = Deploy.GetTargetRootStatus(targetRoot);

            if (targetRootStatus == "null-or-empty")
            {
                Console.WriteLine("ERROR: Target root cannot be null or empty.");

                Environment.Exit(1);
            }
            else if (targetRootStatus == "does-not-exist")
            {
                Console.WriteLine($"ERROR: Target root \"{targetRoot}\" does not exist");

                Environment.Exit(1);
            }
            else if (targetRootStatus == "exists")
            {
                Console.WriteLine("Target root is valid.");
            }
            else
            {
                Console.WriteLine("ERROR: Unknown error occurred while verifying target root.");

                Environment.Exit(1);
            }
        }

        private static void PrepStaging(string devDeployRoot)
        {
            Console.WriteLine($"Preparing staging area at \"{devDeployRoot}\".");

            if (Directory.Exists($@"{devDeployRoot}\staging"))
            {
                Directory.Delete($@"{devDeployRoot}\staging", true);
            }

            Directory.CreateDirectory($@"{devDeployRoot}\staging");
        }

        private static void GetRemoteRepostitory(string repoPath, string devDeployRoot) // TODO Move this to Deploy.cs
        {
            Console.WriteLine($"Preparing repository.");

            if (repoPath.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Downloading repository from URL.");
                var client = new WebClient();
                client.DownloadFile(repoPath, $@"{devDeployRoot}\staging\webservice.zip");

                Console.WriteLine("Extracting.");

                ZipFile.ExtractToDirectory($@"{devDeployRoot}\staging\webservice.zip", $@"{devDeployRoot}\staging\");
            }
        }

        private static void PrepTarget(string targetRoot)
        {
            Console.WriteLine($"Preparing target at \"{targetRoot}\".");

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

        private static void DeployService(string devDeployRoot, string targetRoot)
        {
            var serviceFiles = new List<string>
            {
                "TingenWebService.asmx",
                "TingenWebService.asmx.cs",
                "packages.config",
                "Web.config",
                "Web.Debug.config",
                "Web.Release.config"
            };

            Deploy.CopyDirectory($@"{devDeployRoot}\staging\tingen-web-service-development\src\bin", $@"{targetRoot}\bin");

            foreach (string serviceFile in serviceFiles)
            {
                File.Copy($@"{devDeployRoot}\staging\tingen-web-service-development\src\{serviceFile}", $@"{targetRoot}\{serviceFile}");
            }


        }





    }

}