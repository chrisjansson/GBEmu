﻿using System;
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
        private static MMU _mmu;
        private static Cpu _cpu;
        private static DisplayDataTransferService _displayDataTransferService;
        private static Display _display;
        private static MMUWithBootRom _mmuWithBootRom;
        private static Timer _timer;

        static void Main(string[] args)
        {
            SDL.SDL_Init(SDL.SDL_INIT_VIDEO);

            var window = SDL.SDL_CreateWindow("An SDL Window", 100, 100, 640, 576, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);
            var renderer = SDL.SDL_CreateRenderer(window, -1, 0);

            _mmu = new MMU();

            var fileStream = File.OpenRead(args[0]);
            var rom = new byte[256];
            fileStream.Read(rom, 0, 256);
            _mmuWithBootRom = new MMUWithBootRom(rom, _mmu);
            _cpu = new Cpu(_mmuWithBootRom);
            _displayDataTransferService = new DisplayDataTransferService(_mmuWithBootRom);
            _display = new Display(_displayDataTransferService);
            _mmu.Display = _display;
            _mmu.Cpu = _cpu;
            _timer = new Timer(_mmu);
            _mmu.Timer = _timer;

            var openRead = File.OpenRead(args[1]);
            ushort inPosition = 0;
            while (openRead.Position != openRead.Length)
            {
                byte readByte = (byte) openRead.ReadByte();
                _mmu.SetByte(inPosition, readByte);
                inPosition++;
            }

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

                const double cycleTime = 1 / 4000000.0;
                const double frameTime = 1 / 60.0;
                const double cyclesPerFrame = (frameTime/cycleTime) * 0.8;

                var target = (long)(_cpu.Cycles + cyclesPerFrame);
                while (_cpu.Cycles <= target)
                {
                    EmulateCycle();
                }

                Draw(renderer);
            }

            SDL.SDL_DestroyRenderer(renderer);
            SDL.SDL_DestroyWindow(window);
            SDL.SDL_Quit();
        }

        private static void EmulateCycle()
        {
            var next = _cpu.ProgramCounter;
            var instruction = _mmuWithBootRom.GetByte(next);

            var old = _cpu.Cycles;
            _cpu.Execute(instruction);
            var delta = _cpu.Cycles - old;
            for (var i = 0; i < delta; i++)
            {
                _display.Tick();
                _timer.Tick();
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
                    break;
                case 1:
                    return 0;
                    break;
                case 2:
                    return 0;
                    break;
                case 3:
                    return 0;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
