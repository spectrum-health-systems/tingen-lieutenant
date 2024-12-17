// u241217.1132_code
// u241217_documentation

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using TingenLieutenant.Du;

namespace TingenLieutenant
{
    public class Repository
    {
        public static void UpdateRepositoryVersions(string sessionData)
        {
            DownloadRepositoryFiles(sessionData);
            //txbxTingenDevelopmentMainBranchVersion.Text = GetAsmxVersion($@"{sessionData}\Tingen_development_main.asmx");
            //txbxTingenRepositoryDevelopmentBranch.Text = GetAsmxVersion($@"{sessionData}\Tingen_development.asmx");
        }

        private static void DownloadRepositoryFiles(string sessionData)
        {
            DuInternet.DownloadFileFromURL("https://raw.githubusercontent.com/spectrum-health-systems/Tingen-Development/refs/heads/main/src/Tingen_development.asmx.cs", $@"{sessionData}\Tingen_development_main.asmx");
            DuInternet.DownloadFileFromURL("https://raw.githubusercontent.com/spectrum-health-systems/Tingen-Development/refs/heads/development/src/Tingen_development.asmx.cs", $@"{sessionData}\Tingen_development.asmx");
        }

        //private void DownloadInternetFile(string toDownload, string toSave)
        //{
        //    var asmxUrl = toDownload;
        //    var asmxDownloadPath = toSave;

        //    var client = new WebClient();
        //    client.DownloadFile(asmxUrl, asmxDownloadPath);
        //}
    }
}
