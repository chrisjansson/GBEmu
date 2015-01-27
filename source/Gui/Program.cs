﻿using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Core;
using SDL2.SDL;

namespace Gui
{
    class Program
    {
        static void Main(string[] args)
        {
            SDL_Init(SDL_INIT_VIDEO);

            var window = SDL_CreateWindow("An SDL Window", 100, 100, 640, 576, SDL_WindowFlags.SDL_WINDOW_SHOWN);
            var renderer = SDL_CreateRenderer(window, -1, 0);

            var emulatorBootstrapper = new EmulatorBootstrapper();
            var romBytes = File.ReadAllBytes(args[0]);
            _emulator = emulatorBootstrapper.LoadRom(romBytes);
            var joypad = _emulator.Joypad;

            try
            {
                Emulate(renderer, joypad);
            }
            catch (Exception)
            {
                File.Delete("trace");
                var file = File.Open("trace", FileMode.CreateNew);
                var sw = new StreamWriter(file);
                foreach (var instruction in _emulator.Trace)
                {
                    sw.WriteLine("{0:x} {1:x}", instruction.Item1, instruction.Item2);
                }
                sw.Flush();
                sw.Close();
                throw;
            }

            SDL_DestroyRenderer(renderer);
            SDL_DestroyWindow(window);
            SDL_Quit();
        }

        private static void Emulate(IntPtr renderer, Joypad joypad)
        {
            var running = true;
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            const double cpuSpeed = (4194304) / 4.0;
            const double cyclesPerFrame = cpuSpeed / 60.0;
            var ticksPerMilliseconds = Stopwatch.Frequency / 1000;
            double cyclesUntilNextFrame = 0;
            long milliSeconds = 0;
            int cyclesPerMillisecond = (int)(cpuSpeed / 1000);
            long targetCycles = cyclesPerMillisecond;
            while (running)
            {
                while (_emulator.Cpu.Cycles < targetCycles)
                {
                    _emulator.Tick();
                }
                targetCycles += cyclesPerMillisecond;

                while (milliSeconds > stopwatch.ElapsedTicks)
                {
                    Thread.Sleep(0);
                }
                milliSeconds += ticksPerMilliseconds;

                if (_emulator.Cpu.Cycles > cyclesUntilNextFrame)
                {
                    Draw(renderer);
                    cyclesUntilNextFrame += cyclesPerFrame;
                }

                SDL_Event newEvent;
                SDL_PollEvent(out newEvent);

                switch (newEvent.type)
                {
                    case SDL_EventType.SDL_QUIT:
                        running = false;
                        break;
                    case SDL_EventType.SDL_KEYUP:
                        Up(joypad, KeyState.FromKeyEvent(newEvent.key));
                        break;
                    case SDL_EventType.SDL_KEYDOWN:
                        running = newEvent.key.keysym.scancode != SDL_Scancode.SDL_SCANCODE_ESCAPE;
                        Down(joypad, KeyState.FromKeyEvent(newEvent.key));
                        break;
                }
            }
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

        private static Emulator _emulator;
        private static void Draw(IntPtr renderer)
        {
            var surface = CreateSurface();
            var texture = SDL_CreateTextureFromSurface(renderer, surface);

            SDL_RenderCopy(renderer, texture, (IntPtr)null, (IntPtr)null);
            SDL_RenderPresent(renderer);
            SDL_DestroyTexture(texture);
            SDL_FreeSurface(surface);
        }

        private static IntPtr CreateSurface()
        {
            var buffer = new uint[160 * 144];
            for (var x = 0; x < 160; x++)
            {
                for (var y = 0; y < 144; y++)
                {
                    var color = _emulator.DisplayDataTransferService.FrameBuffer[y * 160 + x];
                    buffer[y * 160 + x] = GetColor(color);
                }
            }

            var gcHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            var address = gcHandle.AddrOfPinnedObject();

            var surface = SDL_CreateRGBSurfaceFrom(address, 160, 144, 32, 160 * 4, 0, 0, 0, 0x000000FF);
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
