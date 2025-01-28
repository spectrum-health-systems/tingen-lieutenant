// u250128_code
// u250128_documentation

using System.IO;

using TingenLieutenant.Du;

namespace TingenLieutenant
{
    class ServiceDetails
    {
        public string ServiceVersion { get; set; }
        public string ServiceBuild { get; set; }
        public string ServiceUpdated { get; set; }
        public string ServiceMode { get; set; }
        public string TraceLogLevel { get; set; }
        public string TraceLogDelay { get; set; }

        internal static ServiceDetails Load(string serviceDataRoot)
        {
            Verify.ServiceDetailsFile(serviceDataRoot);

            return DuJson.ImportFromLocalFile<ServiceDetails>($@"{serviceDataRoot}\Lieutenant\LIVE.json");
        }

        // This really shouldn't be used.
        internal static void Create(string environmentFilePath)
        {
            var defaultEnvironment = Build();

            DuJson.ExportToLocalFile<ServiceDetails>(defaultEnvironment, environmentFilePath);
        }

        // This really shouldn't be used.
        internal static ServiceDetails Build()
        {
            return new ServiceDetails
            {
                ServiceVersion = "YY.MM.x.y",
                ServiceBuild   = "YYMMDD.HHMM",
                ServiceUpdated = "MM/DD/YYYY HH:MM:SS AM",
                ServiceMode    = "not-set",
                TraceLogLevel  = "0",
                TraceLogDelay  = "0"
            };
        }
    }
}
