using Ploeh.AutoFixture;

namespace Test
{
    public class SUB_HLm : SUBTestBase
    {
        protected override byte OpCode
        {
            get { return 0x96; }
        }

        protected override byte InstructionLength
        {
            get { return 1; }
        }

        protected override void ArrangeArgument(byte argument)
        {
            var hl = Fixture.Create<ushort>();
            RegisterPair.HL.Set(Cpu, (byte) (hl >> 8), (byte) hl);
            FakeMmu.Memory[hl] = argument;
        }
    }
}