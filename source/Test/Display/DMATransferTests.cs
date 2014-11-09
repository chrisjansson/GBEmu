using System;
using Xunit;

namespace Test.Display
{
    public class DMATransferTests
    {
        private readonly Core.Display _sut;
        private readonly FakeMmu _mmu;

        public DMATransferTests()
        {
            _mmu = new FakeMmu();
            _sut = new Core.Display(_mmu, new FakeDisplayDataTransferService());
        }

        private static byte[] GenerateSourceValues()
        {
            var random = new Random(123);
            var sourceValues = new byte[160];
            for (var i = 0; i < 160; i++)
            {
                sourceValues[i] = (byte)random.Next();
            }
            return sourceValues;
        }

        [Fact]
        public void Setting_dma_resets_dma_cycles_to_168_cyles_left()
        {
            _sut.DMA = 0x10;

            Assert.Equal(168, _sut.DMACycles);
        }

        [Fact]
        public void Decrements_dma_cycles()
        {
            _sut.DMA = 0x10;

            _sut.Tick(5);

            Assert.Equal(163, _sut.DMACycles);
        }

        [Fact]
        public void Decrements_dma_cycles_to_zero()
        {
            _sut.DMA = 0x10;

            _sut.Tick(170);

            Assert.Equal(0, _sut.DMACycles);
        }

        [Fact]
        public void Copies_values_from_source_address_to_range_FF00_FE9F()
        {
            var sourceValues = GenerateSourceValues();
            InsertValuesAt(0x1600, sourceValues);

            _sut.DMA = 0x16;
            _sut.Tick(168);

            AssertOAMFF00Contains(sourceValues);
        }

        private void InsertValuesAt(ushort startAddress, byte[] sourceValues)
        {
            for (int i = 0; i < 255; i++)
            {
                var address = 0xFE00 + i;
                _mmu.Memory[address] = 0xFF;
            }
            for (var i = 0; i < sourceValues.Length; i++)
            {
                var address = startAddress + i;
                _mmu.Memory[address] = sourceValues[i];
            }
        }

        private void AssertOAMFF00Contains(byte[] expected)
        {
            for (var i = 0; i < expected.Length; i++)
            {
                var expectedAddress = 0xFE00 + i;
                var expectedValue = expected[i];
                Assert.Equal(expectedValue, _mmu.Memory[expectedAddress]);
            }

            Assert.Equal(0xFF, _mmu.Memory[0xFEA0]);
        }
    }
}