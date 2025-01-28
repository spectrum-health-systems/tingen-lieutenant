// u241217.1132_code
// u241217_documentation

using TingenLieutenant.Du;

namespace TingenLieutenant
{
    public static class Repository
    {
        public static void DownloadAsmxFiles(string sessionRoot, string MainBranchUrl, string DevelopmentBranchUrl)
        {
            DuInternet.DownloadFileFromURL(MainBranchUrl, $@"{sessionRoot}\MainBranch.asmx");
            DuInternet.DownloadFileFromURL(DevelopmentBranchUrl, $@"{sessionRoot}\DevelopmentBranch.asmx");
        }
    }
}
