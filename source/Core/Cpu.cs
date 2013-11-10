using System;
using System.Collections.Generic;

namespace Core
{
    public interface IMmu
    {
        byte GetByte(ushort address);
        void SetByte(ushort address, byte value);
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

    public struct InstructionMetaData
    {
        public InstructionMetaData(ushort size, int cycles, string mnemonic)
        {
            Size = size;
            Cycles = cycles;
            Mnemonic = mnemonic;
        }

        public readonly ushort Size;
        public readonly int Cycles;
        public readonly string Mnemonic;
    }

    public class Cpu
    {
        private readonly byte[] _registers = new byte[7];

        private readonly Dictionary<byte, InstructionMetaData> _instructionMetaData = new Dictionary<byte, InstructionMetaData>
        {
            { 0x00, new InstructionMetaData(1, 1, "NOP")},
            { 0x06, new InstructionMetaData(2, 2, "LD B, n")},
            { 0x0D, new InstructionMetaData(1, 1, "DEC C")},
            { 0x0E, new InstructionMetaData(2, 2, "LD C, n")},
            { 0x11, new InstructionMetaData(3, 3, "LD DE, nn")},
            { 0x12, new InstructionMetaData(1, 2, "LD (DE), A")},
            { 0x14, new InstructionMetaData(1, 1, "INC D")},
            { 0x16, new InstructionMetaData(2, 2, "LD D, n")},
            { 0x1C, new InstructionMetaData(1, 1, "INC E")},
            { 0x1E, new InstructionMetaData(2, 2, "LD E, n")},
            { 0x21, new InstructionMetaData(3, 3, "LD HL, nn")},
            { 0x26, new InstructionMetaData(2, 2, "LD H, n")},
            { 0x2A, new InstructionMetaData(1, 2, "LD A, (HLI)")},
            { 0x2E, new InstructionMetaData(2, 2, "LD L, n")},
            { 0x31, new InstructionMetaData(3, 3, "LD SP, nn")},
            { 0x3E, new InstructionMetaData(2, 2, "LD A, n")},
            { 0x40, new InstructionMetaData(1, 1, "LD B, B")},
            { 0x41, new InstructionMetaData(1, 1, "LD B, C")},
            { 0x42, new InstructionMetaData(1, 1, "LD B, D")},
            { 0x43, new InstructionMetaData(1, 1, "LD B, E")},
            { 0x44, new InstructionMetaData(1, 1, "LD B, H")},
            { 0x45, new InstructionMetaData(1, 1, "LD B, L")},
            { 0x46, new InstructionMetaData(1, 2, "LD B, (HL)")},
            { 0x47, new InstructionMetaData(1, 1, "LD B, A")},
            { 0x48, new InstructionMetaData(1, 1, "LD C, B")},
            { 0x49, new InstructionMetaData(1, 1, "LD C, C")},
            { 0x4A, new InstructionMetaData(1, 1, "LD C, D")},
            { 0x4B, new InstructionMetaData(1, 1, "LD C, E")},
            { 0x4C, new InstructionMetaData(1, 1, "LD C, H")},
            { 0x4D, new InstructionMetaData(1, 1, "LD C, L")},
            { 0x4E, new InstructionMetaData(1, 2, "LD C, (HL)")},
            { 0x4F, new InstructionMetaData(1, 1, "LD C, A")},
            { 0x50, new InstructionMetaData(1, 1, "LD D, B")},
            { 0x51, new InstructionMetaData(1, 1, "LD D, C")},
            { 0x52, new InstructionMetaData(1, 1, "LD D, D")},
            { 0x53, new InstructionMetaData(1, 1, "LD D, E")},
            { 0x54, new InstructionMetaData(1, 1, "LD D, H")},
            { 0x55, new InstructionMetaData(1, 1, "LD D, L")},
            { 0x56, new InstructionMetaData(1, 2, "LD D, (HL)")},
            { 0x58, new InstructionMetaData(1, 1, "LD E, B")},
            { 0x59, new InstructionMetaData(1, 1, "LD E, C")},
            { 0x5A, new InstructionMetaData(1, 1, "LD E, D")},
            { 0x5B, new InstructionMetaData(1, 1, "LD E, E")},
            { 0x5C, new InstructionMetaData(1, 1, "LD E, H")},
            { 0x5D, new InstructionMetaData(1, 1, "LD E, L")},
            { 0x5E, new InstructionMetaData(1, 2, "LD E, (HL)")},
            { 0x5F, new InstructionMetaData(1, 1, "LD E, A")},
            { 0x57, new InstructionMetaData(1, 1, "LD D, A")},
            { 0x60, new InstructionMetaData(1, 1, "LD H, B")},
            { 0x61, new InstructionMetaData(1, 1, "LD H, C")},
            { 0x62, new InstructionMetaData(1, 1, "LD H, D")},
            { 0x63, new InstructionMetaData(1, 1, "LD H, E")},
            { 0x64, new InstructionMetaData(1, 1, "LD H, H")},
            { 0x65, new InstructionMetaData(1, 1, "LD H, L")},
            { 0x66, new InstructionMetaData(1, 2, "LD H, (HL)")},
            { 0x67, new InstructionMetaData(1, 1, "LD H, A")},
            { 0x68, new InstructionMetaData(1, 1, "LD L, B")},
            { 0x69, new InstructionMetaData(1, 1, "LD L, C")},
            { 0x6A, new InstructionMetaData(1, 1, "LD L, D")},
            { 0x6B, new InstructionMetaData(1, 1, "LD L, E")},
            { 0x6C, new InstructionMetaData(1, 1, "LD L, H")},
            { 0x6D, new InstructionMetaData(1, 1, "LD L, L")},
            { 0x6E, new InstructionMetaData(1, 2, "LD L, (HL)")},
            { 0x6F, new InstructionMetaData(1, 1, "LD L, A")},
            { 0x70, new InstructionMetaData(1, 2, "LD (HL), B")},
            { 0x71, new InstructionMetaData(1, 2, "LD (HL), C")},
            { 0x72, new InstructionMetaData(1, 2, "LD (HL), D")},
            { 0x73, new InstructionMetaData(1, 2, "LD (HL), E")},
            { 0x74, new InstructionMetaData(1, 2, "LD (HL), H")},
            { 0x75, new InstructionMetaData(1, 2, "LD (HL), L")},
            { 0x77, new InstructionMetaData(1, 2, "LD (HL), A")},
            { 0x78, new InstructionMetaData(1, 1, "LD A, B")},
            { 0x79, new InstructionMetaData(1, 1, "LD A, C")},
            { 0x7A, new InstructionMetaData(1, 1, "LD A, D")},
            { 0x7B, new InstructionMetaData(1, 1, "LD A, E")},
            { 0x7C, new InstructionMetaData(1, 1, "LD A, H")},
            { 0x7D, new InstructionMetaData(1, 1, "LD A, L")},
            { 0x7E, new InstructionMetaData(1, 2, "LD A, (HL)")},
            { 0x7F, new InstructionMetaData(1, 1, "LD A, A")},
            { 0xC3, new InstructionMetaData(0, 4, "JP, nn")},
            { 0xEA, new InstructionMetaData(3, 4, "LD (nn), A")},
            { 0xF3, new InstructionMetaData(1, 1, "DI")},
        };

        private readonly IMmu _mmu;

        public Cpu(IMmu mmu)
        {
            _mmu = mmu;
        }

        private void LD_r_r(Register target, Register source)
        {
            _registers[(int)target] = _registers[(int)source];
        }

        private void LD_r_n(Register target)
        {
            var n = _mmu.GetByte((ushort)(ProgramCounter + 1));
            _registers[(int)target] = n;
        }

        private void INC_r(Register register)
        {
            HC = (byte)((((_registers[(int)register] & 0xF) + 1) & 0x10) == 0 ? 0 : 1);
            _registers[(int)register] = (byte)(_registers[(int)register] + 1);
            Z = (byte)(_registers[(int)register] == 0 ? 1 : 0);
            N = 0;
        }

        public void Execute(byte opcode)
        {
            switch (opcode)
            {
                case 0x00:
                    //NOP
                    break;
                case 0x06:
                    LD_r_n(Register.B);
                    break;
                case 0x0D:
                    N = 1;
                    HC = (byte)((C & 0x0F) == 0 ? 1 : 0);
                    C = (byte)(C - 1);
                    Z = (byte)(C == 0 ? 1 : 0);
                    break;
                case 0x0E:
                    LD_r_n(Register.C);
                    break;
                case 0x11:
                    E = _mmu.GetByte((ushort)(ProgramCounter + 1));
                    D = _mmu.GetByte((ushort)(ProgramCounter + 2));
                    break;
                case 0x12:
                    var target = (D << 8 | E);
                    _mmu.SetByte((ushort)target, A);
                    break;
                case 0x14:
                    INC_r(Register.D);
                    break;
                case 0x16:
                    LD_r_n(Register.D);
                    break;
                case 0x1C:
                    INC_r(Register.E);
                    break;
                case 0x1E:
                    LD_r_n(Register.E);
                    break;
                case 0x20:
                    var eMinusTwo = (sbyte)_mmu.GetByte((ushort)(ProgramCounter + 1));
                    var e = eMinusTwo + 2;
                    //Move instrunction length 2 and cycle 2 into metadata
                    ProgramCounter += (ushort)(Z == 0 ? e : 2);
                    Cycles += Z == 0 ? 3 : 2;
                    break;
                case 0x21:
                    L = _mmu.GetByte((ushort)(ProgramCounter + 1));
                    H = _mmu.GetByte((ushort)(ProgramCounter + 2));
                    break;
                case 0x26:
                    LD_r_n(Register.H);
                    break;
                case 0x2A:
                    A = _mmu.GetByte(HL);
                    var increment = HL + 1;
                    L = (byte)(increment & 0xFF);
                    H = (byte)((increment >> 8) & 0xFF);
                    break;
                case 0x2E:
                    LD_r_n(Register.L);
                    break;
                case 0x31:
                    var low = _mmu.GetByte((ushort) (ProgramCounter + 1));
                    var high = _mmu.GetByte((ushort) (ProgramCounter + 2));
                    SP = (ushort) ((high << 8) | low);
                    break;
                case 0x3E:
                    LD_r_n(Register.A);
                    break;
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
                case 0x46:
                    LD_r_HL(Register.B);
                    break;
                case 0x47: //LD_B_A
                    LD_r_r(Register.B, Register.A);
                    break;
                case 0x70:
                    LD_HL_r(Register.B);
                    break;
                case 0x71:
                    LD_HL_r(Register.C);
                    break;
                case 0x72:
                    LD_HL_r(Register.D);
                    break;
                case 0x73:
                    LD_HL_r(Register.E);
                    break;
                case 0x74:
                    LD_HL_r(Register.H);
                    break;
                case 0x75:
                    LD_HL_r(Register.L);
                    break;
                case 0x77:
                    LD_HL_r(Register.A);
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
                case 0x4E:
                    LD_r_HL(Register.C);
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
                    LD_r_HL(Register.D);
                    break;
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
                    LD_r_HL(Register.E);
                    break;
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
                    LD_r_HL(Register.H);
                    break;
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
                    LD_r_HL(Register.L);
                    break;
                case 0x6F:
                    LD_r_r(Register.L, Register.A);
                    break;
                case 0x7E:
                    LD_r_HL(Register.A);
                    break;
                case 0xC3:
                    var l = _mmu.GetByte((ushort)(ProgramCounter + 1));
                    var h = _mmu.GetByte((ushort)(ProgramCounter + 2));
                    ProgramCounter = (ushort)((h << 8) | l);
                    break;
                case 0xEA:
                    _mmu.SetByte(
                        (ushort) ((_mmu.GetByte((ushort) (ProgramCounter + 2)) << 8) | _mmu.GetByte((ushort) (ProgramCounter + 1))),
                        A);
                    break;
                case 0xF3:
                    break;
                default:
                    throw new IllegalOpcodeException(opcode);
            }

            if (_instructionMetaData.ContainsKey(opcode))
            {
                ProgramCounter += _instructionMetaData[opcode].Size;
                Cycles += _instructionMetaData[opcode].Cycles;
            }
        }

        private void LD_HL_r(Register register)
        {
            var target = HL;
            var value = _registers[(int)register];
            _mmu.SetByte(target, value);
        }

        private void LD_r_HL(Register register)
        {
            var n = _mmu.GetByte(HL);
            _registers[(int)register] = n;
        }

        private ushort HL
        {
            get { return (ushort)(H << 8 | L); }
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

        public ushort SP { get; set; }

        public ushort ProgramCounter { get; set; }
        public long Cycles { get; set; }

        public byte Z { get; set; }
        public byte N { get; set; }
        public byte HC { get; set; }
    }

    public class IllegalOpcodeException : Exception
    {
        private readonly byte _opcode;

        public IllegalOpcodeException(byte opcode)
        {
            _opcode = opcode;
        }

        public override string Message
        {
            get { return string.Format("Illegal opcode 0x{0:x2}", _opcode); }
        }
    }
}
