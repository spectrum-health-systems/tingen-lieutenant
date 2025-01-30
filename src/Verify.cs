// u250128_code
// u250128_documentation
// u250130_xmldocumentation

using System;
using System.IO;
using System.Windows;
using System.Windows.Shapes;

namespace TingenLieutenant
{
    static class Verify
    {
        internal static void ServerAccess(string serviceDataRoot)
        {

            //if (!File.Exists($@"{serviceDataRoot}\README.md"))
            //{
            //    var msg = Catalog.Msg_ServerNotFound();
            //    MessageBox.Show(msg);
            //    Environment.Exit(1);
            //}

            //if (!File.Exists($@"{serviceDataRoot}\README.md"))
            //{
            //    var msg = Catalog.Msg_ServerNotFound();
            //    MessageBox.Show(msg);
            //    Environment.Exit(1);
            //}
        }
        internal static void TingenConfigurationFile(string serviceDataRoot)
        {
            if (!File.Exists($@"{serviceDataRoot}\LIVE\Config\Tingen.config"))
            {
                Dictionary<string, string> msg = Catalog.UserMessages.MsgTingenConfigNotFound();
                MessageBox.Show(msg["Message"], msg["Title"]);
                //MessageBox.Show(msg);
                Environment.Exit(1);
            }
        }

        internal static void SessionRoot(string SessionDataRoot)
        {
            if (!Directory.Exists(SessionDataRoot))
            {
                Directory.CreateDirectory(SessionDataRoot);
            }
        }
    }
}
