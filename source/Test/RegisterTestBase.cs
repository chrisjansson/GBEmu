using System.Collections.Generic;
using System.Linq;
using Xunit.Extensions;

namespace Test
{
    public abstract class RegisterTestBase : CpuTestBase
    {
        [Theory, PropertyData("Registers")]
        public void AdvancesCounters(RegisterMapping register)
        {
            Execute(CreateOpCode(register));

            AdvancedProgramCounter(1);
            AdvancedClock(1);
        }

        public static IEnumerable<object[]> Registers
        {
            get
            {
                return RegisterMapping.GetAll().Select(x => new[] { x });
            }
        }

        public static IEnumerable<object[]> RegistersExceptA
        {
            get
            {
                return RegisterMapping.GetAll()
                    .Where(x => x != RegisterMapping.A)
                    .Select(x => new[] { x });
            }
        }

        protected abstract byte CreateOpCode(RegisterMapping register);
    }
}