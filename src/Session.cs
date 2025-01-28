// u250128_code
// u250128_documentation

namespace TingenLieutenant
{
    class Session
    {
        public string LtntDataRoot { get; set; }
        public string TngnServiceDataRoot { get; set; }

        public string LtntVerificationFile { get; set; }

        internal static Session Create()
        {
            const string ltntDataRoot = @".\AppData";

            Configuration ltntConfig = Configuration.Load($@"{ltntDataRoot}\TngnLtnt.settings");

            return new Session()
            {
                LtntDataRoot         = ltntDataRoot,
                TngnServiceDataRoot  = $@"\\{ltntConfig.TngnServerUnc}\{ltntConfig.TngnServiceData}",
                LtntVerificationFile = @"\Lieutenant\tingen.lieutenant"
            };
        }
    }
}
