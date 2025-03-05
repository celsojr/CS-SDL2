using System;
using System.IO;
using static SDL2.SDL;

namespace SdlEngine
{
    static class Utils
    {
        internal static void CheckSDLError(string context)
        {
            string error = SDL_GetError();
            if (!string.IsNullOrEmpty(error))
            {
                Console.WriteLine($"{context}: {error}");
            }
        }

        internal static string Assets(string filename)
        {
            return Path.Combine(AppContext.BaseDirectory, "Assets", filename);
        }
    }
}
