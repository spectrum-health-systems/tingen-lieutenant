/* ████████╗██╗███╗   ██╗ ██████╗ ███████╗███╗   ██╗
 * ╚══██╔══╝██║████╗  ██║██╔════╝ ██╔════╝████╗  ██║
 *    ██║   ██║██╔██╗ ██║██║  ███╗█████╗  ██╔██╗ ██║
 *    ██║   ██║██║╚██╗██║██║   ██║██╔══╝  ██║╚██╗██║
 *    ██║   ██║██║ ╚████║╚██████╔╝███████╗██║ ╚████║
 *    ╚═╝   ╚═╝╚═╝  ╚═══╝ ╚═════╝ ╚══════╝╚═╝  ╚═══╝
 *
 * ██╗     ██╗███████╗██╗   ██╗████████╗███████╗███╗   ██╗ █████╗ ███╗   ██╗████████╗
 * ██║     ██║██╔════╝██║   ██║╚══██╔══╝██╔════╝████╗  ██║██╔══██╗████╗  ██║╚══██╔══╝
 * ██║     ██║█████╗  ██║   ██║   ██║   █████╗  ██╔██╗ ██║███████║██╔██╗ ██║   ██║
 * ██║     ██║██╔══╝  ██║   ██║   ██║   ██╔══╝  ██║╚██╗██║██╔══██║██║╚██╗██║   ██║
 * ███████╗██║███████╗╚██████╔╝   ██║   ███████╗██║ ╚████║██║  ██║██║ ╚████║   ██║
 * ╚══════╝╚═╝╚══════╝ ╚═════╝    ╚═╝   ╚══════╝╚═╝  ╚═══╝╚═╝  ╚═╝╚═╝  ╚═══╝   ╚═╝
 *                                             A class library for Tingen projects
 *                                                                          v2.1.0
 *
 * https://github.com/spectrum-health-systems/tingen-lieutenant
 * Copyright (c) A Pretty Cool Program. All rights reserved.
 * Licensed under the Apache 2.0 license.
 */

// u250529_code
// u250529_documentation

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