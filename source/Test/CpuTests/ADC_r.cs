using Xunit;
using Xunit.Extensions;

namespace Test.CpuTests
{
    public class ADC_r : RegisterTestBase
    {
        [Theory, PropertyData("Registers")]
        public void Resets_subtract(RegisterMapping register)
        {
            Flags(x => x.Subtract());
            
            Execute(CreateOpCode(register));

            AssertFlags(x => x.ResetSubtract());
        }

        [Theory, PropertyData("RegistersExceptA")]
        public void Adds_register_to_a_sets_half_carry(RegisterMapping register)
        {
            Flags(x => x.Zero().Carry().ResetHalfCarry());
            Cpu.A = 0xE1;
            Set(register, 0x0E);

            Execute(CreateOpCode(register));

            Assert.Equal(0xF0, Cpu.A);
            AssertFlags(x => x.ResetZero().ResetCarry().SetHalfCarry());
        }

        [Theory, PropertyData("RegistersExceptA")]
        public void Adds_register_to_a_sets_carry(RegisterMapping register)
        {
            Flags(x => x.Zero().Carry().ResetHalfCarry());
            Cpu.A = 0xE1;
            Set(register, 0x3B);

            Execute(CreateOpCode(register));

            Assert.Equal(0x1D, Cpu.A);
            AssertFlags(x => x.ResetZero().SetCarry().ResetHalfCarry());
        }

        [Theory, PropertyData("RegistersExceptA")]
        public void Adds_register_to_a_sets_zero(RegisterMapping register)
        {
            Flags(x => x.ResetZero().Carry().ResetHalfCarry());
            Cpu.A = 0xE1;
            Set(register, 0x1E);

            Execute(CreateOpCode(register));

            Assert.Equal(0x00, Cpu.A);
            AssertFlags(x => x.SetZero().SetCarry().SetHalfCarry());
        }

        [Fact]
        public void Adds_a_to_a()
        {
            Flags(x => x.Carry());
            Cpu.A = 0x12;

            Execute(CreateOpCode(RegisterMapping.A));

            Assert.Equal(0x25, Cpu.A);
        }

        protected override byte CreateOpCode(RegisterMapping register)
        {
            return (byte) (0x88 | register);
        }
    }
}