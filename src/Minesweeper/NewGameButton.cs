using Engine;

using static SDL2.SDL;

namespace Minesweeper
{
    public class NewGameButton : Button
    {
        private readonly Text _text;

        public NewGameButton(int x, int y, int w, int h)
            : base(x, y, w, h)
        {
            _text = new Text(x, y, w, h, "NEW GAME", new SDL_Color { r = 0, g = 0, b = 0, a = 255 }, 20);
        }

        public override void Render(nint surface)
        {
            base.Render(surface);
            _text.Render(surface);
        }

        protected override void HandleLeftClick()
        {
            SDL_Event e = new SDL_Event
            {
                type = UserEvents.NEW_GAME
            };
            _ = SDL_PushEvent(ref e);
        }
    }
}
