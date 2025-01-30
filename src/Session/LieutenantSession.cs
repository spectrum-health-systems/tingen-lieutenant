// u250130_code
// u250130_documentation
// u250130_xmldocumentation

using TingenLieutenant.Catalog;
using TingenLieutenant.Configuration;

namespace TingenLieutenant.Session
{
    /// <summary>All resources and data for a Tingen Lieutenant session.</summary>
    public class LieutenantSession
    {
        /// <summary>The root directory for the session data.</summary>
        public string CurrentSessionDataRoot { get; set; }

        /// <summary>The root directory for the service data.</summary>
        public string TngnDataRoot { get; set; }

        /// <summary>The configuration for the Tingen Lieutenant.</summary>
        public TingenConfiguration TngnConfig { get; set; }

        /// <summary>Create a new Tingen Lieutenant session.</summary>
        /// <returns>A Tingen Lieutenant session object.</returns>
        /// <include file='XmlDoc/TingenLieutenant.Session.LieutenantSession_doc.xml' path='TingenLieutenant.Session.LieutenantSession/Type[@name="Method"]/Create/*'/>
        internal static LieutenantSession Create()
        {
            FileNames fileName = FileNames.Initialize();
            FilePaths filePath = FilePaths.Initialize();

            LieutenantConfiguration ltntConfig = LieutenantConfiguration.Load($@"{filePath.LtntDataRoot}/{fileName.LtntConfig}");

            return new LieutenantSession
            {
                CurrentSessionDataRoot = $@"{filePath.LtntSesssionRoot}\{DateTime.Now.ToString("yyMMdd.hhss")}",
                TngnDataRoot           = $@"\\{ltntConfig.ServerUnc}\{ltntConfig.ServiceDataRoot}",
                TngnConfig             = new()
            };
        }
    }
}