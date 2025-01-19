using System;
using static SDL2.SDL;

namespace Engine
{
    public class Window : IDisposable
    {
        private IntPtr _sdlWindow;

        public Window()
        {
            _sdlWindow = SDL_CreateWindow(
                Config.GAME_NAME,
                SDL_WINDOWPOS_UNDEFINED,
                SDL_WINDOWPOS_UNDEFINED,
                Config.WINDOW_WIDTH,
                Config.WINDOW_HEIGHT,
                0
            );

            if (_sdlWindow == IntPtr.Zero)
            {
                throw new Exception($"SDL_CreateWindow failed: {SDL_GetError()}");
            }
        }

        public void Render()
        {
            nint surface = GetSurface();
            if (surface == IntPtr.Zero)
            {
                throw new Exception($"SDL_GetWindowSurface failed: {SDL_GetError()}");
            }

            nint format = SDL_AllocFormat(SDL_PIXELFORMAT_RGB888);
            uint color = SDL_MapRGB(
                format,
                Config.BACKGROUND_COLOR.r,
                Config.BACKGROUND_COLOR.g,
                Config.BACKGROUND_COLOR.b
            );
            SDL_FreeFormat(format);

            _ = SDL_FillRect(surface, IntPtr.Zero, color);
        }

        public void Update()
        {
            _ = SDL_UpdateWindowSurface(_sdlWindow);
        }

        public IntPtr GetSurface()
        {
            return SDL_GetWindowSurface(_sdlWindow);
        }

        public void Dispose()
        {
            if (_sdlWindow != IntPtr.Zero)
            {
                SDL_DestroyWindow(_sdlWindow);
                _sdlWindow = IntPtr.Zero;
            }
            GC.SuppressFinalize(this);
        }

        // Prevent copying
        private Window(Window other) => throw new NotSupportedException();
        // private Window() => throw new NotSupportedException();

        ~Window()
        {
            Dispose();
        }
    }
}
