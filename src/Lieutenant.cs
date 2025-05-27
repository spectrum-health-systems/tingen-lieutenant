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

// u250521_code
// u250521_documentation

namespace TingenLieutenant
{
    public static class Lieutenant
    {
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