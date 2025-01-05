using Engine;

using static SDL2.SDL;

namespace Minesweeper
{
    public class MinesweeperUI
    {
        private readonly MinesweeperGrid _grid;
        private readonly NewGameButton _button;
        private readonly FlagCounter _counter;

        public MinesweeperUI()
        {
            _grid = new MinesweeperGrid(Config.PADDING, Config.PADDING);

            _button = new NewGameButton(
                Config.PADDING,
                Config.GRID_HEIGHT + Config.PADDING * 2,
                Config.WINDOW_WIDTH - Config.PADDING * 3 - Config.FLAG_COUNTER_WIDTH,
                Config.FOOTER_HEIGHT - Config.PADDING
            );

            _counter = new FlagCounter(
                Config.WINDOW_WIDTH - Config.PADDING - Config.FLAG_COUNTER_WIDTH,
                Config.GRID_HEIGHT + Config.PADDING * 2,
                Config.FLAG_COUNTER_WIDTH,
                Config.FOOTER_HEIGHT - Config.PADDING
            );
        }

        public void Render(nint surfacePointer)
        {
            _grid.Render(surfacePointer);
            _button.Render(surfacePointer);
            _counter.Render(surfacePointer);
        }

        public void HandleEvent(SDL_Event e)
        {
            _grid.HandleEvent(e);
            _button.HandleEvent(e);
            _counter.HandleEvent(e);
        }
    }
}
