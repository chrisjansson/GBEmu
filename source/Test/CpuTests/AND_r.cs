using Xunit;
using Xunit.Extensions;

namespace Test.CpuTests
{
    public class AND_r : RegisterTestBase
    {
        protected override byte CreateOpCode(RegisterMapping register)
        {
            return (byte) (0xA0 | register);
        }

        [Theory, PropertyData("Registers")]
        public void Resets_subtract(RegisterMapping register)
        {
            Flags(x => x.Subtract());

            Execute(CreateOpCode(register));

            AssertFlags(x => x.ResetSubtract());
        }

        [Theory, PropertyData("Registers")]
        public void Sets_half_carry(RegisterMapping register)
        {
            Flags(x => x.ResetHalfCarry());

            Execute(CreateOpCode(register));

            AssertFlags(x => x.SetHalfCarry());
        }


        [Theory, PropertyData("Registers")]
        public void Resets_carry(RegisterMapping register)
        {
            Flags(x => x.Carry());

            Execute(CreateOpCode(register));

            AssertFlags(x => x.ResetCarry());
        }

        [Theory, PropertyData("Registers")]
        public void Sets_zero_when_result_is_zero(RegisterMapping register)
        {
            Flags(x => x.ResetZero());
            Cpu.A = 0;
            Set(register, 0);

            Execute(CreateOpCode(register));

            AssertFlags(x => x.SetZero());
        }

        [Theory, PropertyData("RegistersExceptA")]
        public void Ands_A_and_r(RegisterMapping register)
        {
            Flags(x => x.Zero());
            Cpu.A = 0x5A;
            Set(register, 0x3F);

            Execute(CreateOpCode(register));

            Assert.Equal(0x1A, Cpu.A);
            AssertFlags(x => x.ResetZero());
        }
    }
}