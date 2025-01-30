// u250130_code
// u250130_documentation

using System.Diagnostics;

namespace TingenLieutenant.Du
{
    /// <summary>TBD</summary>
    class DuInternet
    {
        /// <summary>Open a URL in the default browser.</summary>
        /// <param name="url"></param>
        public static void OpenUrl(string url)
        {
            ProcessStartInfo _processStartInfo = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };
            Process.Start(_processStartInfo);
        }
    }
}
