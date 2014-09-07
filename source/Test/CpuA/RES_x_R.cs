using Xunit;
using Xunit.Extensions;

namespace Test.CpuA
{
    public abstract class RES_x_R : CBRegisterTestBase
    {
        [Theory, PropertyData("Registers")]
        public void Resets_bit(RegisterMapping register)
        {
            Set(register, 0xFF);

            ExecutingCB(CreateOpCode(register));

            var expected = 0xFF & ~(1 << Bit);
            Assert.Equal(expected, register.Get(Cpu));
        }

        protected override byte CreateOpCode(RegisterMapping register)
        {
            return (byte)(0x80 | register | (Bit << 3));
        }

        protected abstract byte Bit { get; }

        public class RES_0_R : RES_x_R
        {
            protected override byte Bit
            {
                get { return 0; }
            }
        }
    }
}