using System;

using static SDL2.SDL;

namespace Minesweeper
{
    public static class UserEvents
    {
        public static SDL_EventType CELL_CLEARED = (SDL_EventType)SDL_RegisterEvents(1);
        public static SDL_EventType BOMB_PLACED = (SDL_EventType)SDL_RegisterEvents(1);
        public static SDL_EventType FLAG_PLACED = (SDL_EventType)SDL_RegisterEvents(1);
        public static SDL_EventType FLAG_CLEARED = (SDL_EventType)SDL_RegisterEvents(1);
        public static SDL_EventType GAME_WON = (SDL_EventType)SDL_RegisterEvents(1);
        public static SDL_EventType GAME_LOST = (SDL_EventType)SDL_RegisterEvents(1);
        public static SDL_EventType NEW_GAME = (SDL_EventType)SDL_RegisterEvents(1);
    }
}
