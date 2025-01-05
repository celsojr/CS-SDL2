using Engine;

using static SDL2.SDL;

namespace Minesweeper
{
    public class FlagCounter : Rectangle
    {
        private readonly Image _image;
        private readonly Text _text;
        private int _flagsAvailable;

        public FlagCounter(int x, int y, int w, int h)
            : base(x, y, w, h, Config.FLAG_COUNTER_COLOR)
        {
            _image = new Image(
                x, y,
                Config.FOOTER_HEIGHT - Config.PADDING,
                Config.FOOTER_HEIGHT - Config.PADDING,
                Config.FLAG_IMAGE,
                24
            );

            _text = new Text(
                x + Config.FOOTER_HEIGHT, y,
                w - Config.FOOTER_HEIGHT - 24, h,
                Config.BOMB_COUNT.ToString(),
                new SDL_Color { r = 255, g = 255, b = 255, a = 255 },
                20
            );

            _flagsAvailable = Config.BOMB_COUNT;
        }

        public override void Render(nint surfacePointer)
        {
            base.Render(surfacePointer);
            _text.Render(surfacePointer);
            _image.Render(surfacePointer);
        }

        public void HandleEvent(SDL_Event e)
        {
            if (e.type == UserEvents.FLAG_PLACED)
            {
                --_flagsAvailable;
            }
            else if (e.type == UserEvents.FLAG_CLEARED)
            {
                ++_flagsAvailable;
            }
            else if (e.type == UserEvents.GAME_WON)
            {
                _flagsAvailable = 0;
            }
            else if (e.type == UserEvents.NEW_GAME)
            {
                _flagsAvailable = Config.BOMB_COUNT;
            }
            else
            {
                return;
            }

            _text.SetText(_flagsAvailable.ToString());
        }
    }
}
