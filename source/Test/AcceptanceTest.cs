using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Core;
using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class AcceptanceTest
    {
        private readonly List<char> _serialOutput = new List<char>();
        private Emulator _emulator;
        private ushort _previousProgramCounter;

        [Theory, PropertyData("streams")]
        public void Passes_blarrg_test_rom(string rom)
        {
            LoadTestRomIntoMmu(rom);

            Run();

            AssertTestRomPassed();
        }

        private void Run()
        {
            var programCounterRepeatCount = 0;
            for (var i = 0; i < 100000000 && programCounterRepeatCount < 10000; i++)
            {
                _emulator.Tick();
                SendSerialData();

                var programCounter = _emulator.Cpu.ProgramCounter;
                if (programCounter == _previousProgramCounter)
                {
                    programCounterRepeatCount++;
                }
                else
                {
                    _previousProgramCounter = programCounter;
                    programCounterRepeatCount = 0;
                }
            }
        }

        private void SendSerialData()
        {
            var mmu = _emulator.Mmu;
            if ((mmu.GetByte(0xFF02) & 0x80) == 0x80)
            {
                _serialOutput.Add((char) mmu.GetByte(0xFF01));
                mmu.SetByte(0xFF02, 0);
            }
        }

        private void AssertTestRomPassed()
        {
            var output = _serialOutput.Aggregate("", (x, y) => x + y);
            Assert.Contains("passed", output.ToLower());
        }

        private void LoadTestRomIntoMmu(string rom)
        {
            var ldAcceptanceTestRom = File.ReadAllBytes(rom);
            LoadTestRomIntoMmu(ldAcceptanceTestRom);
        }

        private void LoadTestRomIntoMmu(byte[] rom)
        {
            var emulatorBootstrapper = new EmulatorBootstrapper();
            _emulator = emulatorBootstrapper.LoadRom(rom);
        }

        public static IEnumerable<object[]> streams
        {
            get
            {
                return TestRomArchives
                    .SelectMany(x => B(x))
                    .Select(x => new[] { x })
                    .ToList();
            }
        }

        private static IEnumerable<string> B(BlarggTestSuite blarggTestSuite)
        {
            var directoryName = Path.GetFileNameWithoutExtension(blarggTestSuite.Archive);
            if (!Directory.Exists(directoryName))
            {
                ZipFile.ExtractToDirectory(blarggTestSuite.Archive, ".");
            }

            var allRoms = Directory.EnumerateFiles(directoryName, "*.gb", SearchOption.AllDirectories);
            return allRoms;
        }

        public class BlarggTestSuite
        {
            public string Archive { get; set; }
        }

        public static IEnumerable<BlarggTestSuite> TestRomArchives
        {
            get
            {
                return new[]
                {
                    //new BlarggTestSuite {Archive = "cgb_sound.zip",},
                    new BlarggTestSuite {Archive = "cpu_instrs.zip",},
                    new BlarggTestSuite {Archive = "dmg_sound.zip",},
                    //new BlarggTestSuite {Archive = "dmg_sound-2.zip",},
                    new BlarggTestSuite {Archive = "instr_timing.zip",},
                    //new BlarggTestSuite {Archive = "mem_timing.zip",},
                    //new BlarggTestSuite {Archive = "mem_timing-2.zip",},
                    //new BlarggTestSuite {Archive = "oam_bug.zip",},
                    //new BlarggTestSuite {Archive = "oam_bug-2.zip",},
                };
            }
        }
    }
}
