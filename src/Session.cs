// u250128_code
// u250128_documentation

namespace TingenLieutenant
{
    public class Session
    {
        public string SessionDataRoot { get; set; }
        public string ServiceDataRoot { get; set; }
        internal static Session Create(string serverUnc, string serviceDataRoot)
        {
            return new Session
            {
                SessionDataRoot = $@".\AppData\Session\{DateTime.Now.ToString("yyMMdd.hhss")}",
                ServiceDataRoot = $@"\\{serverUnc}\{serviceDataRoot}",
            };
        }
    }
}