// u241217.1451_code
// u241217_documentation

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TingenLieutenant
{
    public class SettingsFile
    {
        public string ServiceSystemCode { get; set; }
        public string ServiceVersion { get; set; }
        public string ServiceUpdated { get; set; }
        public string ServiceMode { get; set; }

        public string TraceLogLevel { get; set; }
        public string TraceLogDelay { get; set; }


        public static SettingsFile Import(string filePath)
        {
            string[] rawData = null;

            SettingsFile settings = new();

            if (File.Exists(filePath))
            {
               rawData = File.ReadAllLines(filePath);
            }

            foreach (var item in rawData)
            {
                if (item.StartsWith("> Service version:"))
                {
                    settings.ServiceVersion = item.Replace("> Service version:", "").Trim();
                }
                else if (item.StartsWith("> Service updated:"))
                {
                    settings.ServiceUpdated = item.Replace("> Service updated:", "").Trim();
                }
                else if (item.StartsWith("Service mode:"))
                {
                    settings.ServiceMode = item.Replace("Service mode:", "").Trim();
                }
                else if (item.StartsWith("Service System Code:"))
                {
                    settings.ServiceSystemCode = item.Replace("Service System Code:", "").Trim();
                }
                else if (item.StartsWith("Trace log level:"))
                {
                    settings.TraceLogLevel = item.Replace("Trace log level:", "").Trim();
                }
                else if (item.StartsWith("Trace log delay:"))
                {
                    settings.TraceLogDelay = item.Replace("Trace log delay:", "").Trim();
                }


            }

            return settings;
        }

        private static string CurrentTingenSettings(string filePath)
        {
            var settingsInfo = File.ReadAllLines(filePath);




            return "";
        }

    }
}
