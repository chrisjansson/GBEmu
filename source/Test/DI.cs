using System;
using Core;
using Xunit;

namespace Test
{
    public class DI : IUseFixture<CpuFixture>
    {
        private Cpu _cpu;

        [Fact]
        public void Disables_interrupts()
        {
            _cpu.Cycles = 93811;
            _cpu.ProgramCounter = 8248;

            _cpu.Execute(0xF3);

            Assert.Equal(93812, _cpu.Cycles);
            Assert.Equal(8249, _cpu.ProgramCounter);
        }

        [Fact]
        public void Reminder()
        {
            throw new Exception("What else should be tested here?");
        }

        public void SetFixture(CpuFixture data)
        {
            _cpu = data.Cpu;
        }
    }
}