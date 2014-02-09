using System;
using SDL2;

namespace Gui
{
    class Program
    {
        static void Main(string[] args)
        {
            SDL.SDL_Init(SDL.SDL_INIT_VIDEO);

            var window = SDL.SDL_CreateWindow("An SDL Window", 100, 100, 800, 600, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);
            var renderer = SDL.SDL_CreateRenderer(window, -1, 0);

            var running = true;
            while (running)
            {
                SDL.SDL_Event newEvent;
                SDL.SDL_PollEvent(out newEvent);

                switch (newEvent.type)
                {
                    case SDL.SDL_EventType.SDL_QUIT:
                        running = false;
                        break;
                    case SDL.SDL_EventType.SDL_KEYDOWN:
                        running = newEvent.key.keysym.scancode != SDL.SDL_Scancode.SDL_SCANCODE_ESCAPE;
                        break;
                }

                Draw(renderer);
            }

            SDL.SDL_DestroyRenderer(renderer);
            SDL.SDL_DestroyWindow(window);
            SDL.SDL_Quit();
        }

        private static void Draw(IntPtr renderer)
        {
            var frameBuffer = new FrameBuffer();

            SDL.SDL_RenderClear(renderer);

            var intPtr = frameBuffer.CreateSurface();
            SDL.SDL_RenderCopy(renderer, intPtr, IntPtr.Zero, IntPtr.Zero);
            SDL.SDL_RenderPresent(renderer);

            SDL.SDL_DestroyTexture(intPtr);


            //SDL.SDL_SetRenderDrawColor(renderer, 255, 0, 0, 0);

            //var rect = new SDL.SDL_Rect { w = 200, h = 200, x = 20, y = 20 };
            //SDL.SDL_RenderFillRect(renderer, ref rect);
            //SDL.SDL_RenderDrawPoint(renderer, 20, 20);
            //SDL.SDL_RenderPresent(renderer);
        }
    }

    public class FrameBuffer
    {
        private const int Width = 160;
        private const int Height = 144;

        private byte[] _data;

        public FrameBuffer()
        {
            _data = new byte[Width * Height];
        }

        public IntPtr CreateSurface()
        {
            var surface = SDL.SDL_CreateRGBSurface(0, Width, Height, 32, 0, 0, 0, 0);

            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    var rect = new SDL.SDL_Rect
                    {
                        w = 1,
                        h = 1,
                        x = x,
                        y = y,
                    };

                    int d = (int) (255.0*((double) (x + y*Width)/(Width*Height)));
                    SDL.SDL_FillRect(surface, ref rect, (uint) ((d << 23) | (d << 15) | (d << 7)));
                }
            }

            return surface;
        }
    }
}
