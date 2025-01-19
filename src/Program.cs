using Engine;
using System;
using Minesweeper;
using System.Runtime.InteropServices;

using static SDL2.SDL;
using static SDL2.SDL_ttf;
using static SDL2.SDL_image;
using static SDL2.SDL_image.IMG_InitFlags;
using static SDL2.SDL.SDL_EventType;

namespace SdlEngine
{
    class Program
    {
        private const int ALLOW = 1;
        private const int BLOCK = 0;

        private static SDL_EventFilter _eventFilter;
 
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

            if (SDL_Init(SDL_INIT_EVENTS) < 0)
            {
                Utils.CheckSDLError("TTF_Init");
                return;
            }

            // _eventFilter = new SDL_EventFilter(FilterEvents);
            // SDL_SetEventFilter(Marshal.GetFunctionPointerForDelegate(_eventFilter), IntPtr.Zero);

            _eventFilter = new SDL_EventFilter(FilterEvents);
            IntPtr userdata = IntPtr.Zero;
            SDL_SetEventFilter(_eventFilter, userdata);

            Window gameWindow = new Window();
            MinesweeperUI ui = new MinesweeperUI();

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
                        ui.HandleEvent(in e);
                    }
                }

                gameWindow.Render();
                ui.Render(gameWindow.GetSurface());
                gameWindow.Update();
            }

            SDL_FlushEvents(SDL_FIRSTEVENT, SDL_LASTEVENT);

            IMG_Quit();
            TTF_Quit();
            SDL_Quit();
        }

        private static int FilterEvents(IntPtr userdata, IntPtr sdlevent)
        {
            SDL_Event managedEvent = Marshal.PtrToStructure<SDL_Event>(sdlevent);

            // if (managedEvent.type == UserEvents.CELL_CLEARED)
            // {
            //     managedEvent.type = UserEvents.CELL_CLEARED;
            //     return 1;
            // }

            return managedEvent.type switch
            {
                SDL_MOUSEMOTION => ALLOW,
                SDL_MOUSEBUTTONDOWN => ALLOW,
                SDL_QUIT => ALLOW,

                UserEvents.CELL_CLEARED => ALLOW,
                UserEvents.BOMB_PLACED => ALLOW,
                UserEvents.FLAG_PLACED => ALLOW,
                UserEvents.FLAG_CLEARED => ALLOW,
                UserEvents.GAME_LOST => ALLOW,
                UserEvents.GAME_WON => ALLOW,
                UserEvents.NEW_GAME => ALLOW,

                _ => BLOCK
            };
        }
    }
}
