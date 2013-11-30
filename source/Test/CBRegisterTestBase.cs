using System.Collections.Generic;
using System.Linq;
using Xunit.Extensions;

namespace Test
{
    public abstract class CBRegisterTestBase : CpuTestBase
    {
        [Theory, PropertyData("Registers")]
        public void Advances_counters(RegisterMapping register)
        {
            ExecutingCB(CreateOpCode(register));

            AdvancedProgramCounter(2);
            AdvancedClock(2);
        }

        public static IEnumerable<object[]> Registers
        {
            get
            {
                return RegisterMapping
                    .GetAll()
                    .Select(x => new object[] { x })
                    .ToList();
            }
        }

        protected abstract byte CreateOpCode(RegisterMapping register);
    }
}