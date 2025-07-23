using System;
using System.IO;
using SDL2;

namespace cslogo
{
    public class TileEditor
    {
        const int TileSize = 16; // 32x32 pixels per tile
        const int FrameDelay = 16; // ~60 FPS
        const string TilemapPath = "Assets/tilemap.txt";
        const string TilesetPath = "Assets/tiles.png";

        static SDL.SDL_Rect GetTileSrcRect(char c)
        {
            int index = c switch
            {
                '0' => 0,
                '1' => 1,
                '2' => 2,
                '3' => 3,
                '4' => 4,
                '5' => 5,
                '6' => 6,
                '7' => 7,
                '8' => 8,
                'F' => 9,
                'M' => 10,
                'H' => 11,
                _ => 11 // Default to hidden
            };
            return new SDL.SDL_Rect { x = index * TileSize, y = 0, w = TileSize, h = TileSize };
        }

        static char[,] LoadTilemap()
        {
            var lines = File.ReadAllLines(TilemapPath);
            int rows = lines.Length;
            int cols = lines[0].Length;
            var map = new char[rows, cols];
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                    map[r, c] = lines[r][c];
            return map;
        }

        static void Main()
        {
            SDL.SDL_Init(SDL.SDL_INIT_VIDEO);
            var window = SDL.SDL_CreateWindow("Minesweeper UI Prototype",
                SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED,
                800, 600, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);

            var renderer = SDL.SDL_CreateRenderer(window, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);
            var surface = SDL_image.IMG_Load(TilesetPath);
            var tileset = SDL.SDL_CreateTextureFromSurface(renderer, surface);
            SDL.SDL_FreeSurface(surface);

            char[,] tilemap = LoadTilemap();
            bool dirty = true;

            // Watch file changes
            var watcher = new FileSystemWatcher(Path.GetDirectoryName(TilemapPath) ?? ".", Path.GetFileName(TilemapPath))
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size,
                EnableRaisingEvents = true
            };
            watcher.Changed += (s, e) =>
            {
                try
                {
                    tilemap = LoadTilemap();
                    dirty = true;
                }
                catch { /* might be reading during write, ignore */ }
            };

            bool running = true;
            SDL.SDL_Event e;
            while (running)
            {
                while (SDL.SDL_PollEvent(out e) != 0)
                {
                    if (e.type == SDL.SDL_EventType.SDL_QUIT)
                        running = false;
                }

                if (dirty)
                {
                    SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255);
                    SDL.SDL_RenderClear(renderer);

                    for (int r = 0; r < tilemap.GetLength(0); r++)
                    {
                        for (int c = 0; c < tilemap.GetLength(1); c++)
                        {
                            SDL.SDL_Rect src = GetTileSrcRect(tilemap[r, c]);
                            SDL.SDL_Rect dst = new SDL.SDL_Rect
                            {
                                x = c * TileSize,
                                y = r * TileSize,
                                w = TileSize,
                                h = TileSize
                            };
                            SDL.SDL_RenderCopy(renderer, tileset, ref src, ref dst);
                        }
                    }

                    SDL.SDL_RenderPresent(renderer);
                    dirty = false;
                }

                SDL.SDL_Delay(FrameDelay);
            }

            SDL.SDL_DestroyTexture(tileset);
            SDL.SDL_DestroyRenderer(renderer);
            SDL.SDL_DestroyWindow(window);
            SDL.SDL_Quit();
        }
    }
}
