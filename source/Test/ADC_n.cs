namespace Test
{
    public class ADC_n : ADCTestBase
    {
        protected override byte OpCode
        {
            get { return 0xCE; }
        }

        protected override int OpCodeLength
        {
            get { return 2; }
        }

        protected override void ArrangeArgument(byte argument)
        {
            FakeMmu.SetByte((ushort) (Cpu.ProgramCounter + 1), argument);
        }
    }
}