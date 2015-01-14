using System;
using Core;
using Xunit;

namespace Test
{
    public abstract class EmulatorBootstrapperTests
    {
        public class WhenLoadingRom : EmulatorBootstrapperTests
        {
            private readonly Emulator _actual;

            public WhenLoadingRom()
            {
                var sut = new EmulatorBootstrapper();

                _actual = sut.LoadRom(new byte[0]);
            }

            [Fact]
            public void Should_select_memory_bank_controller()
            {
                throw new NotImplementedException();
            }

            [Fact]
            public void Should_set_program_counter_to_0x100()
            {
                const int intialProgramCounter = 0x100;
                Assert.Equal(intialProgramCounter, _actual.Cpu.ProgramCounter);
            }

            [Fact]
            public void Should_set_AF_to_0x01B0()
            {
                Assert.Equal(0x01B0, RegisterPair.AF.Get(_actual.Cpu));
            }

            [Fact]
            public void Should_set_BC_to_0x0013()
            {
                Assert.Equal(0x0013, RegisterPair.BC.Get(_actual.Cpu));
            }

            [Fact]
            public void Should_set_DE_to_0x00D8()
            {
                Assert.Equal(0x00D8, RegisterPair.DE.Get(_actual.Cpu));
            }

            [Fact]
            public void Should_set_HL_to_0x014D()
            {
                Assert.Equal(0x014D, RegisterPair.HL.Get(_actual.Cpu));
            }

            [Fact]
            public void Should_set_stack_pointer_to_0xFFFE()
            {
                Assert.Equal(0xFFFE, RegisterPair.SP.Get(_actual.Cpu));
            }

            [Fact]
            public void Should_initialize_hardware_registers()
            {
                AssertMemory(0xFF05, 0x00); //TIMA
                AssertMemory(0xFF06, 0x00); //TMA
                AssertMemory(0xFF07, 0x00); //TAC
                AssertMemory(0xFF10, 0x80); //NR10
                AssertMemory(0xFF11, 0xBF); //NR11
                AssertMemory(0xFF12, 0xF3); //NR12
                AssertMemory(0xFF14, 0xBF); //NR14
                AssertMemory(0xFF16, 0x3F); //NR21
                AssertMemory(0xFF17, 0x00); //NR22
                AssertMemory(0xFF19, 0xBF); //NR24
                AssertMemory(0xFF1A, 0x7F); //NR30
                AssertMemory(0xFF1B, 0xFF); //NR31
                AssertMemory(0xFF1C, 0x9F); //NR32
                AssertMemory(0xFF1E, 0xBF); //NR33
                AssertMemory(0xFF20, 0xFF); //NR41
                AssertMemory(0xFF21, 0x00); //NR42
                AssertMemory(0xFF22, 0x00); //NR43
                AssertMemory(0xFF23, 0xBF); //NR30
                AssertMemory(0xFF24, 0x77); //NR50
                AssertMemory(0xFF25, 0xF3); //NR51
                AssertMemory(0xFF26, 0xF1); //NR52
                AssertMemory(0xFF40, 0x91); //LCDC
                AssertMemory(0xFF42, 0x00); //SCY
                AssertMemory(0xFF43, 0x00); //SCX
                AssertMemory(0xFF45, 0x00); //LYC
                AssertMemory(0xFF47, 0xFC); //BGP
                AssertMemory(0xFF48, 0xFF); //0BP0
                AssertMemory(0xFF49, 0xFF); //0BP1
                AssertMemory(0xFF4A, 0x00); //WY
                AssertMemory(0xFF4B, 0x00); //WX
                AssertMemory(0xFFFF, 0x00); //IE
            }

            private void AssertMemory(ushort address, byte expected)
            {
                Assert.Equal(expected, _actual.Mmu.GetByte(address));
            }
        }
    }
}