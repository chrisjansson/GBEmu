using System;
using Core;
using Xunit;

namespace Test
{
    public class CP_n
    {
        private Cpu _cpu;
        private FakeMmu _fakeMmu;

        public CP_n()
        {
            var cpuFixture = new CpuFixture();
            _cpu = cpuFixture.Cpu;
            _fakeMmu = cpuFixture.FakeMmu;
        }

        [Fact]
        public void Program_counter_cycles_sets_N()
        {
            _cpu.N = 0;
            _cpu.ProgramCounter = 9281;
            _cpu.Cycles = 81123;

            _cpu.Execute(0xFE);

            Assert.Equal(1, _cpu.N);
            Assert.Equal(9283, _cpu.ProgramCounter);
            Assert.Equal(81125, _cpu.Cycles);
        }

        [Fact]
        public void Sets_z_when_A_is_equal_to_n()
        {
            _cpu.A = 0xAB;
            _cpu.ProgramCounter = 9281;
            _fakeMmu.SetByte(9282, 0xAB);

            _cpu.Execute(0xFE);

            Assert.Equal(1, _cpu.Z);
        }

        [Fact]
        public void Resets_z_when_A_is_not_equal_to_n()
        {
            _cpu.Z = 1;
            _cpu.A = 0x34;
            _cpu.ProgramCounter = 3621;
            _fakeMmu.SetByte(3622, 0xAB);

            _cpu.Execute(0xFE);

            Assert.Equal(0, _cpu.Z);
        }

        [Fact]
        public void C_and_half_carry()
        {
            //A - n is the order of subtraction
            throw new NotImplementedException();
        }
    }
}