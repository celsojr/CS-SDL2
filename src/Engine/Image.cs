using System;

using static SDL2.SDL;
using static SDL2.SDL_image;

namespace Engine
{
    public class Image
    {
        private readonly nint _imageSurface;
        // private readonly SDL_Rect _destination;
        // private static SDL_Rect _destination;
        private SDL_Rect _destination;

        public Image(int x, int y, int w, int h, string filename, int padding = 12)
        {
            _destination = new SDL_Rect
            {
                x = x + padding / 2,
                y = y + padding / 2,
                w = w - padding,
                h = h - padding
            };

            _imageSurface = IMG_Load(filename);
            if (_imageSurface == nint.Zero)
            {
#if SHOW_DEBUG_HELPERS
                Utils.CheckSDLError("IMG_Load");
#endif
                throw new Exception($"Failed to load image: {filename}");
            }
        }

        public void Render(nint surface)
        {
            SDL_BlitScaled(_imageSurface, IntPtr.Zero, surface, ref _destination);
        }

        ~Image()
        {
            if (_imageSurface != nint.Zero)
            {
                SDL_FreeSurface(_imageSurface);
            }
        }

        public Image(Image other) : this(
            other._destination.x,
            other._destination.y,
            other._destination.w,
            other._destination.h,
            string.Empty) { }
    }
}
