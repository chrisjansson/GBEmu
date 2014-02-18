namespace Test
{
    public class CP_n : CPTestBase
    {
        protected override void ExecuteCompareATo(byte value)
        {
            Execute(CreateOpCode(), value);
        }

        private byte CreateOpCode()
        {
            return 0xFE;
        }
    }
}