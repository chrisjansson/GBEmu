using Xunit;
using Xunit.Extensions;

namespace Test.CpuA
{
    public class SBC_r : RegisterTestBase
    {
        protected override byte CreateOpCode(RegisterMapping register)
        {
            return (byte) (0x98 | register);
        }

        [Theory, PropertyData("Registers")]
        public void Sets_subtract(RegisterMapping register)
        {
            Flags(x => x.ResetSubtract());
            
            Execute(CreateOpCode(register));

            AssertFlags(x => x.SetSubtract());
        }

        [Theory, PropertyData("RegistersExceptA")]
        public void Sets_zero(RegisterMapping register)
        {
            Flags(x => x.ResetZero().Carry().HalfCarry());
            Cpu.A = 0x3B;
            Set(register, 0x3A);

            Execute(CreateOpCode(register));

            Assert.Equal(0x00, Cpu.A);
            AssertFlags(x => x.SetZero().ResetCarry().ResetHalfCarry());
        }

        [Theory, PropertyData("RegistersExceptA")]
        public void Sets_carry(RegisterMapping register)
        {
            Flags(x => x.Carry().Zero().HalfCarry());
            Cpu.A = 0x3B;
            Set(register, 0x40);

            Execute(CreateOpCode(register));

            Assert.Equal(0xFA, Cpu.A);
            AssertFlags(x => x.SetCarry().ResetHalfCarry().ResetZero());
        }

        [Theory, PropertyData("RegistersExceptA")]
        public void Sets_half_carry(RegisterMapping register)
        {
            Flags(x => x.Carry().Zero().ResetHalfCarry());
            Cpu.A = 0x15;
            Set(register, 0x05);

            Execute(CreateOpCode(register));

            Assert.Equal(0x0F, Cpu.A);
            AssertFlags(x => x.SetHalfCarry().ResetCarry().ResetZero());
        }
    }
}