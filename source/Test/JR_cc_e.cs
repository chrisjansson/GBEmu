using Xunit;

namespace Test
{
    public class JR_cc_e : CpuTestBase
    {
        [Fact]
        public void Jumps_relative_to_program_counter_when_NZ()
        {
            Cpu.ProgramCounter = 0x480;
            Cpu.Z = 0;
            Cpu.Cycles = 48401;
            _fakeMmu.SetByte(0x481, 0xFA);

            Execute(0x20);

            Assert.Equal(0x47C, Cpu.ProgramCounter);
            Assert.Equal(48404, Cpu.Cycles);
        }

        [Fact]
        public void Does_not_jump_when_not_NZ()
        {
            Cpu.ProgramCounter = 0x480;
            Cpu.Z = 1;
            Cpu.Cycles = 2947;
            _fakeMmu.SetByte(0x481, 0xFA);

            Execute(0x20);

            Assert.Equal(0x482, Cpu.ProgramCounter);
            Assert.Equal(2949, Cpu.Cycles);
        }

        [Fact]
        public void Jumps_relative_to_program_counter_when_Z()
        {
            Cpu.ProgramCounter = 0x300;
            Cpu.Z = 1;
            Cpu.Cycles = 48401;
            _fakeMmu.SetByte(0x301, 0x03);

            Execute(0x28);

            Assert.Equal(0x305, Cpu.ProgramCounter);
            Assert.Equal(48404, Cpu.Cycles);
        }

        [Fact]
        public void Does_not_jump_when_not_Z()
        {
            Cpu.ProgramCounter = 0x480;
            Cpu.Z = 0;
            Cpu.Cycles = 2947;
            
            Execute(0x28);

            Assert.Equal(0x482, Cpu.ProgramCounter);
            Assert.Equal(2949, Cpu.Cycles);
        }

        [Fact]
        public void Does_not_jump_when_carry()
        {
            Flags(x => x.Carry());

            Execute(0x30);

            AdvancedProgramCounter(2);
            AdvancedClock(2);
        }

        [Fact]
        public void Jumps_relative_to_program_counter_when_not_carry()
        {
            Flags(x => x.ResetCarry());

            Execute(0x30, 0xFA);

            AdvancedProgramCounter(-4);
            AdvancedClock(3);
        }
    }
}
