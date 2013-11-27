using Xunit;

namespace Test
{
    public class OR_HL : CpuTestBase
    {
        [Fact]
        public void Advances_counters()
        {
            Execute(0xB6);

            AdvancedProgramCounter(1);
            AdvancedClock(2);
        }

        [Fact]
        public void Resets_flags()
        {
            Flags(x => x.HalfCarry().Subtract().Carry());

            Execute(0xb6);

            AssertFlags(x => x.ResetHalfCarry().ResetSubtract().ResetCarry());
        }

        [Fact]
        public void ORs_value_of_register_and_stores_register_result_in_A()
        {
            Cpu.A = 0x12;
            Cpu.H = 0xAB;
            Cpu.L = 0xCE;
            FakeMmu.SetByte(0xABCE, 0x48);
            Flags(x => x.Zero());

            Execute(0xb6);

            Assert.Equal(0x5A, Cpu.A);
            AssertFlags(x => x.ResetZero());
        }

        [Fact]
        public void Sets_zero_when_result_is_zero()
        {
            Flags(x => x.ResetZero());
            Cpu.A = 0;

            Execute(0xb6);
            
            AssertFlags(x => x.SetZero());
        }
    }
}