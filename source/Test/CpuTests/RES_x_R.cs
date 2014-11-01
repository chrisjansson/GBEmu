using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace Test.CpuTests
{
    public class RES_x_R : CBTestTargetBase
    {
        [Theory, InstancePropertyData("Targets")]
        public void Resets_bit(IRESxR_CBTestTarget target)
        {
            target.SetUp(this);
            target.ArrangeArgument(0xFF);

            ExecutingCB(target.OpCode);

            Assert.Equal(target.Expected, target.Actual);
        }

        protected override IEnumerable<ICBTestTarget> GetTargets()
        {
            var targets =
                from bit in Enumerable.Range(0, 8)
                from register in RegisterMapping.GetAll()
                select new RESxR_CBTestTarget(bit, register);

            return targets.ToList();
        }

        public interface IRESxR_CBTestTarget : ICBTestTarget
        {
            byte Expected { get; }
        }

        private class RESxR_CBTestTarget : RegisterCBTestTargetBase, IRESxR_CBTestTarget
        {
            private readonly int _bit;

            public RESxR_CBTestTarget(int bit, RegisterMapping register)
                : base(register)
            {
                _bit = bit;
            }

            public override byte OpCode
            {
                get
                {
                    return (byte)(0x80 | Register | (_bit << 3));
                }
            }

            public byte Expected
            {
                get
                {
                    return (byte)(0xFF & ~(1 << _bit));
                }
            }

            public override string ToString()
            {
                return Register + " " + _bit;
            }
        }
    }
}