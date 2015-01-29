using System;

namespace Core
{
    public class Emulator
    {
        public readonly RingBuffer<Tuple<ushort, byte>> Trace = new RingBuffer<Tuple<ushort, byte>>(50000);

        public IMmu Mmu { get; set; }
        public Cpu Cpu { get; set; }
        public Timer Timer { get; set; }
        public Display Display { get; set; }
        public Joypad Joypad { get; set; }
        public DisplayDataTransferService DisplayDataTransferService { get; set; }

        public void Tick()
        {
            var next = Cpu.ProgramCounter;
            var instruction = Mmu.GetByte(next);

            var old = Cpu.Cycles;
            Trace.Insert(new Tuple<ushort, byte>(Cpu.ProgramCounter, instruction));
            Cpu.Execute(instruction);
            var delta = Cpu.Cycles - old;
            for (var i = 0; i < delta; i++)
            {
                Display.Tick();
                Timer.Tick();
            }
        }
    }
}