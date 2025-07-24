using System;
using System.IO;
using System.Timers;
using System.Collections.Generic;
using SDL2;

namespace cslogo
{
    public class TileEditor
    {
        const int TileSize = 16; // 16x16 pixels per tile
        const int FrameDelay = 16; // ~60 FPS
        const string TilemapPath = "Assets/tilemap.txt";
        const string TilesetPath = "Assets/tileset2.png";

        private static List<TilemapElement> tileElements = new();
        private static bool dirty = true;
        private static Timer debounceTimer;

        static SDL.SDL_Rect GetTileSrcRect(string type, string value)
        {
            int index = 0;
            if ((type == "mine" || type == "mine2") && int.TryParse(value, out int n)) index = n - 1;
            // else if (type == "mine" && value == "F") index = 9;
            // else if (type == "mine" && value == "M") index = 10;
            // else if (type == "mine" && value == "H") index = 11;
            else if (type == "face") index = value switch { "smile" => 0, "win" => 1, "lose" => 2, _ => int.Parse(value) };
            else if (type == "digit" && int.TryParse(value, out int d)) index = d - 1;

            return new SDL.SDL_Rect
            {
                x = index * type switch
                {
                    "mine" or "mine2" => TileSize,
                    "face" => 24,
                    "digit" => 13,
                    _ => TileSize
                },
                y = type switch
                {
                    "mine" => 63,
                    "mine2" => 63 - TileSize,
                    "face" => 24 - 1,
                    "digit" => 0,
                    _ => 0
                },
                w = type switch { "mine" or "mine2" => TileSize, "face" => 24, "digit" => 13, _ => TileSize },
                h = type switch { "mine" or "mine2" => TileSize, "face" => 24, "digit" => 23, _ => TileSize }
            };
        }

        static List<TilemapElement> LoadTileElements()
        {
            var list = new List<TilemapElement>();
            foreach (var line in File.ReadAllLines(TilemapPath))
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;
                var parts = line.Split(',');
                if (parts.Length < 4) continue;

                string type = parts[0];
                int row = int.Parse(parts[1]);
                int col = int.Parse(parts[2]);
                string value = parts[3];
                int width = parts.Length > 4 ? int.Parse(parts[4]) : 1;
                int height = parts.Length > 5 ? int.Parse(parts[5]) : 1;

                list.Add(new TilemapElement(type, row, col, value, width, height));
            }
            return list;
        }

        static void OnChanged(object sender, FileSystemEventArgs e)
        {
            debounceTimer.Stop();
            debounceTimer.Start();
        }

        static void Main()
        {
            SDL.SDL_Init(SDL.SDL_INIT_VIDEO);

            var window = SDL.SDL_CreateWindow("Minesweeper UI Prototype",
                SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED,
                168, 168, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);

            var renderer = SDL.SDL_CreateRenderer(window, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);
            var surface = SDL_image.IMG_Load(TilesetPath);
            var tileset = SDL.SDL_CreateTextureFromSurface(renderer, surface);
            SDL.SDL_FreeSurface(surface);

            tileElements = LoadTileElements();

            // Setup debounce timer
            debounceTimer = new Timer(200); // milliseconds
            debounceTimer.AutoReset = false;
            debounceTimer.Elapsed += (s, e) =>
            {
                try
                {
                    tileElements = LoadTileElements();
                    dirty = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reloading tilemap: {ex.Message}");
                }
            };

            var watcher = new FileSystemWatcher(Path.GetDirectoryName(TilemapPath), Path.GetFileName(TilemapPath))
            {
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Size,
                EnableRaisingEvents = true,
                IncludeSubdirectories = false
            };
            watcher.Changed += OnChanged;
            watcher.Created += OnChanged;
            watcher.Renamed += OnChanged;
            watcher.Deleted += OnChanged;

            bool running = true;

            while (running)
            {
                while (SDL.SDL_PollEvent(out SDL.SDL_Event e) != 0)
                {
                    if (e.type == SDL.SDL_EventType.SDL_QUIT)
                        running = false;
                }

                if (dirty)
                {
                    SDL.SDL_SetRenderDrawColor(renderer, 192, 192, 192, 255);
                    SDL.SDL_RenderClear(renderer);

                    foreach (var tile in tileElements)
                    {
                        SDL.SDL_Rect src = GetTileSrcRect(tile.Type, tile.Value);
                        SDL.SDL_Rect dst = new SDL.SDL_Rect
                        {
                            x = tile.Col * tile.Width,
                            y = tile.Type == "digit" ? tile.Row * tile.Height + 3 : tile.Row * tile.Height,
                            w = tile.Width,
                            h = tile.Height
                        };
                        SDL.SDL_RenderCopy(renderer, tileset, ref src, ref dst);
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
