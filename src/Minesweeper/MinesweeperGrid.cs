using System;
using Engine;
using System.Runtime.InteropServices;
using System.Collections.Generic;

using static SDL2.SDL;

namespace Minesweeper
{
    public class MinesweeperGrid
    {
        private readonly List<MinesweeperCell> _children = new();
        private int _cellsToClear;

        public MinesweeperGrid(int x, int y)
        {
            int spacing = Config.CELL_SIZE + Config.PADDING;
            for (int col = 1; col <= Config.GRID_COLUMNS; col++)
            {
                for (int row = 1; row <= Config.GRID_ROWS; row++)
                {
                    _children.Add(new MinesweeperCell(
                        x + spacing * (col - 1),
                        y + spacing * (row - 1),
                        Config.CELL_SIZE,
                        Config.CELL_SIZE,
                        row,
                        col
                    ));
                }
            }
            PlaceBombs();
        }

        public void Render(nint surfacePointer)
        {
            foreach (var child in _children)
            {
                child.Render(surfacePointer);
            }
        }

        public void HandleEvent(SDL_Event e)
        {
            if (e.type == UserEvents.CELL_CLEARED)
            {
                HandleCellCleared(e.user);
            }
            else if (e.type == UserEvents.NEW_GAME)
            {
                foreach (MinesweeperCell child in _children)
                {
                    child.Reset();
                }
                PlaceBombs();
            }

            foreach (MinesweeperCell child in _children)
            {
                child.HandleEvent(e);
            }
        }

        private void HandleCellCleared(SDL_UserEvent e)
        {
            if (e.data1 == IntPtr.Zero)
            {
                throw new Exception("Invalid cell reference in user event.");
            }

            // MinesweeperCell cell = e.data1 as MinesweeperCell;
            // MinesweeperCell cell = Marshal.PtrToStructure<MinesweeperCell>(e.data1);
            MinesweeperCellData cellData = Marshal.PtrToStructure<MinesweeperCellData>(e.data1);

            if (cellData.HasBomb)
            {
                SDL_Event gameLostEvent = new SDL_Event { type = UserEvents.GAME_LOST };
                SDL_PushEvent(ref gameLostEvent);
            }
            else
            {
                _cellsToClear--;
                if (_cellsToClear == 0)
                {
                    SDL_Event gameWonEvent = new SDL_Event { type = UserEvents.GAME_WON };
                    SDL_PushEvent(ref gameWonEvent);
                }
            }
        }

        private void PlaceBombs()
        {
            int bombsToPlace = Config.BOMB_COUNT;
            _cellsToClear = Config.GRID_COLUMNS * Config.GRID_ROWS - Config.BOMB_COUNT;

            Random random = new Random();
            while (bombsToPlace > 0)
            {
                int randomIndex = random.Next(0, _children.Count);
                if (_children[randomIndex].PlaceBomb())
                {
                    bombsToPlace--;
                }
            }
        }
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct MinesweeperCellData
{
    public int Row;
    public int Col;
    public bool HasBomb;
    public int AdjacentBombs;
}
