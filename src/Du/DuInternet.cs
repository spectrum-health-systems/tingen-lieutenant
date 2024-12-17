// u241217.1143_code
// u241217_documentation

using System.Net;

namespace TingenLieutenant.Du
{
    public class DuInternet
    {
        public static void DownloadFileFromURL(string downloadUrl, string filePath)
        {
            var client = new WebClient();
            client.DownloadFile(downloadUrl, filePath);
        }
    }
}
