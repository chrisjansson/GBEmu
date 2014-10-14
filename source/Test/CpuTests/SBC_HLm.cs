using Ploeh.AutoFixture;

namespace Test.CpuTests
{
    public class SBC_HLm : SBCTestBase
    {
        protected override byte OpCode
        {
            get { return 0x9E; }
        }

        protected override byte InstructionLength
        {
            get { return 1; }
        }

        protected override void ArrangeArgument(byte argument)
        {
            var hl = Fixture.Create<ushort>();
            Set(RegisterPair.HL, hl);
            FakeMmu.Memory[hl] = argument;
        }
    }
}