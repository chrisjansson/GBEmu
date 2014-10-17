namespace Test.CpuTests
{
    public abstract class RegisterCBTestTargetBase : ICBTestTarget
    {
        protected RegisterCBTestTargetBase(RegisterMapping register)
        {
            Register = register;
        }


        public void SetUp(CpuTestBase fixture)
        {
            Fixture = fixture;
        }

        public void ArrangeArgument(byte argument)
        {
            Fixture.Set(Register, argument);
        }

        public abstract byte OpCode { get; }
        protected RegisterMapping Register { get; private set; }
        protected CpuTestBase Fixture { get; private set; }

        public byte Actual
        {
            get { return Register.Get(Fixture.Cpu); }
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
            return Register.ToString();
        }
    }
}