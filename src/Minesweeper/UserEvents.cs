using static SDL2.SDL;

namespace Minesweeper
{
    public static class UserEvents
    {
        // public static SDL_EventType CELL_CLEARED = (SDL_EventType)SDL_RegisterEvents(1);
        // public static SDL_EventType BOMB_PLACED = (SDL_EventType)SDL_RegisterEvents(1);
        // public static SDL_EventType FLAG_PLACED = (SDL_EventType)SDL_RegisterEvents(1);
        // public static SDL_EventType FLAG_CLEARED = (SDL_EventType)SDL_RegisterEvents(1);
        // public static SDL_EventType GAME_WON = (SDL_EventType)SDL_RegisterEvents(1);
        // public static SDL_EventType GAME_LOST = (SDL_EventType)SDL_RegisterEvents(1);
        // public static SDL_EventType NEW_GAME = (SDL_EventType)SDL_RegisterEvents(1);

        public const SDL_EventType CELL_CLEARED = SDL_EventType.SDL_USEREVENT + 1;
        public const SDL_EventType BOMB_PLACED = SDL_EventType.SDL_USEREVENT + 2;
        public const SDL_EventType FLAG_PLACED = SDL_EventType.SDL_USEREVENT + 3;
        public const SDL_EventType FLAG_CLEARED = SDL_EventType.SDL_USEREVENT + 4;
        public const SDL_EventType GAME_WON = SDL_EventType.SDL_USEREVENT + 5;
        public const SDL_EventType GAME_LOST = SDL_EventType.SDL_USEREVENT + 6;
        public const SDL_EventType NEW_GAME = SDL_EventType.SDL_USEREVENT + 7;
    }
}
