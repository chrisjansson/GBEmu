namespace Test.CpuTests
{
    public class AND_n : ANDTestTempate
    {
        protected override byte OpCode
        {
            get { return 0xE6; }
        }

        protected override int InstructionLength
        {
            get { return 2; }
        }

        protected override void ArrangeArgument(byte i)
        {
            FakeMmu.Memory[Cpu.ProgramCounter + 1] = i;
        }
    }
}