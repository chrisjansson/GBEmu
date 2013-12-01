using Core;
using Xunit;

namespace Test
{
    public class JP_nn : CpuTestBase
    {
        [Fact]
        public void Loads_nn_into_program_counter_lower_order_byte_first()
        {
            Cpu.ProgramCounter = 9876;
            const byte l = 123;
            const byte h = 234;
            FakeMmu.SetByte(9877, l);
            FakeMmu.SetByte(9878, h);

            Cpu.Execute(0xC3);

            Assert.Equal(60027, Cpu.ProgramCounter);
        }

        [Fact]
        public void Advances_clock()
        {
            Cpu.Cycles = 7931;

            Cpu.Execute(0xC3);

            Assert.Equal(Cpu.Cycles, 7935);
        }
    }
}