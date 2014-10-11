using Xunit;
using Xunit.Extensions;

namespace Test.CpuTests
{
    public class SWAP_r : CBRegisterTestBase
    {
        protected override byte CreateOpCode(RegisterMapping register)
        {
            return (byte) (0x30 | register);
        }

        [Theory, PropertyData("Registers")]
        public void Resets_subtract_carry_and_half_carry(RegisterMapping register)
        {
            Flags(x => x.Subtract().Carry().HalfCarry());

            ExecutingCB(CreateOpCode(register));

            AssertFlags(x => x.ResetSubtract().ResetCarry().ResetHalfCarry());
        }

        [Theory, PropertyData("Registers")]
        public void Swaps_uppper_nibble_with_lower_nibble(RegisterMapping register)
        {
            Flags(x => x.Zero());
            register.Set(Cpu, 0xCE);

            ExecutingCB(CreateOpCode(register));

            Assert.Equal(0xEC, register.Get(Cpu));
            AssertFlags(x => x.ResetZero());
        }

        [Theory, PropertyData("Registers")]
        public void Sets_zero_when_value_is_zero(RegisterMapping register)
        {
            Flags(x => x.ResetZero());
            register.Set(Cpu, 0);

            ExecutingCB(CreateOpCode(register));

            AssertFlags(x => x.SetZero());
        }
    }
}