using Xunit;
using Xunit.Extensions;

namespace Test.CpuA
{
    public class RRC_r : CBRegisterTestBase
    {
        protected override byte CreateOpCode(RegisterMapping register)
        {
            return (byte) (0x08 | register);
        }

        [Theory, PropertyData("Registers")]
        public void Rotates_register_right(RegisterMapping register)
        {
            Flags(x => x.ResetCarry().Zero());
            Set(register, 0x11);

            ExecutingCB(CreateOpCode(register));

            Assert.Equal(0x88, register.Get(Cpu));
            AssertFlags(x => x.SetCarry().ResetZero());
        }

        [Theory, PropertyData("Registers")]
        public void Rotates_register_right_set_zero_reset_carry(RegisterMapping register)
        {
            Flags(x => x.Carry().ResetZero());
            Set(register, 0x00);

            ExecutingCB(CreateOpCode(register));

            Assert.Equal(0x00, register.Get(Cpu));
            AssertFlags(x => x.ResetCarry().SetZero());
        }

        [Theory, PropertyData("Registers")]
        public void Resets_subtract_and_half_carry(RegisterMapping register)
        {
            Flags(x => x.Subtract().HalfCarry());

            ExecutingCB(CreateOpCode(register));

            AssertFlags(x => x.ResetSubtract().ResetHalfCarry());
        }
    }
}