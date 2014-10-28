using Xunit;

namespace Test.CpuTests
{
    public class DAA : CpuTestBase
    {
        private const byte OpCode = 0x27;

        [Fact]
        public void Advances_counters()
        {
            Execute(OpCode);

            AdvancedProgramCounter(1);
            AdvancedClock(1);
        }

        [Fact]
        public void Resets_half_carry()
        {
            Flags(x => x.HalfCarry());

            Execute(OpCode);

            AssertFlags(x => x.ResetHalfCarry());
        }

        public class DAA_Add : CpuTestBase
        {
            [Fact]
            public void Corrects_0x60_when_A_is_greater_than_0x99()
            {
                Flags(x => x.ResetCarry().ResetHalfCarry());
                Cpu.N = 0;
                Cpu.A = 0xA0;

                Execute(OpCode);

                Assert.Equal(0x00, Cpu.A);
                AssertFlags(x => x.SetCarry());
            }

            [Fact]
            public void Corrects_0x60_when_carry_is_set()
            {
                Flags(x => x.Carry().ResetHalfCarry());
                Cpu.N = 0;
                Cpu.A = 0;

                Execute(OpCode);

                Assert.Equal(0x60, Cpu.A);
                AssertFlags(x => x.SetCarry());
            }

            [Fact]
            public void Clears_carry_when_A_is_less_than_0x99()
            {
                Flags(x => x.ResetCarry().ResetHalfCarry());
                Cpu.N = 0;
                Cpu.A = 0;

                Execute(OpCode);

                Assert.Equal(0, Cpu.A);
                AssertFlags(x => x.ResetCarry());
            }

            [Fact]
            public void Corrects_0x06_when_lower_4_are_greater_than_0x09()
            {
                Flags(x => x.ResetHalfCarry().ResetCarry());
                Cpu.N = 0;
                Cpu.A = 0x0A;

                Execute(OpCode);

                Assert.Equal(0x10, Cpu.A);
            }

            [Fact]
            public void Corrects_0x06_when_half_carry_is_set()
            {
                Flags(x => x.ResetCarry().HalfCarry());
                Cpu.N = 0;
                Cpu.A = 0x00;

                Execute(OpCode);

                Assert.Equal(0x06, Cpu.A);
            }

            [Fact]
            public void Corrects_0x66_when_A_is_greater_than_0x99_and_lower_four_bits_are_greater_than_0x09()
            {
                Flags(x => x.ResetCarry().ResetHalfCarry());
                Cpu.N = 0;
                Cpu.A = 0xAA;

                Execute(OpCode);

                Assert.Equal(0x10, Cpu.A);
                AssertFlags(x => x.SetCarry());
            }
        }

        public class DAA_Subtract : CpuTestBase
        {
            [Fact]
            public void Corrects_0x60_when_A_is_greater_than_0x99()
            {
                Flags(x => x.ResetCarry().ResetHalfCarry());
                Cpu.N = 1;
                Cpu.A = 0xA0;

                Execute(OpCode);

                Assert.Equal(0x40, Cpu.A);
                AssertFlags(x => x.SetCarry());
            }

            [Fact]
            public void Corrects_0x60_when_carry_is_set()
            {
                Flags(x => x.Carry().ResetHalfCarry());
                Cpu.N = 1;
                Cpu.A = 0;

                Execute(OpCode);

                Assert.Equal(0xA0, Cpu.A);
                AssertFlags(x => x.SetCarry());
            }

            [Fact]
            public void Clears_carry_when_A_is_less_than_0x99()
            {
                Flags(x => x.ResetCarry().ResetHalfCarry());
                Cpu.N = 1;
                Cpu.A = 0;

                Execute(OpCode);

                Assert.Equal(0, Cpu.A);
                AssertFlags(x => x.ResetCarry());
            }

            [Fact]
            public void Corrects_0x06_when_lower_4_are_greater_than_0x09()
            {
                Flags(x => x.ResetHalfCarry().ResetCarry());
                Cpu.N = 1;
                Cpu.A = 0x0A;

                Execute(OpCode);

                Assert.Equal(0x4, Cpu.A);
            }

            [Fact]
            public void Corrects_0x06_when_half_carry_is_set()
            {
                Flags(x => x.ResetCarry().HalfCarry());
                Cpu.N = 1;
                Cpu.A = 0x00;

                Execute(OpCode);

                Assert.Equal(0xFA, Cpu.A);
            }
        }
    }
}