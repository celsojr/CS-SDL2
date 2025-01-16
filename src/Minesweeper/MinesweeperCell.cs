using System;
using Engine;
using System.Runtime.InteropServices;

using static SDL2.SDL;

namespace Minesweeper
{
    public class MinesweeperCell : Button
    {
        public bool HasBomb { get { return _hasBomb; } private set { _hasBomb = value; } }
        public int AdjacentBombs { get { return _adjacentBombs; } private set { _adjacentBombs = value; } }
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

        public override void HandleEvent(in SDL_Event e, int adjBombs)
        {
            if (_isCleared) return;

            base.HandleEvent(in e, adjBombs);

            if (e.type == UserEvents.CELL_CLEARED)
            {
                HandleCellCleared(in e.user);
                // HandleCellCleared(e);
            }
            else if (e.type == UserEvents.BOMB_PLACED)
            {
                HandleBombPlaced(in e.user, adjBombs);
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
                    // _text.SetText(8.ToString(), Config.TEXT_COLORS[8]);
                    _text.Render(surfacePointer);
                }
            }

#if DEBUG
            if (_hasBomb) _bombImage.Render(surfacePointer);
#endif
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

            // Console.WriteLine("Cell bomb placed");

            _hasBomb = true;

            // ReportEvent(UserEvents.BOMB_PLACED);

            // Handle adjacent bombs here



            return true;
        }

        private void ClearCell()
        {
            if (_isCleared) return;
            _isCleared = true;
            SetIsDisabled(true);
            SetColor(Config.BUTTON_CLEARED_COLOR);
            ReportEvent(UserEvents.CELL_CLEARED);
        }

        private void ReportEvent(SDL_EventType eventType)
        {
            if (eventType < SDL_EventType.SDL_USEREVENT)
            {
                Console.WriteLine($"Invalid user event type: {eventType}");
                return;
            }

            var handle = GCHandle.Alloc(this);

            try
            {
                SDL_Event sdlEvent = new SDL_Event
                {
                    type = eventType,
                    user = new SDL_UserEvent
                    {
                        type = eventType,
                        data1 = (nint)handle
                    }
                };

                // SDL_Event[] events = new SDL_Event[10];
                // int count = SDL_PeepEvents(events, 10, SDL_eventaction.SDL_PEEKEVENT, SDL_EventType.SDL_FIRSTEVENT, SDL_EventType.SDL_LASTEVENT);
                // Console.WriteLine($"Number of events in queue: {count}");
                // 398 Filtered out a mouse motion event!

                if (SDL_PushEvent(ref sdlEvent) != 0)
                {
                    Console.WriteLine($"Failed to push event: {SDL_GetError()}");
                }
            }
            finally
            {
                handle.Free();
            }
        }

        private void HandleCellCleared(in SDL_UserEvent e)
        {
            if (_isCleared) return;

            GCHandle handle = GCHandle.FromIntPtr(e.data1);

            if (handle.Target is MinesweeperCell other && IsAdjacent(other) && !other._hasBomb)
            {
                ClearCell();
            }

            handle.Free();
        }

        private void HandleBombPlaced(in SDL_UserEvent e, int adjBombs)
        {
            GCHandle handle = GCHandle.FromIntPtr(e.data1);

            if (handle.Target is MinesweeperCell other && IsAdjacent(other))
            {
                // _adjacentBombs++;
                // ++_adjacentBombs;
                ++_adjacentBombs;
                Console.WriteLine(_adjacentBombs + " bombs placed");
                Console.WriteLine(adjBombs + " bombs placed");
                // _text.SetText(_adjacentBombs.ToString(), Config.TEXT_COLORS[other._adjacentBombs]);
                _text.SetText(_adjacentBombs.ToString(), Config.TEXT_COLORS[_adjacentBombs]);
                // int adjacentBombs = other.CountAdjacentBombs();
                // _text.SetText(_adjacentBombs.ToString(), Config.TEXT_COLORS[cell.CountAdjacentBombs()]);
            }

            handle.Free();
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
                ReportEvent(_hasFlag ? UserEvents.FLAG_PLACED : UserEvents.FLAG_CLEARED);
            }
        }
    }
}
