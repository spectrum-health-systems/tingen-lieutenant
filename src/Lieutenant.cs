/* TingenLieutenant.Lieutenant.cs
 * u250702_code
 * u250702_documentation
 */

namespace TingenLieutenant
{
    /// <summary>The main class for Tingen Lieutenant.</summary>
    /// <remarks>This class contains general methods for Tingen Lieutenant.</remarks>
    public static class Lieutenant
    {
        /// <summary>Displays a message to the user.</summary>
        /// <remarks>
        ///     If <paramref name="useCli"/> is <see langword="true"/>, the message is written to the console.<br/>
        ///     <br/>
        ///     If <paramref name="useCli"/> is <see langword="false"/>, the message is displayed in a graphical user interface (GUI).
        /// </remarks>
        /// <param name="message">The message to display.</param>
        /// <param name="useCli">Determines if the message will be displayed on the CLI or the GUI.</param>
        public static void DisplayMessage(string message, bool useCli)
        {
            if (useCli)
            {
                Console.WriteLine(message);
            }
            else
            {
                // Placeholder for Tingen Commander GUI output.
            }
        }
    }
}