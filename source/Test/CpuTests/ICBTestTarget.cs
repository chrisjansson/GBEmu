namespace Test.CpuTests
{
    public interface ICBTestTarget
    {
        void SetUp(CpuTestBase fixture);
        void ArrangeArgument(byte argument);
        byte OpCode { get; }
        byte Actual { get; }
        int InstructionLength { get; }
        int InstructionTime { get; }
    }
}