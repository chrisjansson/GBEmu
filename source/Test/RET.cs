using Core;
using Xunit;

namespace Test
{
    public abstract class RETConditionalTestBase : CpuTestBase
    {
        [Fact]
        public void Loads_program_counter_with_memory_pointer_to_by_sp()
        {
            SetTrueFlag();
            Cpu.SP = 9481;
            FakeMmu.Memory[9481] = 0x03;
            FakeMmu.Memory[9482] = 0x80;

            Cpu.Execute(OpCode);

            Assert.Equal(0x8003, Cpu.ProgramCounter);
            Assert.Equal(9483, Cpu.SP);
            AdvancedClock(5);
        }

        [Fact]
        public void Continues_after_instruction()
        {
            SetFalseFlag();
            Cpu.SP = 9481;

            Cpu.Execute(OpCode);

            AdvancedProgramCounter(1);
            AdvancedClock(2);
            Assert.Equal(9481, Cpu.SP);
        }

        protected abstract byte OpCode { get; }
        protected abstract void SetTrueFlag();
        protected abstract void SetFalseFlag();
    }

    public class RET_NC : RETConditionalTestBase
    {
        protected override byte OpCode
        {
            get { return 0xD0; }
        }

        protected override void SetTrueFlag()
        {
            Flags(x => x.ResetCarry());
        }

        protected override void SetFalseFlag()
        {
            Flags(x => x.Carry());
        }
    }

    public class RET_Z : RETConditionalTestBase
    {
        protected override byte OpCode
        {
            get { return 0xC8; }
        }

        protected override void SetTrueFlag()
        {
            Flags(x => x.Zero());
        }

        protected override void SetFalseFlag()
        {
            Flags(x => x.ResetZero());
        }
    }

    public class RET_NZ : RETConditionalTestBase
    {
        protected override byte OpCode
        {
            get { return 0xC0; }
        }

        protected override void SetTrueFlag()
        {
            Flags(x => x.ResetZero());
        }

        protected override void SetFalseFlag()
        {
            Flags(x => x.Zero());
        }
    }

    public class RET_C : RETConditionalTestBase
    {
        protected override byte OpCode
        {
            get { return 0xD8; }
        }

        protected override void SetTrueFlag()
        {
            Flags(x => x.Carry());
        }

        protected override void SetFalseFlag()
        {
            Flags(x => x.ResetCarry());
        }
    }

    public abstract class RETBase : CpuTestBase
    {
        [Fact]
        public void Loads_program_counter_with_memory_pointed_to_by_SP()
        {
            Cpu.Cycles = 12384;
            Cpu.SP = 9481;
            FakeMmu.Memory[9481] = 0x03;
            FakeMmu.Memory[9482] = 0x80;

            Cpu.Execute(OpCode);

            Assert.Equal(0x8003, Cpu.ProgramCounter);
            Assert.Equal(12388, Cpu.Cycles);
        }

        [Fact]
        public void Increases_sp()
        {
            Cpu.SP = 9481;

            Cpu.Execute(OpCode);

            Assert.Equal(9483, Cpu.SP);
        }

        protected abstract byte OpCode { get; }
    }

    public class RET : RETBase
    {
        protected override byte OpCode
        {
            get { return 0xC9; }
        }
    }

    public class RETI : RETBase
    {
        protected override byte OpCode
        {
            get { return 0xD9; }
        }

        [Fact]
        public void Enables_interrups()
        {
            Cpu.IME = false;

            Execute(OpCode);
            Assert.True(Cpu.IME);
        }
    }
}