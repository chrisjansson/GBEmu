using Xunit;
using Xunit.Extensions;

namespace Test.CpuTests
{
    public class SRA_r : CBRegisterTestBase
    {
        protected override byte CreateOpCode(RegisterMapping register)
        {
            return (byte)(0x28 | register);
        }

        [Theory, PropertyData("Registers")]
        public void Resets_half_carry_and_subtract(RegisterMapping register)
        {
            Flags(x => x.HalfCarry().Subtract());

            ExecutingCB(CreateOpCode(register));

            AssertFlags(x => x.ResetHalfCarry().ResetSubtract());
        }

        [Theory, PropertyData("Registers")]
        public void Shifts_content_right_into_carry(RegisterMapping register)
        {
            Flags(x => x.ResetCarry().Zero());
            Set(register, 0x81);

            ExecutingCB(CreateOpCode(register));

            Assert.Equal(0xC0, register.Get(Cpu));
            AssertFlags(x => x.SetCarry().ResetZero());
        }

        [Theory, PropertyData("Registers")]
        public void Shifts_content_right_zero(RegisterMapping register)
        {
            Flags(x => x.Carry().Zero());
            Set(register, 0x00);

            ExecutingCB(CreateOpCode(register));

            Assert.Equal(0x00, register.Get(Cpu));
            AssertFlags(x => x.ResetCarry().SetZero());
        }
    }
}