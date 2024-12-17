// u241217.1143_code
// u241217_documentation

using System.Net;

namespace TingenLieutenant.Du
{
    public class DuInternet
    {
        //public void DownloadRepositoryFiles(string sessionData)
        //{
        //    DownloadInternetFile("https://raw.githubusercontent.com/spectrum-health-systems/Tingen-Development/refs/heads/main/src/Tingen_development.asmx.cs", $@"{sessionData}\Tingen_development_main.asmx");
        //    DownloadInternetFile("https://raw.githubusercontent.com/spectrum-health-systems/Tingen-Development/refs/heads/development/src/Tingen_development.asmx.cs", $@"{sessionData}\Tingen_development.asmx");
        //}

        public static void DownloadFileFromURL(string toDownload, string toSave)
        {
            var asmxUrl = toDownload;
            var asmxDownloadPath = toSave;

            var client = new WebClient();
            client.DownloadFile(asmxUrl, asmxDownloadPath);
        }
    }
}
