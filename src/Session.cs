// u250128_code
// u250128_documentation

using TingenLieutenant.Catalog;
using TingenLieutenant.Configuration;

namespace TingenLieutenant
{
    /// <summary>All resources and data for a Tingen Lieutenant session.</summary>
    public class Session
    {
        /// <summary>The root directory for the session data.</summary>
        public string CurrentSessionDataRoot { get; set; }

        /// <summary>The root directory for the service data.</summary>
        public string TngnDataRoot { get; set; }

        /// <summary>The configuration for the Tingen Lieutenant.</summary>
        public TingenConfig TngnConfig { get; set; }

        internal static Session Create()
        {
            FileNames commonFileName = FileNames.GetCommon();
            FilePaths commonFilePath = FilePaths.GetCommon();

            LieutenantConfig ltntConfig = LieutenantConfig.Load($@"{commonFilePath.LtntDataRoot}/{commonFileName.LtntConfig}");

            return new Session
            {
                CurrentSessionDataRoot = $@"{commonFilePath.LtntSesssionRoot}\{DateTime.Now.ToString("yyMMdd.hhss")}",
                TngnDataRoot           = $@"\\{ltntConfig.ServerUnc}\{ltntConfig.ServiceDataRoot}",
                TngnConfig             = new ()
            };
        }
    }
}