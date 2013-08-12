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
                case 0x47: //LD_B_A
                    B = A;
                    break;
                case 0x7F: //LD_A_A
                    A = A;
                    break;
                case 0x78: //LD_A_B
                    A = B;
                    break;
                case 0x79: //LD_A_C
                    A = C;
                    break;
                case 0x7A: //LD_A_D
                    A = D;
                    break;
                case 0x7B: //LD_A_E
                    A = E;
                    break;
                case 0x7C: //LD_A_H
                    A = H;
                    break;
                case 0x7D: //LD_A_L
                    A = L;
                    break;
                case 0x4F: //LD_C_A
                    C = A;
                    break;
                case 0x48: //LD_C_B
                    C = B;
                    break;
                case 0x49: //LD_C_C
                    C = C;
                    break;
                case 0x4A: //LC_C_D
                    C = D;
                    break;
                case 0x4B:
                    C = E;
                    break;
                case 0x4C:
                    C = H;
                    break;
                case 0x4D:
                    C = L;
                    break;
                case 0x50:
                    D = B;
                    break;
                case 0x51:
                    D = C;
                    break;
                case 0x52:
                    D = D;
                    break;
                case 0x53:
                    D = E;
                    break;
                case 0x54:
                    D = H;
                    break;
                case 0x55:
                    D = L;
                    break;
                case 0x56:
                    throw new NotImplementedException("LD D, (HL)");
                case 0x57:
                    D = A;
                    break;
                case 0x58:
                    E = B;
                    break;
                case 0x59:
                    E = C;
                    break;
                case 0x5A:
                    E = D;
                    break;
                case 0x5B:
                    E = E;
                    break;
                case 0x5C:
                    E = H;
                    break;
                case 0x5D:
                    E = L;
                    break;
                case 0x5E:
                    throw new NotImplementedException("LD E, (HL)");
                case 0x5F:
                    E = A;
                   break;
                case 0x60:
                    H = B;
                    break;
                case 0x61:
                    H = C;
                    break;
                case 0x62:
                    H = D;
                    break;
                case 0x63:
                    H = E;
                    break;
                case 0x64:
                    H = H;
                    break;
                case 0x65:
                    H = L;
                    break;
                case 0x66:
                    throw new NotImplementedException("LD H, (HL)");
                case 0x67:
                    H = A;
                    break;
                case 0x68:
                    L = B;
                    break;
                case 0x69:
                    L = C;
                    break;
                case 0x6A:
                    L = D;
                    break;
                case 0x6B:
                    L = E;
                    break;
                case 0x6C:
                    L = H;
                    break;
                case 0x6D:
                    L = L;
                    break;
                case 0x6E:
                    throw new NotImplementedException("LD L, (HL)");
                case 0x6F:
                    L = A;
                    break;
                default:
                    throw new IllegalOpcodeException();
            }
        }

        public byte A { get; set; }

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
