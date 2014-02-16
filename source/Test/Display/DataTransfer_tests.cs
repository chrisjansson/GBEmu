using System;
using Core;
using Xunit;

namespace Test.Display
{
    public class DataTransfer_tests
    {
        private DisplayDataTransferService _sut;
        private FakeMmu _fakeMmu;

        public DataTransfer_tests()
        {
            _fakeMmu = new FakeMmu();
            _sut = new DisplayDataTransferService(_fakeMmu);
        }

        [Fact]
        public void Copies_upper_left_corner_to_first_row()
        {
            _fakeMmu.Memory[0x8000] = 0x7C;
            _fakeMmu.Memory[0x8001] = 0x7C;
            _fakeMmu.Memory[0x8002] = 0x00;
            _fakeMmu.Memory[0x8003] = 0xC6;
            _fakeMmu.Memory[0x8004] = 0xC6;
            _fakeMmu.Memory[0x8005] = 0x00;
            _fakeMmu.Memory[0x8006] = 0x00;
            _fakeMmu.Memory[0x8007] = 0xFE;
            _fakeMmu.Memory[0x8008] = 0xC6;
            _fakeMmu.Memory[0x8009] = 0xC6;
            _fakeMmu.Memory[0x800A] = 0x00;
            _fakeMmu.Memory[0x800B] = 0xC6;
            _fakeMmu.Memory[0x800C] = 0xC6;
            _fakeMmu.Memory[0x800D] = 0x00;
            _fakeMmu.Memory[0x800E] = 0x00;
            _fakeMmu.Memory[0x800F] = 0x00;

            _fakeMmu.Memory[0x8010] = 0x3C;
            _fakeMmu.Memory[0x8011] = 0x3C;
            _fakeMmu.Memory[0x8012] = 0x66;
            _fakeMmu.Memory[0x8013] = 0x66;
            _fakeMmu.Memory[0x8014] = 0x6E;
            _fakeMmu.Memory[0x8015] = 0x6E;
            _fakeMmu.Memory[0x8016] = 0x76;
            _fakeMmu.Memory[0x8017] = 0x76;
            _fakeMmu.Memory[0x8018] = 0x66;
            _fakeMmu.Memory[0x8019] = 0x66;
            _fakeMmu.Memory[0x801A] = 0x66;
            _fakeMmu.Memory[0x801B] = 0x66;
            _fakeMmu.Memory[0x801C] = 0x3C;
            _fakeMmu.Memory[0x801D] = 0x3C;
            _fakeMmu.Memory[0x801E] = 0x00;
            _fakeMmu.Memory[0x801F] = 0x00;

            _fakeMmu.Memory[0x8020] = 0x7C;
            _fakeMmu.Memory[0x8021] = 0x7C;
            _fakeMmu.Memory[0x8022] = 0x66;
            _fakeMmu.Memory[0x8023] = 0x66;
            _fakeMmu.Memory[0x8024] = 0x66;
            _fakeMmu.Memory[0x8025] = 0x66;
            _fakeMmu.Memory[0x8026] = 0x7C;
            _fakeMmu.Memory[0x8027] = 0x7C;
            _fakeMmu.Memory[0x8028] = 0x66;
            _fakeMmu.Memory[0x8029] = 0x66;
            _fakeMmu.Memory[0x802A] = 0x66;
            _fakeMmu.Memory[0x802B] = 0x66;
            _fakeMmu.Memory[0x802C] = 0x7C;
            _fakeMmu.Memory[0x802D] = 0x7C;
            _fakeMmu.Memory[0x802E] = 0x00;
            _fakeMmu.Memory[0x802F] = 0x00;

            _fakeMmu.Memory[0x9800] = 0;
            _fakeMmu.Memory[0x9801] = 1;
            _fakeMmu.Memory[0x9802] = 3;
            _fakeMmu.Memory[0x9803] = 3;
            _fakeMmu.Memory[0x9820] = 2;
            _fakeMmu.Memory[0x9820] = 2;
            _fakeMmu.Memory[0x9821] = 3;
            _fakeMmu.Memory[0x9822] = 3;

            //_fakeMmu.Memory[RegisterAddresses.ScrollX] = 247;
            //_fakeMmu.Memory[RegisterAddresses.ScrollY] = 247;

            for (int j = 0; j < 16; j++)
            {
                _sut.TransferScanLine(j);

                for (int i = 0; i < 160; i++)
                {
                    Console.Write(Convert(_sut.FrameBuffer[j * 160 + i]));
                }
                Console.WriteLine();
            }
        }

        private char Convert(int color)
        {
            if (color == 0)
            {
                return '.';
            }

            if (color == 1)
            {
                return '1';
            }

            if (color == 2)
            {
                return '2';
            }

            return '3';
        }
    }

    public static class MMUExtensions
    {
        public static void ScrollX(this IMmu mmu, byte scrollY)
        {
            mmu.SetByte(RegisterAddresses.ScrollX, scrollY);
        }

        public static void ScrollY(this IMmu mmu, byte scrollY)
        {
            mmu.SetByte(RegisterAddresses.ScrollY, scrollY);
        }
    }
}