namespace Test.CpuTests
{
    public class RegisterCBTestTarget : RegisterCBTestTargetBase
    {
        public RegisterCBTestTarget(RegisterMapping register)
            : base(register) { }

        public override byte OpCode
        {
            get { return Register; }
        }

    }
}