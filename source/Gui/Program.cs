﻿using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Core;
using SDL2;
using Timer = Core.Timer;

namespace Gui
{
    class Program
    {
        private static Cpu _cpu;
        private static DisplayDataTransferService _displayDataTransferService;
        private static Display _display;
        private static IMmu _mmu;
        private static Timer _timer;

        static void Main(string[] args)
        {
            SDL.SDL_Init(SDL.SDL_INIT_VIDEO);

            var window = SDL.SDL_CreateWindow("An SDL Window", 100, 100, 640, 576, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);
            var renderer = SDL.SDL_CreateRenderer(window, -1, 0);

            var emulatorBootstrapper = new EmulatorBootstrapper();
            var readAllBytes = File.ReadAllBytes(args[1]);
            var emulator = emulatorBootstrapper.LoadRom(readAllBytes);
            var joypad = emulator.Joypad;
            _mmu = emulator.Mmu;
            _cpu = emulator.Cpu;
            _timer = emulator.Timer;
            _display = emulator.Display;
            _displayDataTransferService = emulator.DisplayDataTransferService;

            var running = true;
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            const double cpuSpeed = (4194304) / 4.0;
            const double cyclesPerFrame = cpuSpeed / 60.0;
            var ticksPerSecond = TimeSpan.TicksPerSecond;
            double cyclesUntilNextFrame = 0;
            long milliSeconds = 0;
            var cyclesPerMillisecond = cpuSpeed / 1000;
            while (running)
            {
                for (var i = 0; i < cyclesPerMillisecond; i++)
                {
                    EmulateCycle();
                }

                while (milliSeconds > stopwatch.ElapsedMilliseconds)
                {
                    Thread.Sleep(1);
                }
                milliSeconds++;

                if (_cpu.Cycles > cyclesUntilNextFrame)
                {
                    Draw(renderer);
                    cyclesUntilNextFrame += cyclesPerFrame;
                }

                SDL.SDL_Event newEvent;
                SDL.SDL_PollEvent(out newEvent);

                switch (newEvent.type)
                {
                    case SDL.SDL_EventType.SDL_QUIT:
                        running = false;
                        break;
                    case SDL.SDL_EventType.SDL_KEYUP:
                        Up(joypad, KeyState.FromKeyEvent(newEvent.key));
                        break;
                    case SDL.SDL_EventType.SDL_KEYDOWN:
                        running = newEvent.key.keysym.scancode != SDL.SDL_Scancode.SDL_SCANCODE_ESCAPE;
                        Down(joypad, KeyState.FromKeyEvent(newEvent.key));
                        break;
                }
            }

            SDL.SDL_DestroyRenderer(renderer);
            SDL.SDL_DestroyWindow(window);
            SDL.SDL_Quit();
        }


        private static void Up(Joypad joypad, KeyState newEvent)
        {
            joypad.Left = !newEvent.Left && joypad.Left;
            joypad.Right = !newEvent.Right && joypad.Right;
            joypad.Up = !newEvent.Up && joypad.Up;
            joypad.Down = !newEvent.Down && joypad.Down;
            joypad.A = !newEvent.A && joypad.A;
            joypad.B = !newEvent.B && joypad.B;
            joypad.Start = !newEvent.Start && joypad.Start;
            joypad.Select = !newEvent.Select && joypad.Select;
        }

        private static void Down(Joypad joypad, KeyState newEvent)
        {
            joypad.Left = newEvent.Left || joypad.Left;
            joypad.Right = newEvent.Right || joypad.Right;
            joypad.Up = newEvent.Up || joypad.Up;
            joypad.Down = newEvent.Down || joypad.Down;
            joypad.A = newEvent.A || joypad.A;
            joypad.B = newEvent.B || joypad.B;
            joypad.Start = newEvent.Start || joypad.Start;
            joypad.Select = newEvent.Select || joypad.Select;
        }

        private static ushort[] _trace = new ushort[10000];
        private static int _index;
        private static void EmulateCycle()
        {
            var next = _cpu.ProgramCounter;
            _trace[_index++] = next;
            _index = _index % _trace.Length;
            var instruction = _mmu.GetByte(next);

            var old = _cpu.Cycles;
            _cpu.Execute(instruction);
            var delta = _cpu.Cycles - old;
            for (var i = 0; i < delta; i++)
            {
                _display.Tick();
                _timer.Tick();
            }
        }

        private static void PrintTrace()
        {
            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine(_trace[_index - i].ToString("x4"));
            }
        }

        private static void Draw(IntPtr renderer)
        {
            var surface = CreateSurface();
            var texture = SDL.SDL_CreateTextureFromSurface(renderer, surface);

            SDL.SDL_RenderCopy(renderer, texture, (IntPtr)null, (IntPtr)null);
            SDL.SDL_RenderPresent(renderer);
            SDL.SDL_DestroyTexture(texture);
            SDL.SDL_FreeSurface(surface);
        }

        private static IntPtr CreateSurface()
        {
            var buffer = new uint[160 * 144];
            for (var x = 0; x < 160; x++)
            {
                for (var y = 0; y < 144; y++)
                {
                    var color = _displayDataTransferService.FrameBuffer[y * 160 + x];
                    buffer[y * 160 + x] = GetColor(color);
                }
            }

            var gcHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            var address = gcHandle.AddrOfPinnedObject();

            var surface = SDL.SDL_CreateRGBSurfaceFrom(address, 160, 144, 32, 160 * 4, 0, 0, 0, 0x000000FF);
            gcHandle.Free();
            return surface;
        }

        private static uint GetColor(byte color)
        {
            switch (color)
            {
                case 0:
                    return 0xFFFFFFFF;
                case 1:
                    return 0xD3D3D3FF;
                case 2:
                    return 0xA9A9A9FF;
                case 3:
                    return 0;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
