// u250128_code
// u250128_documentation

namespace TingenLieutenant
{
    class Catalog
    {
        internal static string Msg_ServerNotFound()
        {
            return $"Tingen Lieutenant cannot connect to the Tingen web service server.{Environment.NewLine}" +
                   Environment.NewLine +
                   $"Please see the Tingen Lieutenant documentation for more information.";
        }

        internal static string Msg_ServiceDetailNotFound()
        {
            return $"Tingen Lieutenant cannot find the Tingen web service details file.{Environment.NewLine}" +
                   Environment.NewLine +
                   $"Please see the Tingen Lieutenant documentation for more information.";
        }
    }
}