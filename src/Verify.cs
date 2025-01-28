// u250128_code
// u250128_documentation

using System.IO;
using System.Windows;

namespace TingenLieutenant
{
    static class Verify
    {
        internal static void ServerAccess(string path)
        {
            if (!File.Exists(path))
            {
                var msg = Catalog.MsgTngnServerNotFound(path);
                MessageBox.Show(msg);
                Environment.Exit(1);
            }
        }
    }
}
