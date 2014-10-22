using System.Collections.Generic;
using System.Linq;
using Xunit.Extensions;

namespace Test.CpuTests
{
    public abstract class CBTestTargetBase : CpuTestBase
    {
        protected abstract IEnumerable<ICBTestTarget> GetTargets();

        public IEnumerable<object[]> Targets
        {
            get
            {
                return GetTargets()
                    .Select(x => new object[] { x })
                    .ToList();
            }
        }

        [Theory, InstancePropertyData("Targets")]
        public void Advances_counters(ICBTestTarget target)
        {
            target.SetUp(this);

            ExecutingCB(target.OpCode);

            AdvancedProgramCounter(target.InstructionLength);
            AdvancedClock(target.InstructionTime);
        }
    }
}