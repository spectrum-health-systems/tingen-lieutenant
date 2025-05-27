/* ████████ ██ ███    ██  ██████  ███████ ███    ██
 *    ██    ██ ██ ██  ██ ██   ███ █████   ██ ██  ██
 *    ██    ██ ██   ████  ██████  ███████ ██   ████
 *
 * ██      ██ ███████ ██    ██ ████████ ███████ ███    ██  █████  ███    ██ ████████
 * ██      ██ █████   ██    ██    ██    █████   ██ ██  ██ ███████ ██ ██  ██    ██
 * ███████ ██ ███████  ██████     ██    ███████ ██   ████ ██   ██ ██   ████    ██
 *                                                  TingenLieutenant.Blueprint.cs
 */

// u250527_code
// u250527_documentation

namespace TingenLieutenant.Blueprint
{
    internal static class UserMessage
    {
        public static string DeploymentProcess(string status, string stagePath = "", string deployPath = "") =>
            status switch
            {
                "start"       => $"====================={Environment.NewLine}" +
                                 $"Tingen DevDeploy v2.0{Environment.NewLine}" +
                                 $"====================={Environment.NewLine}" +
                                 Environment.NewLine +
                                 $"Starting Tingen Web Service deployment process...{Environment.NewLine}",
                "complete"    => $"Tingen Web Service deployment process completed successfully.{Environment.NewLine}",
                "deploying"   => $"Deploying the Tingen Web Service...{Environment.NewLine}",
                "binpath"     => $"  From: \"{stagePath}\\bin\"{Environment.NewLine}" +
                                 $"    To: \"{deployPath}\\bin\"",
                "appdatapath" => $"  From: \"{stagePath}\\bin\\AppData\"{Environment.NewLine}" +
                                 $"    To: \"{deployPath}\\bin\\AppData\"",
                "runtimepath" => $"  From: \"{stagePath}\\bin\\AppData\\Runtime\"{Environment.NewLine}" +
                                 $"    To: \"{deployPath}\\bin\\AppData\\Runtime\"",
                "roslyn"      => $"  From: \"{stagePath}\\bin\\roslyn\"{Environment.NewLine}" +
                                 $"    To: \"{deployPath}\\bin\\roslyn\"",
                "servicepath" => $"  From: \"{stagePath}\"{Environment.NewLine}" +
                                 $"    To: \"{deployPath}\"",
                "fail"        => $"[ERROR] The Tingen Web Service deployment process failed ({status}).{Environment.NewLine}",
                _             => $"[ERROR] There was an unknown error during the deployment process ({status}).{Environment.NewLine}"
            };

        public static string DeploymentFramework(string status) =>
            status switch
            {
                "verifying" => $"Verifying the deployment framework...{Environment.NewLine}",
                "creating"  => $"Deployment framework not found...creating...{Environment.NewLine}",
                "created"   => $"Deployment framework created.{Environment.NewLine}",
                "verified"  => $"Deployment framework verified.{Environment.NewLine}",
                _           => $"[ERROR] There was an unknown deployment framework error ({status}).{Environment.NewLine}"
            };

        public static string DeploymentConfiguration(string status, string configPath = "") =>
            status switch
            {
                "verifying" => $"Verifying the deployment configuration...{Environment.NewLine}",
                "creating"  => $"Deployment configuration not found...creating...{Environment.NewLine}",
                "building"  => $"Building default configuration file...{Environment.NewLine} +" +
                               Environment.NewLine +
                               $"The default configuration will work with a standard{Environment.NewLine}" +
                               $"installation of the Tingen Web Service.{Environment.NewLine}" +
                               Environment.NewLine +
                               $"If you are not using a standard installation of the{Environment.NewLine}" +
                               $"Tingen Web Service, you will need to edit the{Environment.NewLine}" +
                               $"configuration file, which can be found here:{Environment.NewLine}" +
                               $"  {configPath}",
                "writing"   => $"Writing deployment configuration to local file...{Environment.NewLine}",
                "created"   => $"Default configuration file created.{Environment.NewLine}",
                "found"     => $"Deployment configuration found.{Environment.NewLine}",
                "Loading"   => $"Loading deployment configuration from local file...{Environment.NewLine}",
                _           => $"[ERROR] There was an unknown deployment configuration error ({status}).{Environment.NewLine}",
            };

        public static string SourceType(string status) =>
            status switch
            {
                "verifying"   => $"Verifying the source path/URL...{Environment.NewLine}",
                "not-found"   => $"Source path not found...{Environment.NewLine}",
                "invalid-url" => $"Source URL is invalid...{Environment.NewLine}",
                "review-url"  => $"Source URL seems valid, but please verify...{Environment.NewLine}",
                "found"      => $"Source path found.{Environment.NewLine}",
                _ => $"[ERROR] There was an error verifying the source path/URL ({status}).{Environment.NewLine}"
            };

        public static string SourcePath(string status) =>
            status switch
            {
                "verifying"   => $"Verifying the source path/URL...{Environment.NewLine}",
                "not-found"   => $"Source path not found...{Environment.NewLine}",
                "found"       => $"Source path found.{Environment.NewLine}",
                _             => $"[ERROR] There was an error verifying the source path/URL ({status}).{Environment.NewLine}"
            };

        public static string SourceUrl(string status) =>
            status switch
            {
                "verifying"   => $"Verifying the source path/URL...{Environment.NewLine}",
                "invalid-url" => $"Source URL is invalid...{Environment.NewLine}",
                "review-url"  => $"Source URL seems valid, but please verify...{Environment.NewLine}",
                "download"    => $"Downloading source from URL...{Environment.NewLine}",
                "downloaded"  => $"Download complete.{Environment.NewLine}",
                "extract"     => $"Extracting source...{Environment.NewLine}",
                "extracted"   => $"Extraction complete.{Environment.NewLine}",
                _             => $"[ERROR] There was an error verifying the source path/URL ({status}).{Environment.NewLine}"
            };

        public static string StagePath(string status) =>
            status switch
            {
                "verifying" => $"Verifying the staging path...{Environment.NewLine}",
                "not-found" => $"The staging path not found...{Environment.NewLine}",
                "found"     => $"The staging path found.{Environment.NewLine}",
                "refresh"   => $"Refreshing the staging path...{Environment.NewLine}",
                "refreshed" => $"The staging path has been refreshed...{Environment.NewLine}",
                _           => $"[ERROR] There was a unknown staging path error (\"{status}\").{Environment.NewLine}"
            };

        public static string DeployPath(string status) =>
            status switch
            {
                "verifying" => $"Verifying the deploy path...{Environment.NewLine}",
                "not-found" => $"The deploy  path not found...{Environment.NewLine}",
                "found"     => $"The deploy path found.{Environment.NewLine}",
                "refresh"   => $"Refreshing the deploy path...{Environment.NewLine}",
                "refreshed" => $"The deploy path has been refreshed...{Environment.NewLine}",
                _           => $"[ERROR] There was a unknown deploy path error ({status}).{Environment.NewLine}"
            };
    }
}