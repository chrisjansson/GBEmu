using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Cpu
    {
        public void Execute(byte opcode)
        {
            switch (opcode)
            {
                case 0x40: //LD_B_B
                    B = B;
                    break;
                case 0x41: //LD_B_C
                    B = C;
                    break;
                case 0x42: //LD_B_D
                    B = D; 
                    break;
                case 0x43: //LD_B_E
                    B = E;
                    break;
                case 0x44:
                    B = H; //LD_B_H
                    break;
                case 0x45:
                    B = L;
                    break;
                default:
                    throw new IllegalOpcodeException();
            }
        }

        public byte B { get; set; }
        public byte C { get; set; }
        public byte D { get; set; }
        public byte E { get; set; }
        public byte H { get; set; }
        public byte L { get; set; }
    }

    public class IllegalOpcodeException : Exception
    {
        public override string Message
        {
            get { return "Illegal opcode"; }
        }
    }
}
