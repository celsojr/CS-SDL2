using System;
using SDL2;
using Minesweeper;

using static SDL2.SDL;
using static SDL2.SDL_ttf;
using static SDL2.SDL_image;
using static SDL2.SDL_image.IMG_InitFlags;
using static SDL2.SDL.SDL_EventType;

namespace SdlEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            if (SDL_Init(SDL_INIT_VIDEO) < 0)
            {
                Utils.CheckSDLError("SDL_Init");
                return;
            }

            if (IMG_Init(IMG_INIT_PNG) == 0)
            {
                Utils.CheckSDLError("IMG_Init");
                return;
            }

            if (TTF_Init() == -1)
            {
                Utils.CheckSDLError("TTF_Init");
                return;
            }

            var gameWindow = new Engine.Window();
            var ui = new MinesweeperUI();

            SDL_Event e;
            bool shouldQuit = false;

            while (!shouldQuit)
            {
                while (SDL_PollEvent(out e) != 0)
                {
                    if (e.type == SDL_QUIT)
                    {
                        shouldQuit = true;
                    }
                    else
                    {
                        ui.HandleEvent(e);
                    }
                }

                gameWindow.Render();
                ui.Render(gameWindow.GetSurface());
                gameWindow.Update();
            }

            IMG_Quit();
            TTF_Quit();
            SDL_Quit();
        }
    }
}
