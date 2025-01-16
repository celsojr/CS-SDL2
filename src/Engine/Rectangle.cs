using SDL2;
using System.Runtime.InteropServices;

using static SDL2.SDL;

namespace Engine
{
    public class Rectangle
    {
        private SDL_Rect _rect;
        private SDL_Color _color;

        public Rectangle(int x, int y, int w, int h, SDL_Color? color = null)
        {
            _rect = new SDL_Rect { x = x, y = y, w = w, h = h };
            _color = color ?? new SDL_Color { r = 0, g = 0, b = 0, a = 255 };
        }

        public virtual void Render(nint surfacePointer)
        {
            // SDL_Surface surface = (SDL_Surface)surfacePointer;
            SDL_Surface surface = Marshal.PtrToStructure<SDL_Surface>(surfacePointer);
            _ = SDL_FillRect(
                surfacePointer,
                ref _rect,
                SDL_MapRGB(
                    // SDL.SDL_GetSurfaceFormat(surface),
                    surface.format,
                    _color.r,
                    _color.g,
                    _color.b
                )
            );
        }

        public void SetColor(SDL_Color color)
        {
            _color = color;
        }

        public bool IsWithinBounds(int x, int y)
        {
            // Too far left
            if (x < _rect.x) return false;
            // Too far right
            if (x > _rect.x + _rect.w) return false;
            // Too high
            if (y < _rect.y) return false;
            // Too low
            if (y > _rect.y + _rect.h) return false;
            // Within bounds
            return true;
        }

        public SDL_Rect GetRect()
        {
            return _rect;
        }
    }
}
