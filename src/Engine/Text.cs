using System;
using SdlEngine;
using System.Runtime.InteropServices;

using static SDL2.SDL;
using static SDL2.SDL_ttf;

namespace Engine
{
    public class Text
    {
        private readonly nint _font;
        private nint _textSurface;
        private SDL_Rect _destinationRect;
        private SDL_Rect _textPosition;
        private SDL_Color _color;

        public Text(int x, int y, int w, int h, string content, SDL_Color color = default, int fontSize = 30)
        {
            _destinationRect = new SDL_Rect { x = x, y = y, w = w, h = h };
            _color = color.Equals(default(SDL_Color)) ? new SDL_Color { r = 0, g = 0, b = 0, a = 255 } : color;

            _font = TTF_OpenFont(Config.FONT, fontSize);
            if (_font == nint.Zero)
            {
#if SHOW_DEBUG_HELPERS
                Utils.CheckSDLError("TTF_OpenFont");
#endif
                throw new Exception($"Failed to load font: {Config.FONT}");
            }

            SetText(content);
        }

        public void SetText(string text)
        {
            SetText(text, _color);
        }

        public void SetText(string text, SDL_Color newColor)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new Exception($"text cannot be null or empty.");
            }

            if (_textSurface != nint.Zero)
            {
                SDL_FreeSurface(_textSurface);
            }

            _color = newColor;
            _textSurface = TTF_RenderUTF8_Blended(_font, text, _color);

            if (_textSurface == nint.Zero)
            {
#if SHOW_DEBUG_HELPERS
                Utils.CheckSDLError("TTF_RenderUTF8_Blended");
#endif
                throw new Exception($"Failed to render text: {text}");
            }

            CenterTextPosition();
        }

        private void CenterTextPosition()
        {
            if (_textSurface == nint.Zero)
            {
                throw new InvalidOperationException("Text surface is not initialized.");
            }

            SDL_Surface surface = Marshal.PtrToStructure<SDL_Surface>(_textSurface);

            int widthDifference = _destinationRect.w - surface.w;
            int leftOffset = widthDifference / 2;

            int heightDifference = _destinationRect.h - surface.h;
            int topOffset = heightDifference / 2;

            _textPosition = new SDL_Rect
            {
                x = _destinationRect.x + leftOffset,
                y = _destinationRect.y + topOffset,
                w = surface.w,
                h = surface.h
            };
        }

        public void Render(nint surface)
        {
            if (_textSurface != nint.Zero)
            {
                _ = SDL_BlitSurface(_textSurface, IntPtr.Zero, surface, ref _textPosition);
            }
        }

        ~Text()
        {
            if (_font != nint.Zero)
            {
                TTF_CloseFont(_font);
            }

            if (_textSurface != nint.Zero)
            {
                SDL_FreeSurface(_textSurface);
            }
        }
    }
}
