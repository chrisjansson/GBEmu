namespace Test.CpuTests
{
    public class SBC_n : SBCTestBase
    {
        protected override byte OpCode
        {
            get { return 0xDE; }
        }

        protected override byte InstructionLength
        {
            get { return 2; }
        }

        protected override void ArrangeArgument(byte argument)
        {
            FakeMmu.Memory[Cpu.ProgramCounter + 1] = argument;
        }
    }
}