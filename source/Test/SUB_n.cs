namespace Test
{
    public class SUB_n : SUBTestBase
    {
        protected override byte OpCode
        {
            get { return 0xD6; }
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