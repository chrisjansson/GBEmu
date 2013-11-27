﻿using Core;
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
            Flags(x => x.Carry());
        }

        protected override void SetFalseFlag()
        {
            Flags(x => x.ResetCarry());
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

    public class RET : IUseFixture<CpuFixture>
    {
        private Cpu _cpu;
        private FakeMmu _fakeMmu;

        [Fact]
        public void Loads_program_counter_with_memory_pointed_to_by_SP()
        {
            _cpu.Cycles = 12384;
            _cpu.SP = 9481;
            _fakeMmu.Memory[9481] = 0x03;
            _fakeMmu.Memory[9482] = 0x80;

            _cpu.Execute(0xC9);

            Assert.Equal(0x8003, _cpu.ProgramCounter);
            Assert.Equal(12388, _cpu.Cycles);
        }

        [Fact]
        public void Increases_sp()
        {
            _cpu.SP = 9481;

            _cpu.Execute(0xC9);

            Assert.Equal(9483, _cpu.SP);
        }

        public void SetFixture(CpuFixture data)
        {
            _cpu = data.Cpu;
            _fakeMmu = data.FakeMmu;
        }
    }
}