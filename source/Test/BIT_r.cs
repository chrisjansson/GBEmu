using System.Collections.Generic;
using System.Linq;
using Xunit.Extensions;

namespace Test
{
    public class BIT_r : CpuTestBase
    {
        [Theory, PropertyData("Arguments")]
        public void Advances_counters(RegisterMapping register, int bit)
        {
            ExecutingCB(CreateOpCode(register, bit));

            AdvancedProgramCounter(2);
            AdvancedClock(2);
        }

        [Theory, PropertyData("Arguments")]
        public void Resets_subtract_and_sets_half_carry_flag(RegisterMapping register, int bit)
        {
            Flags(x => x.Subtract().ResetHalfCarry());

            ExecutingCB(CreateOpCode(register, bit));

            AssertFlags(x => x.ResetSubtract().SetHalfCarry());
        }

        [Theory, PropertyData("Arguments")]
        public void Sets_zero_when_bits_complement_is_zero(RegisterMapping register, int bit)
        {
            Flags(x => x.ResetZero());
            register.Set(Cpu, (byte) ~(1 << bit));

            ExecutingCB(CreateOpCode(register, bit));

            AssertFlags(x => x.SetZero());
        }

        [Theory, PropertyData("Arguments")]
        public void Resets_zero_when_bits_complement_is_one(RegisterMapping register, int bit)
        {
            Flags(x => x.Zero());
            register.Set(Cpu, (byte)(1 << bit));

            ExecutingCB(CreateOpCode(register, bit));

            AssertFlags(x => x.ResetZero());
        }

        private byte CreateOpCode(RegisterMapping register, int bit)
        {
            return (byte) (0x40 | register | (bit << 3));
        }

        public static IEnumerable<object[]> Arguments
        {
            get
            {
                return from r in RegisterMapping.GetAll()
                    from bit in Enumerable.Range(0, 8)
                    select new object[] {r, bit};
            }
        }
    }
}