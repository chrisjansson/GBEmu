using System;
using System.IO;
using Xunit;

namespace Test
{
    public class LD_acceptance_test : TestBase
    {
        public LD_acceptance_test()
        {
            var ldAcceptanceTestRom = File.ReadAllBytes("dmg_rom.bin");

            for (var i = 0; i < ldAcceptanceTestRom.Length; i++)
            {
                var n = ldAcceptanceTestRom[i];
                FakeMmu.SetByte((ushort) i, n);
            }
        }

        [Fact]
        public void Executes_instructions()
        {
            Sut.ProgramCounter = 0x00;

            var verbose = false;
            var count = 0;
            for (var i = 0; i < 100000000; i++)
            {
                var instruction = FakeMmu.GetByte(Sut.ProgramCounter);
                Sut.Execute(instruction);

                if ((FakeMmu.Memory[0xFF02] & 0x80) == 0x80)
                {
                    FakeMmu.Memory[0xFF02] = 0;
                    verbose = true;
                }
                if (verbose)
                {
                    //Console.WriteLine("{0:x}", Sut.ProgramCounter);
                    count++;
                    if (count == 1000)
                    {
                        verbose = false;
                    }
                }
            }

            var fileStream = File.OpenWrite("dump");
            foreach (var b   in FakeMmu.Memory)
            {
                fileStream.WriteByte(b);
            }

            fileStream.Close();
        }
    }
}
