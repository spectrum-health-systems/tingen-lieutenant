// u250129_code
// u250129_documentation

namespace TingenLieutenant.Catalog
{
    /// <summary>Catalog of user messages.</summary>
    class UserMessages
    {
        /// <summary>
        /// Message for when the Tingen web service server cannot be found.
        /// </summary>
        /// <returns></returns>
        internal static Dictionary<string, string> MsgServerNotFound()
        {
            var msg = $"Tingen Lieutenant cannot connect to the Tingen web service server.{Environment.NewLine}" +
                      Environment.NewLine +
                      $"Please see the Tingen Lieutenant documentation for more information.";

            return new Dictionary<string, string>()
            {
                { "Title", "Server Not Found" },
                { "Message", msg }
            };
        }

        /// <summary>
        /// Message for when the Tingen configuration file cannot be found.
        /// </summary>
        /// <returns></returns>
        internal static Dictionary<string, string> MsgTingenConfigNotFound()
        {
            var msg = $"Tingen Lieutenant cannot find the Tingen configuration file.{Environment.NewLine}" +
                      Environment.NewLine +
                      $"Please see the Tingen Lieutenant documentation for more information.";

            return new Dictionary<string, string>()
            {
                { "Title", "Server Not Found" },
                { "Message", msg }
            };
        }
    }
}