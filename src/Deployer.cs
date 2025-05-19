/* ████████ ██ ███    ██  ██████  ███████ ███    ██
 *    ██    ██ ██ ██  ██ ██   ███ █████   ██ ██  ██
 *    ██    ██ ██   ████  ██████  ███████ ██   ████
 *
 * ██      ██ ███████ ██    ██ ████████ ███████ ███    ██  █████  ███    ██ ████████
 * ██      ██ █████   ██    ██    ██    █████   ██ ██  ██ ███████ ██ ██  ██    ██
 * ███████ ██ ███████  ██████     ██    ███████ ██   ████ ██   ██ ██   ████    ██
 *                                                   TingenLieutenant.Deployer.cs
 */

// u250519_code
// u250519_documentation

using System.Text.Json;

namespace TingenLieutenant
{
    public class Deployer
    {
        public string RepoLocation { get; set; }
        public string DevDeployRoot { get; set; }
        public string ServiceRoot { get; set; }

        public static void ResetEnvironment(string environment, bool cli)
        {
            if (cli)
            {
                Console.WriteLine($"Resetting environment for {environment}...");
            }

            var deployer = new Deployer
            {
                RepoLocation  = GetRepoLocation(environment),
                DevDeployRoot = @"C:\Tingen_Data\DevDeploy\",
                ServiceRoot   = @"C:\Tingen\UAT"
            };

            if (cli)
            {
                Console.WriteLine(MsgResetEnvironment(deployer));
            }

            WriteDevDeployConfig(deployer);

            if (cli)
            {
                Console.WriteLine("Configuration file written successfully.");
            }
        }

        public static string GetRepoLocation(string environment)
        {
            if (environment.ToLower() == "remote")
            {
                return "https://github.com/spectrum-health-systems/Tingen-WebService/archive/refs/heads/development.zip";
            }
            else if (environment.ToLower() == "local")
            {
                return @"C:\path\to\repository\";
            }
            else
            {
                return "Invalid location";
            }

        }

        public static void ValidateConfigFile(string filePath, bool cli)
        {
            if (!File.Exists(filePath))
            {
                if (cli)
                {
                    Console.WriteLine(MsgMissingConfigFile(filePath));
                    Environment.Exit(1);
                }        
            }
        }

        private static void WriteDevDeployConfig(Deployer deployer)
        {
            var deployConfig = JsonSerializer.Serialize(deployer, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            //File.Create($@"C:\Tingen_Data\WebService\{tngnWbsvEnvironment}\Configs\TngnWbsv.config").Close();

            File.WriteAllText($@"./AppData/devdeploy.conf", deployConfig);
        }

        private static string MsgResetEnvironment(Deployer deployer)
        {
            return $@"The following will be written to \AppData\devdeploy.conf:" +
                   $"Repository location: {deployer.RepoLocation}\n" +
                   $"DevDeploy Root: {deployer.DevDeployRoot}\n" +
                   $"Service Root: {deployer.ServiceRoot}";
        }

        private static string MsgMissingConfigFile(string filePath)
        {
            return $"Configuration file not found at {filePath}.{Environment.NewLine}" +
                   Environment.NewLine+
                   $"Please run the setup command:{Environment.NewLine}" +
                   $"     $ TingenDevDeploy reset <environment>";
        }
    }
}