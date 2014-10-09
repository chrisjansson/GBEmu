using Xunit;

namespace Test.CpuA.Interrupts
{
    public class InterruptPriorityTests : CpuTestBase
    {
        public InterruptPriorityTests()
        {
            Cpu.IME = true;
            Cpu.IE = 0xFF;
        }

        [Fact]
        public void V_blank_has_highest_priority()
        {
            Cpu.IF = 0xFF; //Request all interrupts

            Execute(0x00);

            Assert.Equal(Interrupt.VBlank.InterruptVector, Cpu.ProgramCounter);
        }

        [Fact]
        public void LCD_stat_comes_after_v_blank()
        {
            Cpu.IF = 0xFE;

            Execute(0x00);

            Assert.Equal(Interrupt.LCDStat.InterruptVector, Cpu.ProgramCounter);
        }

        [Fact]
        public void Timer_comes_after_LCD_stat()
        {
            Cpu.IF = 0xFC;

            Execute(0x00);

            Assert.Equal(Interrupt.Timer.InterruptVector, Cpu.ProgramCounter);
        }

        [Fact]
        public void Serial_comes_after_timer()
        {
            Cpu.IF = 0xF8;

            Execute(0x00);

            Assert.Equal(Interrupt.Serial.InterruptVector, Cpu.ProgramCounter);
        }

        [Fact]
        public void Joypad_comes_after_serial()
        {
            Cpu.IF = 0xF0;

            Execute(0x00);

            Assert.Equal(Interrupt.JoyPad.InterruptVector, Cpu.ProgramCounter);
        }
    }
}