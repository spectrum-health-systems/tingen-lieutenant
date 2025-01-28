// u250128_code
// u250128_documentation

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
            if (!File.Exists($@"{serviceDataRoot}\README.md"))
            {
                var msg = Catalog.Msg_ServerNotFound();
                MessageBox.Show(msg);
                Environment.Exit(1);
            }
        }
        internal static void ServiceDetailsFile(string serviceDataRoot)
        {
            if (!File.Exists($@"{serviceDataRoot}\Lieutenant\LIVE.json"))
            {
                var msg = Catalog.Msg_ServiceDetailNotFound();
                MessageBox.Show(msg);
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
