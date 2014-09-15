using System.Collections.Generic;
using System.Linq;

namespace Test
{
    public class Arithmetic16BitTestBase : CpuTestBase
    {
        public static IEnumerable<object[]> ArithmeticRegisterPairs
        {
            get
            {
                return RegisterPair.GetArithmeticPairs()
                    .Select(x => new[] {x})
                    .ToList();
            }
        }
    }
}