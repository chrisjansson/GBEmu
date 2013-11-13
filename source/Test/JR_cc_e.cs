using Core;
using Xunit;

namespace Test
{
    public class JR_cc_e
    {
        private Cpu _cpu;
        private FakeMmu _fakeMmu;

        public JR_cc_e()
        {
            var cpuFixture = new CpuFixture();
            _cpu = cpuFixture.Cpu;
            _fakeMmu = cpuFixture.FakeMmu;
        }

        [Fact]
        public void Jumps_relative_to_program_counter_when_NZ()
        {
            _cpu.ProgramCounter = 0x480;
            _cpu.Z = 0;
            _cpu.Cycles = 48401;
            _fakeMmu.SetByte(0x481, 0xFA);

            _cpu.Execute(0x20);

            Assert.Equal(0x47C, _cpu.ProgramCounter);
            Assert.Equal(48404, _cpu.Cycles);
        }

        [Fact]
        public void Does_not_jump_when_not_NZ()
        {
            _cpu.ProgramCounter = 0x480;
            _cpu.Z = 1;
            _cpu.Cycles = 2947;
            _fakeMmu.SetByte(0x481, 0xFA);

            _cpu.Execute(0x20);

            Assert.Equal(0x482, _cpu.ProgramCounter);
            Assert.Equal(2949, _cpu.Cycles);
        }

        [Fact]
        public void Jumps_relative_to_program_counter_when_Z()
        {
            _cpu.ProgramCounter = 0x300;
            _cpu.Z = 1;
            _cpu.Cycles = 48401;
            _fakeMmu.SetByte(0x301, 0x03);

            _cpu.Execute(0x28);

            Assert.Equal(0x305, _cpu.ProgramCounter);
            Assert.Equal(48404, _cpu.Cycles);
        }

        [Fact]
        public void Does_not_jump_when_not_Z()
        {
            _cpu.ProgramCounter = 0x480;
            _cpu.Z = 0;
            _cpu.Cycles = 2947;
            _fakeMmu.SetByte(0x481, 0xFA);

            _cpu.Execute(0x28);

            Assert.Equal(0x482, _cpu.ProgramCounter);
            Assert.Equal(2949, _cpu.Cycles);
        }
    }
}
