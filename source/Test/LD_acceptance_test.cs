using System.IO;
using Xunit;

namespace Test
{
    public class LD_acceptance_test : TestBase
    {
        public LD_acceptance_test()
        {
            var ldAcceptanceTestRom = File.ReadAllBytes("07-jr,jp,call,ret,rst.gb");

            for (var i = 0; i < ldAcceptanceTestRom.Length; i++)
            {
                var n = ldAcceptanceTestRom[i];
                FakeMmu.SetByte((ushort) i, n);
            }
        }

        [Fact]
        public void Executes_instructions()
        {
            Sut.ProgramCounter = 0x100;

            for (var i = 0; i < 10000; i++)
            {
                var instruction = FakeMmu.GetByte(Sut.ProgramCounter);
                Sut.Execute(instruction);
            }
        }
    }
}
