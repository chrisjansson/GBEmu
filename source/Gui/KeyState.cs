using SDL2;

namespace Gui
{
    public struct KeyState
    {
        public bool Left;
        public bool Right;
        public bool Up;
        public bool Down;

        public bool A;
        public bool B;
        public bool Start;
        public bool Select;

        public static KeyState FromKeyEvent(SDL.SDL_KeyboardEvent key)
        {
            var scanCode = key.keysym.scancode;
            return new KeyState
            {
                Left = scanCode == SDL.SDL_Scancode.SDL_SCANCODE_LEFT,
                Right = scanCode == SDL.SDL_Scancode.SDL_SCANCODE_RIGHT,
                Up = scanCode == SDL.SDL_Scancode.SDL_SCANCODE_UP,
                Down = scanCode == SDL.SDL_Scancode.SDL_SCANCODE_DOWN,
                A = scanCode == SDL.SDL_Scancode.SDL_SCANCODE_X,
                B = scanCode == SDL.SDL_Scancode.SDL_SCANCODE_Z,
                Start = scanCode == SDL.SDL_Scancode.SDL_SCANCODE_RETURN,
                Select = scanCode == SDL.SDL_Scancode.SDL_SCANCODE_SPACE
            };
        }
    }
}