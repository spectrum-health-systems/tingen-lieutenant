// u250128_code
// u250128_documentation

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TingenLieutenant.Du
{
    class DuInternet
    {
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
