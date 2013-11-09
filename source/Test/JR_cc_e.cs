using Core;
using Xunit;

namespace Test
{
    public class JR_cc_e : IUseFixture<CpuFixture>
    {
        private Cpu _cpu;
        private FakeMmu _fakeMmu;

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

        public void SetFixture(CpuFixture data)
        {
            _cpu = data.Cpu;
            _fakeMmu = data.FakeMmu;
        }
    }
}
