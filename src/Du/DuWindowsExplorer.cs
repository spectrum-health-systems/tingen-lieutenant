// u250130_code
// u250130_documentation

using System.Diagnostics;

namespace TingenLieutenant.Du
{
    /// <summary>TBD</summary>
    class DuWindowsExplorer
    {
        /// <summary>Open a folder in Windows Explorer.</summary>
        /// <param name="folder"></param>
        public static void OpenFolder(string folder)
        {
            ProcessStartInfo _processStartInfo = new ProcessStartInfo
            {
                FileName = folder,
                UseShellExecute = true
            };
            Process.Start(_processStartInfo);
        }
    }
}
