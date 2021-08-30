using System;
using static SDL2.SDL;

namespace cslogo
{
    static class Util
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
    }
}
