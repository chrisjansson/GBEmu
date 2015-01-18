using System;

namespace Core
{
    public class Emulator
    {
        public IMmu Mmu { get; set; }
        public Cpu Cpu { get; set; }
        public Timer Timer { get; set; }
        public Display Display { get; set; }
        public Joypad Joypad { get; set; }
        public DisplayDataTransferService DisplayDataTransferService { get; set; }
    }

    public class EmulatorBootstrapper
    {
        public Emulator LoadRom(byte[] rom)
        {
            const int programCounterAfterInitialization = 0x100;

            var headerBytes = new byte[0x4F];
            Array.Copy(rom, 0x100, headerBytes, 0, headerBytes.Length);
            var header = new CartridgeHeaderParser().Parse(headerBytes);
            var mbc = SelectMBC(header, rom);

            var mmu = new MMU(mbc);
            var joyPad = new Joypad(mmu);
            mmu.Joypad = joyPad;
            var timer = new Timer(mmu);
            mmu.Timer = timer;
            var displayDataTransferService = new DisplayDataTransferService(mmu, new SpriteRenderer(mmu));
            var display = new Display(mmu, displayDataTransferService);
            mmu.Display = display;
            var cpu = new Cpu(mmu)
            {
                ProgramCounter = programCounterAfterInitialization,
                A = 0x01,
                F = 0xB0,
                B = 0x00,
                C = 0x13,
                D = 0x00,
                E = 0xD8,
                H = 0x01,
                L = 0x4D,
                SP = 0xFFFE,
            };
            mmu.Cpu = cpu;

            mmu.SetByte(0xFF10, 0x80);
            mmu.SetByte(0xFF11, 0xBF);
            mmu.SetByte(0xFF12, 0xF3);
            mmu.SetByte(0xFF14, 0xBF);
            mmu.SetByte(0xFF16, 0x3F);
            mmu.SetByte(0xFF19, 0xBF);
            mmu.SetByte(0xFF1A, 0x7F);
            mmu.SetByte(0xFF1B, 0xFF);
            mmu.SetByte(0xFF1C, 0x9F);
            mmu.SetByte(0xFF1E, 0xBF);
            mmu.SetByte(0xFF20, 0xFF);
            mmu.SetByte(0xFF23, 0xBF);
            mmu.SetByte(0xFF24, 0x77);
            mmu.SetByte(0xFF25, 0xF3);
            mmu.SetByte(0xFF26, 0xF1);
            mmu.SetByte(0xFF40, 0x91);
            mmu.SetByte(0xFF47, 0xFC);
            mmu.SetByte(0xFF48, 0xFF);
            mmu.SetByte(0xFF49, 0xFF);

            return new Emulator
            {
                Cpu = cpu,
                Mmu = mmu,
                Timer = timer,
                Display = display,
                DisplayDataTransferService = displayDataTransferService,
                Joypad = joyPad,
            };
        }

        private IMBC SelectMBC(CartridgeHeader header, byte[] rom)
        {
            switch (header.MBC)
            {
                case CartridgeHeader.CartridgeTypeEnum.None:
                    return new NoMBC(rom);
                case CartridgeHeader.CartridgeTypeEnum.MBC1:
                    return new MBC1(rom);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}