using Xunit;
using Xunit.Extensions;

namespace Test.CpuTests
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
        
        [Fact]
        public void Subtracts_a_from_a()
        {
            Flags(x => x.Carry().Zero().ResetHalfCarry());
            Cpu.A = 0x15;

            Execute(CreateOpCode(RegisterMapping.A));

            Assert.Equal(0xFF, Cpu.A);
            AssertFlags(x => x.SetHalfCarry().SetCarry().ResetZero());
        }

        [Fact]
        public void Subtracts_a_from_a_2()
        {
            Flags(x => x.ResetCarry().ResetZero().HalfCarry());
            Cpu.A = 0x15;

            Execute(CreateOpCode(RegisterMapping.A));

            Assert.Equal(0x00, Cpu.A);
            AssertFlags(x => x.ResetHalfCarry().ResetCarry().SetZero());
        }
    }
}