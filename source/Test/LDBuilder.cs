namespace Test
{
    public class LDBuilder
    {
        private RegisterMapping _from = RegisterMapping.A;

        public LDBuilder From(RegisterMapping registerMapping)
        {
            _from = registerMapping;
            return this;
        }

        public byte To(RegisterMapping registerMapping)
        {
            var to = registerMapping << 3;
            return (byte)(0x40 | _from | to);
        }
    }
}