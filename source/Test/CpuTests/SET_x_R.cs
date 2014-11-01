using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace Test.CpuTests
{
    public class SET_x_R : CBTestTargetBase
    {
        [Theory, InstancePropertyData("Targets")]
        public void Sets_bit(ISETxR_CBTestTarget target)
        {
            target.SetUp(this);
            target.ArrangeArgument(0x00);

            ExecutingCB(target.OpCode);

            Assert.Equal(target.Expected, target.Actual);
        }

        protected override IEnumerable<ICBTestTarget> GetTargets()
        {
            var targets = from bit in Enumerable.Range(0, 8)
                from register in RegisterMapping.GetAll()
                select new SETxR_CBTestTarget(bit, register);

            return targets.ToList();
        }

        public interface ISETxR_CBTestTarget : ICBTestTarget
        {
            byte Expected { get; }
        }

        private class SETxR_CBTestTarget : RegisterCBTestTargetBase, ISETxR_CBTestTarget
        {
            private readonly int _bit;

            public SETxR_CBTestTarget(int bit, RegisterMapping register)
                : base(register)
            {
                _bit = bit;
            }

            public override byte OpCode
            {
                get
                {
                    return (byte)(0xC0 | Register | (_bit << 3));
                }
            }

            public byte Expected
            {
                get
                {
                    return (byte)(1 << _bit);
                }
            }

            public override string ToString()
            {
                return Register + " " + _bit;
            }
        }
    }
}