using System;
using static SDL2.SDL;

namespace SdlEngine
{
    static class Utils
    {
        internal static void CheckResult(object result)
        {
            if (result == default)
            {
                throw new InvalidOperationException(nameof(result),
                    new Exception(SDL_GetError()));
            }
        }

        internal static void CheckResult(int result)
        {
            if (result != default)
            {
                throw new InvalidOperationException(nameof(result),
                    new Exception(SDL_GetError()));
            }
        }

        internal static void CheckSDLError(string context)
        {
            string error = SDL_GetError();
            if (!string.IsNullOrEmpty(error))
            {
                Console.WriteLine($"{context}: {error}");
            }
        }

    }
}
