using Ploeh.AutoFixture;

namespace Test
{
    public class ADC_A_HLm : ADCTestBase
    {
        protected override byte OpCode
        {
            get { return 0x8E; }
        }

        protected override int OpCodeLength
        {
            get { return 1; }
        }

        protected override void ArrangeArgument(byte argument)
        {
            var hl = Fixture.Create<ushort>();
            RegisterPair.HL.Set(Cpu, (byte) (hl >> 8), (byte) hl);
            FakeMmu.SetByte(hl, argument);
        }
    }
}