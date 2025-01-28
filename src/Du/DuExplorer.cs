using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TingenLieutenant.Du
{
    class DuExplorer
    {
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
