using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class SRL_r : CBRegisterTestBase
    {
        [Theory, PropertyData("Registers")]
        public void Shifts_contents_right_sets_carry_and_resets_zerno(RegisterMapping register)
        {
            Flags(x => x.Zero().ResetCarry());
            register.Set(Cpu, 0x8F);

            ExecutingCB(CreateOpCode(register));

            Assert.Equal(0x47, register.Get(Cpu));
            AssertFlags(x => x.SetCarry().ResetZero());
        }

        [Theory, PropertyData("Registers")]
        public void Resets_carry_and_sets_zero(RegisterMapping register)
        {
            Flags(x => x.Carry().ResetZero());
            register.Set(Cpu, 0x00);

            ExecutingCB(CreateOpCode(register));

            AssertFlags(x => x.ResetCarry().SetZero());
        }

        [Theory, PropertyData("Registers")]
        public void Subtract_and_half_carry_flags_are_reset(RegisterMapping register)
        {
            Flags(_ => _.Subtract().HalfCarry());

            ExecutingCB(CreateOpCode(register));

            AssertFlags(x => x.ResetSubtract().ResetHalfCarry());
        }

        protected override byte CreateOpCode(RegisterMapping register)
        {
            return (byte)(0x38 | register);
        }
    }
}