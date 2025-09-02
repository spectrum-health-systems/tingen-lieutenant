/* TingenLieutenant.Blueprint.UserMessage.DevDeploy.cs
 * u250625_code
 * u250625_documentation
 */

namespace TingenLieutenant.Blueprint
{
    /// <summary>Contains user messages for the Tingen DevDeploy deployment process.</summary>
    internal static partial class UserMessage
    {
        /// <summary>Status messages related to the overall deployment process.</summary>
        /// <param name="status">The current status of the deployment process.</param>
        /// <param name="stagePath">The location where the Tingen Web Service is staged for deployment.</param>
        /// <param name="deployPath">The location where the Tingen Web Service is deployed.</param>
        /// <returns>A message to display to the user.</returns>
        public static string DeploymentProcess(string status, string stagePath = "", string deployPath = "") =>
            status switch
            {
                "start"       => $"====================={Environment.NewLine}" +
                                 $"Tingen DevDeploy v2.2{Environment.NewLine}" +
                                 $"====================={Environment.NewLine}" +
                                 Environment.NewLine +
                                 $"Starting Tingen Web Service deployment process...",
                "archiving"   => $"Archiving the existing Tingen Web Service...please wait...",
                "archived"    => $"Tingen Web Service archived successfully.",
                "complete"    => $"{Environment.NewLine}Tingen Web Service deployment process completed successfully.",
                "deploying"   => $"Deploying the Tingen Web Service...",
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
                "restart"     => $"{Environment.NewLine}Please run Tingen DevDeploy again.",
                "exit"        => $"{Environment.NewLine}Exiting Tingen DevDeploy...{Environment.NewLine}",
                "fail"        => $"[ERROR] The Tingen Web Service deployment process failed ({status}).{Environment.NewLine}",
                _             => $"[ERROR] There was an unknown error during the deployment process ({status}).{Environment.NewLine}"
            };

        /// <summary>Status messages related the deployment framework.</summary>
        /// <param name="status">The current status of the deployment framework verification process.</param>
        /// <returns>A message to display to the user.</returns>
        public static string DeploymentFramework(string status) =>
            status switch
            {
                "verifying" => $"Verifying the deployment framework...",
                "creating"  => $"Deployment framework not found...creating...",
                "created"   => $"Deployment framework created.",
                "verified"  => $"Deployment framework verified.",
                _           => $"[ERROR] There was an unknown deployment framework error ({status}).{Environment.NewLine}"
            };

        /// <summary>Status messages related the deployment configuration.</summary>
        /// <param name="status">The current status of the deployment configuration verification process.</param>
        /// <returns>A message to display to the user.</returns>
        public static string DeploymentConfiguration(string status, string configPath = "") =>
            status switch
            {
                "verifying" => $"Verifying the deployment configuration...",
                "creating"  => $"Deployment configuration not found...creating...",
                "building"  => $"Building default configuration file...{Environment.NewLine}" +
                               Environment.NewLine +
                               $"  Please note{Environment.NewLine}" +
                               $"  -----------{ Environment.NewLine}" +
                               $"  The default configuration will work with a standard{Environment.NewLine}" +
                               $"  installation of the Tingen Web Service.{Environment.NewLine}" +
                               Environment.NewLine +
                               $"  If you are not using a standard installation of the{Environment.NewLine}" +
                               $"  Tingen Web Service, you will need to edit the{Environment.NewLine}" +
                               $"  configuration file, which can be found here:{Environment.NewLine}" +
                               Environment.NewLine +
                               $"    {configPath}{Environment.NewLine}",
                "created"   => $"Default configuration file created.",
                "writing"   => $"Writing deployment configuration to local file...",
                "found"     => $"Deployment configuration found.",
                "loading"   => $"Loading deployment configuration from local file...",
                "loaded"    => $"Deployment configuration loaded.",
                _           => $"[ERROR] There was an unknown deployment configuration error ({status}).{Environment.NewLine}",
            };

        /// <summary>Status messages related the archive path.</summary>
        /// <param name="status">The current status of the archive path verification process.</param>
        /// <returns>A message to display to the user.</returns>
        public static string ArchivePath(string status) =>
            status switch
            {
                "verifying" => $"Verifying the archive path...",
                "not-found" => $"[ERROR] Archive path not found.",
                "found"     => $"Archive path found.",
                _           => $"[ERROR] There was an error verifying the archive path ({status}).{Environment.NewLine}"
            };

        /// <summary>Status messages related the type of source.</summary>
        /// <param name="status">The current status of the source type verification process.</param>
        /// <returns>A message to display to the user.</returns>
        public static string SourceType(string status) =>
            status switch
            {
                "url"  => $"Source type is: URL",
                "path" => $"Source type is: Path",
                _      => $"[ERROR] There was an error verifying the source type ({status}).{Environment.NewLine}"
            };

        /// <summary>Status messages related the source path.</summary>
        /// <param name="status">The current status of the source path verification process.</param>
        /// <returns>A message to display to the user.</returns>
        public static string SourcePath(string status) =>
            status switch
            {
                "verifying" => $"Verifying the source path/URL...",
                "not-found" => $"[ERROR] Source path not found.",
                "found"     => $"Source path found.",
                _           => $"[ERROR] There was an error verifying the source path/URL ({status}).{Environment.NewLine}"
            };

        /// <summary>Status messages related the source url.</summary>
        /// <param name="status">The current status of the source URL verification process.</param>
        /// <returns>A message to display to the user.</returns>
        public static string SourceUrl(string status) =>
            status switch
            {
                "verifying"   => $"Verifying the source path/URL...",
                "invalid-url" => $"[ERROR] Source URL is invalid.",
                "review-url"  => $"Source URL seems valid, but please verify...",
                "download"    => $"Downloading source from URL...",
                "downloaded"  => $"Download complete.",
                "extract"     => $"Extracting source...",
                "extracted"   => $"Extraction complete.",
                _             => $"[ERROR] There was an error verifying the source path/URL ({status}).{Environment.NewLine}"
            };

        /// <summary>Status messages related the staging path.</summary>
        /// <param name="status">The current status of the staging path verification process.</param>
        /// <returns>A message to display to the user.</returns>
        public static string StagePath(string status) =>
            status switch
            {
                "verifying" => $"Verifying the staging path...",
                "not-found" => $"[ERROR] Staging path not found.",
                "found"     => $"The staging path found.",
                "refresh"   => $"Refreshing the staging path...",
                "refreshed" => $"The staging path has been refreshed...",
                _           => $"[ERROR] There was a unknown staging path error (\"{status}\").{Environment.NewLine}"
            };

        /// <summary>Status messages related the deployment path.</summary>
        /// <param name="status">The current status of the deployment path verification process.</param>
        /// <returns>A message to display to the user.</returns>
        public static string DeployPath(string status) =>
            status switch
            {
                "verifying" => $"Verifying the deploy path...",
                "not-found" => $"The deploy path not found...",
                "found"     => $"The deploy path found.",
                "refresh"   => $"Refreshing the deploy path...",
                "refreshed" => $"The deploy path has been refreshed...",
                _           => $"[ERROR] There was a unknown deploy path error ({status}).{Environment.NewLine}"
            };
    }
}