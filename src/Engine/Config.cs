using System.Collections.Generic;
using static SDL2.SDL;

namespace Engine
{
    public static class Config
    {
        // Game Settings
        public const string GAME_NAME = "Minesweeper";
        public const int BOMB_COUNT = 8;
        public const int GRID_ROWS = 5;
        public const int GRID_COLUMNS = 5;

        static Config()
        {
            // Assert that BOMB_COUNT is less than total cells
            if (BOMB_COUNT >= GRID_COLUMNS * GRID_ROWS)
                throw new System.Exception("Cannot have more bombs than cells");
        }

        // Size and Positioning
        public const int PADDING = 5;
        public const int CELL_SIZE = 50;
        public const int FOOTER_HEIGHT = 60;
        public const int FLAG_COUNTER_WIDTH = 100;

        public static int GRID_HEIGHT => CELL_SIZE * GRID_ROWS + PADDING * (GRID_ROWS - 1);
        public static int GRID_WIDTH => CELL_SIZE * GRID_COLUMNS + PADDING * (GRID_COLUMNS - 1);
        public static int WINDOW_HEIGHT => GRID_HEIGHT + FOOTER_HEIGHT + PADDING * 2;
        public static int WINDOW_WIDTH => GRID_WIDTH + PADDING * 2;

        // Colors
        public static readonly SDL_Color BACKGROUND_COLOR = new SDL_Color { r = 170, g = 170, b = 170, a = 255 };
        public static readonly SDL_Color BUTTON_COLOR = new SDL_Color { r = 200, g = 200, b = 200, a = 255 };
        public static readonly SDL_Color BUTTON_HOVER_COLOR = new SDL_Color { r = 220, g = 220, b = 220, a = 255 };
        public static readonly SDL_Color BUTTON_CLEARED_COLOR = new SDL_Color { r = 240, g = 240, b = 240, a = 255 };
        public static readonly SDL_Color BUTTON_SUCCESS_COLOR = new SDL_Color { r = 210, g = 235, b = 210, a = 255 };
        public static readonly SDL_Color BUTTON_FAILURE_COLOR = new SDL_Color { r = 235, g = 210, b = 210, a = 255 };
        public static readonly SDL_Color FLAG_COUNTER_COLOR = new SDL_Color { r = 80, g = 80, b = 80, a = 255 };

        // Text color based on number of surrounding bombs
        public static readonly List<SDL_Color> TEXT_COLORS = new List<SDL_Color>
        {
            new SDL_Color { r = 0, g = 0, b = 0, a = 255 }, // Unused
            new SDL_Color { r = 0, g = 1, b = 249, a = 255 },
            new SDL_Color { r = 1, g = 126, b = 1, a = 255 },
            new SDL_Color { r = 250, g = 1, b = 2, a = 255 },
            new SDL_Color { r = 1, g = 0, b = 128, a = 255 },
            new SDL_Color { r = 129, g = 1, b = 0, a = 255 },
            new SDL_Color { r = 0, g = 128, b = 128, a = 255 },
            new SDL_Color { r = 0, g = 0, b = 0, a = 255 },
            new SDL_Color { r = 128, g = 128, b = 128, a = 255 }
        };

        // Asset Paths
        public const string BOMB_IMAGE = "/Users/celsojr/Repos/CS-SDL2/Assets/Bomb.png";
        public const string FLAG_IMAGE = "/Users/celsojr/Repos/CS-SDL2/Assets/flag.png";
        public const string FONT = "/Users/celsojr/Repos/CS-SDL2/Assets/Rubik-SemiBold.ttf";
    }
}
