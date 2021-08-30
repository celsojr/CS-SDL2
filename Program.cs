using System;
using static SDL2.SDL;
using static SDL2.SDL_image;
using static SDL2.SDL.SDL_EventType;

namespace cslogo
{
    class Program
    {
        static void Main()
        {
            Util.CheckResult(SDL_Init(SDL_INIT_VIDEO));

            var window = SDL_CreateWindow("C# bindings for SDL2",
                SDL_WINDOWPOS_UNDEFINED,
                SDL_WINDOWPOS_UNDEFINED,
                512,
                512,
                0);

            Util.CheckResult(window);

            var renderer = SDL_CreateRenderer(window, -1, SDL_RendererFlags.SDL_RENDERER_ACCELERATED);
            var logoTexture = IMG_LoadTexture(renderer, "Assets/cslogo.jpg");

            Util.CheckResult(SDL_RenderClear(renderer));
            Util.CheckResult(SDL_RenderCopy(renderer, logoTexture, IntPtr.Zero, IntPtr.Zero));

            SDL_RenderPresent(renderer);
                
            var quit = false;
            while (!quit && SDL_WaitEvent(out SDL_Event e) != 0)
            {
                switch (e.type)
                {
                    case SDL_QUIT:
                        quit = true;
                        break;
                    
                    case SDL_KEYDOWN:
                        if (e.key.keysym.sym == SDL_Keycode.SDLK_ESCAPE)
                        {
                            quit = true;
                        }
                        break;
                }
            }

            SDL_DestroyTexture(logoTexture);
            SDL_DestroyRenderer(renderer);
            SDL_DestroyWindow(window);
            SDL_Quit();
        }
    }
}
