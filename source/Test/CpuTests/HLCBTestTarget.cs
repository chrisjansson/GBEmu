using Ploeh.AutoFixture;

namespace Test.CpuTests
{
    public class HLCBTestTarget : ICBTestTarget
    {
        private CpuTestBase _fixture;
        private ushort _hl;

        public void SetUp(CpuTestBase fixture)
        {
            _fixture = fixture;
            _hl = _fixture.Fixture.Create<ushort>();
        }

        public void ArrangeArgument(byte argument)
        {
            _fixture.FakeMmu.Memory[_hl] = argument;
            _fixture.Set(RegisterPair.HL, _hl);
        }

        public byte OpCode
        {
            get { return 0x06; }
        }

        public byte Actual
        {
            get { return _fixture.FakeMmu.Memory[_hl]; }
        }

        public int InstructionLength
        {
            get { return 2; }
        }

        public int InstructionTime
        {
            get { return 4; }
        }
    }
}