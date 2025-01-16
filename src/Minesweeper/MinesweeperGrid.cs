using System;
using Engine;
using System.Runtime.InteropServices;
using System.Collections.Generic;

using static SDL2.SDL;

namespace Minesweeper
{
    public class MinesweeperGrid
    {
        private readonly List<MinesweeperCell> _children = [];
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

        public void HandleEvent(in SDL_Event e)
        {
            if (e.type == UserEvents.CELL_CLEARED)
            {
                HandleCellCleared(in e.user);
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
                int adjBombs = CountAdjacentBombs(child.Row, child.Col);
                child.HandleEvent(in e, adjBombs);
            }
        }

        private void HandleCellCleared(in SDL_UserEvent e)
        {
            if (e.data1 == IntPtr.Zero)
            {
                throw new Exception("Invalid cell reference in user event.");
            }

            // MinesweeperCell cell = e.data1 as MinesweeperCell;
            // MinesweeperCell cell = Marshal.PtrToStructure<MinesweeperCell>(e.data1);
            // MinesweeperCellData cellData = Marshal.PtrToStructure<MinesweeperCellData>(e.data1);
            GCHandle handle = GCHandle.FromIntPtr(e.data1);

            if (handle.Target is MinesweeperCell cell && cell.HasBomb)
            {
                SDL_Event gameLostEvent = new SDL_Event { type = UserEvents.GAME_LOST };
                _ = SDL_PushEvent(ref gameLostEvent);
            }
            else
            {
                _cellsToClear--;
                if (_cellsToClear == 0)
                {
                    SDL_Event gameWonEvent = new SDL_Event { type = UserEvents.GAME_WON };
                    _ = SDL_PushEvent(ref gameWonEvent);
                }
            }

            handle.Free();
        }

        private void PlaceBombs()
        {
            int bombsToPlace = Config.BOMB_COUNT;

            // Double check this
            _cellsToClear = Config.GRID_COLUMNS * Config.GRID_ROWS - Config.BOMB_COUNT;

            while (bombsToPlace > 0)
            {
                int randomIndex = Random.Shared.Next(0, _children.Count);
                if (_children[randomIndex] is MinesweeperCell cell && !cell.HasBomb && cell.PlaceBomb())
                {
                    // cell.PlaceBomb();
                    // UpdateAdjacentCells(cell.Row, cell.Col);
                    --bombsToPlace;
                }
            }

            // UpdateAllAdjacentBombCounts();
        }

        // Aditional function
        private void UpdateAllAdjacentBombCounts()
        {
            foreach (var cell in _children)
            {
                if (cell is MinesweeperCell minesweeperCell && !minesweeperCell.HasBomb)
                {
                    int adjacentBombs = CountAdjacentBombs(minesweeperCell.Row, minesweeperCell.Col);
                    minesweeperCell.SetAdjacentBombs(adjacentBombs);
                }
            }
        }

        // Aditional function
        private int CountAdjacentBombs(int row, int col)
        {
            int bombCount = 0;

            // Relative positions of all 8 possible neighbors
            int[] dRow = [-1, -1, -1, 0, 0, 1, 1, 1];
            int[] dCol = [-1, 0, 1, -1, 1, -1, 0, 1];

            for (int i = 0; i < 8; i++)
            {
                int adjRow = row + dRow[i];
                int adjCol = col + dCol[i];

                // Ensure the adjacent cell is within bounds
                if (adjRow >= 1 && adjRow <= Config.GRID_ROWS &&
                    adjCol >= 1 && adjCol <= Config.GRID_COLUMNS)
                {
                    int index = (adjRow - 1) * Config.GRID_COLUMNS + (adjCol - 1);
                    if (_children[index] is MinesweeperCell adjCell && adjCell.HasBomb)
                    {
                        bombCount++;
                    }
                }
            }

            return bombCount;
        }

        // Aditional function
        private void UpdateAdjacentCells(int row, int col)
        {
            int[] dRow = [-1, -1, -1, 0, 0, 1, 1, 1];
            int[] dCol = [-1, 0, 1, -1, 1, -1, 0, 1];

            for (int i = 0; i < 8; i++)
            {
                int adjRow = row + dRow[i];
                int adjCol = col + dCol[i];

                if (adjRow >= 1 && adjRow <= Config.GRID_ROWS &&
                    adjCol >= 1 && adjCol <= Config.GRID_COLUMNS)
                {
                    // int index = (adjRow - 1) * Config.GRID_COLUMNS + (adjCol - 1);
                    int index = 1;
                    if (_children[index] is MinesweeperCell adjCell && !adjCell.HasBomb)
                    {
                        adjCell.IncrementAdjacentBombCount();
                    }
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
