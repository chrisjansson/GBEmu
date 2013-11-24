using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class RR_r : CBRegisterTestBase
    {
        protected override byte CreateOpCode(RegisterMapping register)
        {
            return (byte) (0x18 | register);
        }

        [Theory, PropertyData("Registers")]
        public void Rotates_the_contents_right_rotating_in_carry(RegisterMapping register)
        {
            Flags(x => x.ResetCarry());
            register.Set(Cpu, 0xDD);

            ExecutingCB(CreateOpCode(register));

            Assert.Equal(0x6E, register.Get(Cpu));
            AssertFlags(x => x.SetCarry());
        }

        [Theory, PropertyData("Registers")]
        public void Rotates_the_contents_right_and_resets_carry(RegisterMapping register)
        {
            Flags(x => x.Carry().Zero());
            register.Set(Cpu, 0x00);

            ExecutingCB(CreateOpCode(register));

            Assert.Equal(0x80, register.Get(Cpu));
            AssertFlags(x => x.ResetZero().ResetCarry());
        }

        [Theory, PropertyData("Registers")]
        public void Sets_zero_when_result_is_zero(RegisterMapping registerMapping)
        {
            Flags(x => x.ResetZero().ResetCarry());
            registerMapping.Set(Cpu, 0x00);

            ExecutingCB(CreateOpCode(registerMapping));

            AssertFlags(x => x.SetZero());
        }

        [Theory, PropertyData("Registers")]
        public void Resets_half_carry_and_subtract(RegisterMapping register)
        {
            Flags(x => x.HalfCarry().Subtract());

            ExecutingCB(CreateOpCode(register));

            AssertFlags(x => x.ResetHalfCarry().ResetSubtract());
        }
    }
}