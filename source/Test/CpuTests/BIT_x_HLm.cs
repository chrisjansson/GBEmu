using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Xunit.Extensions;

namespace Test.CpuTests
{
    public class BIT_x_HLm : CpuTestBase
    {
        private readonly ushort _hl;

        public BIT_x_HLm()
        {
            _hl = Fixture.Create<ushort>();
        }

        [Theory, PropertyData("Bits")]
        public void Advances_counters(int bit)
        {
            ExecutingCB(CreateOpCode(bit));

            AdvancedProgramCounter(2);
            AdvancedClock(4);
        }

        [Theory, PropertyData("Bits")]
        public void Sets_z_when_bit_is_zero(int bit)
        {
            Flags(x => x.ResetZero());
            RegisterPair.HL.Set(Cpu, (byte)(_hl >> 8), (byte)_hl);
            FakeMmu.SetByte(_hl, (byte)(~(1 << bit)));

            ExecutingCB(CreateOpCode(bit));

            AssertFlags(x => x.SetZero());
        }

        [Theory, PropertyData("Bits")]
        public void Resets_z_when_bit_is_set(int bit)
        {
            Flags(x => x.Zero());
            RegisterPair.HL.Set(Cpu, (byte)(_hl >> 8), (byte)_hl);
            FakeMmu.SetByte(_hl, (byte)(1 << bit));

            ExecutingCB(CreateOpCode(bit));

            AssertFlags(x => x.ResetZero());
        }

        [Theory, PropertyData("Bits")]
        public void Sets_hc_and_resets_n(int bit)
        {
            Flags(x => x.ResetHalfCarry().Subtract());

            ExecutingCB(CreateOpCode(bit));

            AssertFlags(x => x.HalfCarry().ResetSubtract());
        }

        private static byte CreateOpCode(int bit)
        {
            return (byte)(0x46 | (bit << 3));
        }

        public static IEnumerable<object[]> Bits
        {
            get
            {
                return Enumerable.Range(0, 8)
                    .Select(x => new Object[] { x })
                    .ToList();
            }
        }
    }
}