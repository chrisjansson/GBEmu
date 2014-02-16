using System;

namespace Core
{
    public interface IDisplayDataTransferService
    {
        void TransferScanLine(int line);
        void FinishFrame();
    }

    public class DisplayDataTransferService : IDisplayDataTransferService
    {
        private const int WindowWidth = 160;
        private const int WindowHeight = 144;
        private const int TileHeight = 8;
        private const int TileWidth = 8;

        private readonly IMmu _mmu;
        public byte[] FrameBuffer = new Byte[WindowWidth*WindowHeight];

        public DisplayDataTransferService(IMmu mmu)
        {
            _mmu = mmu;
        }

        public void TransferScanLine(int line)
        {
            var scrollX = _mmu.GetByte(RegisterAddresses.ScrollX);
            var scrollY = _mmu.GetByte(RegisterAddresses.ScrollY);

            var backgroundY = (line + scrollY) & 0xFF;
            for (var i = 0; i < WindowWidth; i++)
            {
                var backgroundX = (scrollX + i) & 0xFF;
                var block = backgroundX / 8 + 32 * (backgroundY / 8);
                var tileNumber = _mmu.GetByte((ushort)(0x9800 + block));

                var y = backgroundY % TileHeight;
                var lower = _mmu.GetByte((ushort)(0x8000 + (tileNumber * 16) + y * 2));
                var higher = _mmu.GetByte((ushort)(0x8000 + (tileNumber * 16) + 1 + y * 2));

                var x = backgroundX % TileWidth;
                var lowerPixel = (lower >> (7 - x)) & 0x01;
                var higherPixel = (higher >> (6 - x)) & 0x02;

                var color = higherPixel | lowerPixel;
                FrameBuffer[line*WindowWidth + i] = (byte) color;
            }
        }

        public void FinishFrame()
        {
            throw new System.NotImplementedException();
        }
    }
}