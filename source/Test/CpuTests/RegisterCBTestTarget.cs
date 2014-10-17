namespace Test.CpuTests
{
    public class RegisterCBTestTarget : ICBTestTarget
    {
        private CpuTestBase _fixture;
        private readonly RegisterMapping _register;

        public RegisterCBTestTarget(RegisterMapping register)
        {
            _register = register;
        }

        public void SetUp(CpuTestBase fixture)
        {
            _fixture = fixture;
        }

        public void ArrangeArgument(byte argument)
        {
            _register.Set(_fixture.Cpu, argument);
        }

        public byte OpCode
        {
            get { return _register; }
        }

        public byte Actual
        {
            get { return _register.Get(_fixture.Cpu); }
        }

        public int InstructionLength
        {
            get { return 2; }
        }

        public int InstructionTime
        {
            get { return 2; }
        }

        public override string ToString()
        {
            return _register.ToString();
        }
    }
}