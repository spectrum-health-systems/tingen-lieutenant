// u250128_code
// u250128_documentation

namespace TingenLieutenant
{
    class Catalog
    {
        /// <summary>Message when Tingen Lieutenant cannot find the verification file.</summary>
        /// <param name="verificationFilePath">The location of Tingen web service data, according to the configuration.</param>
        /// <returns>A message to the user letting them know that the Tingen web service data location was not found.</returns>
        internal static string MsgTngnServerNotFound(string verificationFilePath)
        {
            return $"Tingen Lieutenant cannot connect to the Tingen web service server.{Environment.NewLine}" +
                   Environment.NewLine +
                   $"Please see the Tingen Lieutenant documentation for more information.";
        }
    }
}