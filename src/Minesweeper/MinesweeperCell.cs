using System;
using Engine;
using System.Runtime.InteropServices;

using static SDL2.SDL;

namespace Minesweeper
{
    public class MinesweeperCell : Button
    {
        public bool HasBomb { get; private set; }
        public int AdjacentBombs { get; private set; }
        public int Row { get { return _row; } }
        public int Col { get { return _col; } }

        private int _adjacentBombs;
        private readonly int _row;
        private readonly int _col;
        private bool _hasBomb;
        private bool _hasFlag;
        private bool _isCleared;

        private readonly Image _flagImage;
        private readonly Image _bombImage;
        private readonly Text _text;

        public void IncrementAdjacentBombCount()
        {
            AdjacentBombs++;
        }

        public void SetAdjacentBombs(int count)
        {
            AdjacentBombs = count;

            if (count > 0)
            {
                _text.SetText(count.ToString(), Config.TEXT_COLORS[count]);
            }
        }


        public MinesweeperCell(int x, int y, int w, int h, int row, int col)
            : base(x, y, w, h)
        {
            _row = row;
            _col = col;
            _flagImage = new Image(x, y, w, h, "/Users/celsojr/Repos/CS-SDL2/Assets/flag.png");
            _bombImage = new Image(x, y, w, h, "/Users/celsojr/Repos/CS-SDL2/Assets/bomb.png");
            // _text = new Text(x, y, w, h, "", Config.TEXT_COLORS[0]);
            _text = new Text(x, y, w, h, " ", Config.TEXT_COLORS[0]);
        }

        public override void HandleEvent(SDL_Event e)
        {
            if (_isCleared) return;

            base.HandleEvent(e);

            if (e.type == UserEvents.CELL_CLEARED)
            {
                HandleCellCleared(e.user);
                // HandleCellCleared(e);
            }
            else if (e.type == UserEvents.BOMB_PLACED)
            {
                HandleBombPlaced(e.user);
                // HandleBombPlaced(e);
            }
            else if (e.type == UserEvents.GAME_WON)
            {
                if (_hasBomb)
                {
                    _hasFlag = true;
                    SetColor(Config.BUTTON_SUCCESS_COLOR);
                }
                SetIsDisabled(true);
            }
            else if (e.type == UserEvents.GAME_LOST)
            {
                if (_hasBomb)
                {
                    _isCleared = true;
                    SetColor(Config.BUTTON_FAILURE_COLOR);
                }
                SetIsDisabled(true);
            }
        }

        public override void Render(nint surfacePointer)
        {
            base.Render(surfacePointer);

            if (_hasFlag)
            {
                _flagImage.Render(surfacePointer);
            }
            else if (_isCleared)
            {
                if (_hasBomb)
                {
                    _bombImage.Render(surfacePointer);
                }
                else if (_adjacentBombs > 0)
                {
                    _text.SetText(_adjacentBombs.ToString(), Config.TEXT_COLORS[_adjacentBombs]);
                    _text.Render(surfacePointer);
                }
            }
        }

        public void Reset()
        {
            _isCleared = false;
            _hasFlag = false;
            _hasBomb = false;
            _adjacentBombs = 0;
            SetIsDisabled(false);
            SetColor(Config.BUTTON_COLOR);
            _text.SetText(" ");
        }

        public bool PlaceBomb()
        {
            if (_hasBomb) return false;

            _hasBomb = true;
            ReportEvent((uint)UserEvents.BOMB_PLACED);
            return true;
        }

        private void ClearCell()
        {
            if (_isCleared) return;
            _isCleared = true;
            SetIsDisabled(true);
            SetColor(Config.BUTTON_CLEARED_COLOR);
            ReportEvent((uint)UserEvents.CELL_CLEARED);
        }

        private void ReportEvent(uint eventType)
        {
            SDL_Event sdlEvent = new SDL_Event
            {
                type = (SDL_EventType)eventType,
                user = new SDL_UserEvent
                {
                    type = (SDL_EventType)eventType,
                    data1 = (nint)GCHandle.Alloc(this)
                }
            };
            _ = SDL_PushEvent(ref sdlEvent);
        }

        private void HandleCellCleared(SDL_UserEvent e)
        {
            if (_isCleared) return;

            GCHandle handle = GCHandle.FromIntPtr(e.data1);

            if (handle.Target is MinesweeperCell other && IsAdjacent(other) && !other._hasBomb)
            {
                ClearCell();
            }

            handle.Free();
        }

        private void HandleBombPlaced(SDL_UserEvent e)
        {
            GCHandle handle = GCHandle.FromIntPtr(e.data1);

            if (handle.Target is MinesweeperCell other && IsAdjacent(other))
            {
                _adjacentBombs++;
                _text.SetText(_adjacentBombs.ToString(), Config.TEXT_COLORS[_adjacentBombs]);
            }
        }

        private bool IsAdjacent(MinesweeperCell other)
        {
            return !(other == this)
                && Math.Abs(other._row - _row) <= 1
                && Math.Abs(other._col - _col) <= 1;
        }

        protected override void HandleLeftClick()
        {
            if (!_hasFlag && !_isCleared)
            {
                ClearCell();
            }
        }

        protected override void HandleRightClick()
        {
            if (!_isCleared)
            {
                _hasFlag = !_hasFlag;
                ReportEvent(_hasFlag ? (uint)UserEvents.FLAG_PLACED : (uint)UserEvents.FLAG_CLEARED);
            }
        }
    }
}
