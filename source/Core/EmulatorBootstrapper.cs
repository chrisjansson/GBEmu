namespace Core
{
    public class Emulator
    {
        public IMmu Mmu { get; set; }
        public Cpu Cpu { get; set; }
    }

    public class EmulatorBootstrapper
    {
        public Emulator LoadRom(byte[] rom)
        {
            const int programCounterAfterInitialization = 0x100;


            var mmu = new MMU();
            var timer = new Timer(mmu);
            mmu.Timer = timer;
            var display = new Display(mmu, null);
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
            };
        }
    }
}