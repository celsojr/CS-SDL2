using SDL2;
using static SDL2.SDL;

namespace Engine
{
    public class Button : Rectangle
    {
        private bool _isDisabled;

        public Button(int x, int y, int w, int h)
            : base(x, y, w, h, Config.BUTTON_COLOR) { }

        public virtual void HandleEvent(SDL_Event e)
        {
            if (_isDisabled) return;

            if (e.type == SDL_EventType.SDL_MOUSEMOTION)
            {
                HandleMouseMotion(e.motion);
            }
            else if (e.type == SDL_EventType.SDL_MOUSEBUTTONDOWN)
            {
                if (IsWithinBounds(e.button.x, e.button.y))
                {
                    if (e.button.button == SDL_BUTTON_LEFT)
                    {
                        HandleLeftClick();
                    }
                    else
                    {
                        HandleRightClick();
                    }
                }
            }
        }

        public void SetIsDisabled(bool newValue)
        {
            _isDisabled = newValue;
        }

        protected virtual void HandleLeftClick() { }

        protected virtual void HandleRightClick() { }

        protected virtual void HandleMouseMotion(SDL_MouseMotionEvent e)
        {
            if (IsWithinBounds(e.x, e.y))
            {
                SetColor(Config.BUTTON_HOVER_COLOR);
            }
            else
            {
                SetColor(Config.BUTTON_COLOR);
            }
        }
    }
}
