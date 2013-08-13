using System;

namespace Core
{
    public interface IMmu
    {
        byte GetByte(ushort address);
    }

    public enum Register
    {
        A = 0,
        B,
        C,
        D,
        E,
        H,
        L
    }

    public class Cpu
    {
        private readonly byte[] _registers = new byte[7];

        private void LD_r_r(Register target, Register source)
        {
            _registers[(int)target] = _registers[(int)source];
            ProgramCounter++;
        }

        public void Execute(byte opcode)
        {
            switch (opcode)
            {
                case 0x40:
                    LD_r_r(Register.B, Register.B);
                    break;
                case 0x41:
                    LD_r_r(Register.B, Register.C);
                    break;
                case 0x42:
                    LD_r_r(Register.B, Register.D);
                    break;
                case 0x43:
                    LD_r_r(Register.B, Register.E);
                    break;
                case 0x44:
                    LD_r_r(Register.B, Register.H);
                    break;
                case 0x45:
                    LD_r_r(Register.B, Register.L);
                    break;
                case 0x47: //LD_B_A
                    LD_r_r(Register.B, Register.A);
                    break;
                case 0x7F: //LD_A_A
                    LD_r_r(Register.A, Register.A);
                    break;
                case 0x78: //LD_A_B
                    LD_r_r(Register.A, Register.B);
                    break;
                case 0x79: //LD_A_C
                    LD_r_r(Register.A, Register.C);
                    break;
                case 0x7A: //LD_A_D
                    LD_r_r(Register.A, Register.D);
                    break;
                case 0x7B: //LD_A_E
                    LD_r_r(Register.A, Register.E);
                    break;
                case 0x7C: //LD_A_H
                    LD_r_r(Register.A, Register.H);
                    break;
                case 0x7D: //LD_A_L
                    LD_r_r(Register.A, Register.L);
                    break;
                case 0x4F: //LD_C_A
                    LD_r_r(Register.C, Register.A);
                    break;
                case 0x48: //LD_C_B
                    LD_r_r(Register.C, Register.B);
                    break;
                case 0x49: //LD_C_C
                    LD_r_r(Register.C, Register.C);
                    break;
                case 0x4A: //LC_C_D
                    LD_r_r(Register.C, Register.D);
                    break;
                case 0x4B:
                    LD_r_r(Register.C, Register.E);
                    break;
                case 0x4C:
                    LD_r_r(Register.C, Register.H);
                    break;
                case 0x4D:
                    LD_r_r(Register.C, Register.L);
                    break;
                case 0x50:
                    LD_r_r(Register.D, Register.B);
                    break;
                case 0x51:
                    LD_r_r(Register.D, Register.C);
                    break;
                case 0x52:
                    LD_r_r(Register.D, Register.D);
                    break;
                case 0x53:
                    LD_r_r(Register.D, Register.E);
                    break;
                case 0x54:
                    LD_r_r(Register.D, Register.H);
                    break;
                case 0x55:
                    LD_r_r(Register.D, Register.L);
                    break;
                case 0x56:
                    throw new NotImplementedException("LD D, (HL)");
                case 0x57:
                    LD_r_r(Register.D, Register.A);
                    break;
                case 0x58:
                    LD_r_r(Register.E, Register.B);
                    break;
                case 0x59:
                    LD_r_r(Register.E, Register.C);
                    break;
                case 0x5A:
                    LD_r_r(Register.E, Register.D);
                    break;
                case 0x5B:
                    LD_r_r(Register.E, Register.E);
                    break;
                case 0x5C:
                    LD_r_r(Register.E, Register.H);
                    break;
                case 0x5D:
                    LD_r_r(Register.E, Register.L);
                    break;
                case 0x5E:
                    throw new NotImplementedException("LD E, (HL)");
                case 0x5F:
                    LD_r_r(Register.E, Register.A);
                    break;
                case 0x60:
                    LD_r_r(Register.H, Register.B);
                    break;
                case 0x61:
                    LD_r_r(Register.H, Register.C);
                    break;
                case 0x62:
                    LD_r_r(Register.H, Register.D);
                    break;
                case 0x63:
                    LD_r_r(Register.H, Register.E);
                    break;
                case 0x64:
                    LD_r_r(Register.H, Register.H);
                    break;
                case 0x65:
                    LD_r_r(Register.H, Register.L);
                    break;
                case 0x66:
                    throw new NotImplementedException("LD H, (HL)");
                case 0x67:
                    LD_r_r(Register.H, Register.A);
                    break;
                case 0x68:
                    LD_r_r(Register.L, Register.B);
                    break;
                case 0x69:
                    LD_r_r(Register.L, Register.C);
                    break;
                case 0x6A:
                    LD_r_r(Register.L, Register.D);
                    break;
                case 0x6B:
                    LD_r_r(Register.L, Register.E);
                    break;
                case 0x6C:
                    LD_r_r(Register.L, Register.H);
                    break;
                case 0x6D:
                    LD_r_r(Register.L, Register.L);
                    break;
                case 0x6E:
                    throw new NotImplementedException("LD L, (HL)");
                case 0x6F:
                    LD_r_r(Register.L, Register.A);
                    break;
                default:
                    throw new IllegalOpcodeException();
            }
        }

        public byte A
        {
            get { return _registers[(int)Register.A]; }
            set { _registers[(int)Register.A] = value; }
        }

        public byte B
        {
            get { return _registers[(int)Register.B]; }
            set { _registers[(int)Register.B] = value; }
        }

        public byte C
        {
            get { return _registers[(int)Register.C]; }
            set { _registers[(int)Register.C] = value; }
        }

        public byte D
        {
            get { return _registers[(int)Register.D]; }
            set { _registers[(int)Register.D] = value; }
        }

        public byte E
        {
            get { return _registers[(int)Register.E]; }
            set { _registers[(int)Register.E] = value; }
        }

        public byte H
        {
            get { return _registers[(int)Register.H]; }
            set { _registers[(int)Register.H] = value; }
        }

        public byte L
        {
            get { return _registers[(int)Register.L]; }
            set { _registers[(int)Register.L] = value; }
        }

        public ushort ProgramCounter { get; set; }
    }

    public class IllegalOpcodeException : Exception
    {
        public override string Message
        {
            get { return "Illegal opcode"; }
        }
    }
}
