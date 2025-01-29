// u250129_code
// u250129_documentation

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TingenLieutenant.Du;

namespace TingenLieutenant.Configuration
{
    public class TingenConfig
    {
        public string TngnVersion { get; set; } = "00.00.0.0";
        public string TngnBuild { get; set; } = "000000";
        public string TngnMode { get; set; }
        public string TngnTraceLevel { get; set; }
        public string TngnTraceDelay { get; set; }

        public static TingenConfig Load(string configPath)
        {
            if (!File.Exists(configPath))
            {
                //if (!File.Exists($@"{serviceDataRoot}\README.md"))
                //{
                //    var msg = Catalog.Msg_ServerNotFound();
                //    MessageBox.Show(msg);
                //    Environment.Exit(1);
                //}
            }

            return DuJson.ImportFromLocalFile<TingenConfig>(configPath);
        }
    }
}