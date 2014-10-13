using System;
using System.Collections.Generic;

namespace Core
{
    public class Register
    {
        public static readonly Register A = new Register(0);
        public static readonly Register B = new Register(1);
        public static readonly Register C = new Register(2);
        public static readonly Register D = new Register(3);
        public static readonly Register E = new Register(4);
        public static readonly Register H = new Register(5);
        public static readonly Register L = new Register(6);

        private Register(int index)
        {
            _index = index;
        }

        private readonly int _index;

        public static implicit operator int(Register register)
        {
            return register._index;
        }
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
            { 0x01, new InstructionMetaData(3, 3, "LD BC, nn")},
            { 0x02, new InstructionMetaData(1, 2, "LD (BC), A")},
            { 0x03, new InstructionMetaData(1, 2, "INC BC")},
            { 0x04, new InstructionMetaData(1, 1, "INC B")},
            { 0x05, new InstructionMetaData(1, 1, "DEC B")},
            { 0x06, new InstructionMetaData(2, 2, "LD B, n")},
            { 0x07, new InstructionMetaData(1, 1, "RLCA")},
            { 0x08, new InstructionMetaData(3, 5, "LD (nn), SP")},
            { 0x09, new InstructionMetaData(1, 2, "ADD HL, BC")},
            { 0x0A, new InstructionMetaData(1, 2, "LD A, (BC)")},
            { 0x0B, new InstructionMetaData(1, 2, "DEC BC")},
            { 0x0C, new InstructionMetaData(1, 1, "INC C")},
            { 0x0D, new InstructionMetaData(1, 1, "DEC C")},
            { 0x0E, new InstructionMetaData(2, 2, "LD C, n")},
            { 0x0F, new InstructionMetaData(1, 1, "RRCA")},
            { 0x11, new InstructionMetaData(3, 3, "LD DE, nn")},
            { 0x12, new InstructionMetaData(1, 2, "LD (DE), A")},
            { 0x13, new InstructionMetaData(1, 2, "INC DE")},
            { 0x14, new InstructionMetaData(1, 1, "INC D")},
            { 0x15, new InstructionMetaData(1, 1, "DEC D")},
            { 0x16, new InstructionMetaData(2, 2, "LD D, n")},
            { 0x17, new InstructionMetaData(1, 1, "RLA")},
            { 0x18, new InstructionMetaData(0, 3, "JR, $+e")},
            { 0x19, new InstructionMetaData(1, 2, "ADD HL, DE")},
            { 0x1A, new InstructionMetaData(1, 2, "LD A, (DE)")},
            { 0x1B, new InstructionMetaData(1, 2, "DEC DE")},
            { 0x1C, new InstructionMetaData(1, 1, "INC E")},
            { 0x1D, new InstructionMetaData(1, 1, "DEC E")},
            { 0x1E, new InstructionMetaData(2, 2, "LD E, n")},
            { 0x1F, new InstructionMetaData(1, 1, "RRA")},
            { 0x21, new InstructionMetaData(3, 3, "LD HL, nn")},
            { 0x22, new InstructionMetaData(1, 2, "LD (HLI), A")},
            { 0x23, new InstructionMetaData(1, 2, "INC HL")},
            { 0x24, new InstructionMetaData(1, 1, "INC H")},
            { 0x25, new InstructionMetaData(1, 1, "DEC H")},
            { 0x26, new InstructionMetaData(2, 2, "LD H, n")},
            { 0x28, new InstructionMetaData(0, 0, "JR Z, $ + e")},
            { 0x29, new InstructionMetaData(1, 2, "ADD HL, HL")},
            { 0x2A, new InstructionMetaData(1, 2, "LD A, (HLI)")},
            { 0x2B, new InstructionMetaData(1, 2, "DEC HL")},
            { 0x2C, new InstructionMetaData(1, 1, "INC L")},
            { 0x2D, new InstructionMetaData(1, 1, "DEC L")},
            { 0x2E, new InstructionMetaData(2, 2, "LD L, n")},
            { 0x2F, new InstructionMetaData(1, 1, "CPL")},
            { 0x30, new InstructionMetaData(0, 0, "JR NC, $+e")},
            { 0x31, new InstructionMetaData(3, 3, "LD SP, nn")},
            { 0x32, new InstructionMetaData(1, 2, "LD (HLD), A")},
            { 0x33, new InstructionMetaData(1, 2, "INC SP")},
            { 0x34, new InstructionMetaData(1, 3, "INC (HL)")},
            { 0x35, new InstructionMetaData(1, 3, "DEC (HL)")},
            { 0x36, new InstructionMetaData(2, 3, "LD (HL), n")},
            { 0x37, new InstructionMetaData(1, 1, "SCF")},
            { 0x38, new InstructionMetaData(0, 0, "JR, C, $+e")},
            { 0x39, new InstructionMetaData(1, 2, "ADD HL, SP")},
            { 0x3A, new InstructionMetaData(1, 2, "LD A, (HL-)")},
            { 0x3B, new InstructionMetaData(1, 2, "DEC SP")},
            { 0x3C, new InstructionMetaData(1, 1, "INC A")},
            { 0x3D, new InstructionMetaData(1, 1, "DEC A")},
            { 0x3E, new InstructionMetaData(2, 2, "LD A, n")},
            { 0x3F, new InstructionMetaData(1, 1, "CCF")},
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
            { 0x76, new InstructionMetaData(1, 1, "HALT")},
            { 0x77, new InstructionMetaData(1, 2, "LD (HL), A")},
            { 0x78, new InstructionMetaData(1, 1, "LD A, B")},
            { 0x79, new InstructionMetaData(1, 1, "LD A, C")},
            { 0x7A, new InstructionMetaData(1, 1, "LD A, D")},
            { 0x7B, new InstructionMetaData(1, 1, "LD A, E")},
            { 0x7C, new InstructionMetaData(1, 1, "LD A, H")},
            { 0x7D, new InstructionMetaData(1, 1, "LD A, L")},
            { 0x7E, new InstructionMetaData(1, 2, "LD A, (HL)")},
            { 0x7F, new InstructionMetaData(1, 1, "LD A, A")},
            { 0x80, new InstructionMetaData(1, 1, "ADD A, B")},
            { 0x81, new InstructionMetaData(1, 1, "ADD A, C")},
            { 0x82, new InstructionMetaData(1, 1, "ADD A, D")},
            { 0x83, new InstructionMetaData(1, 1, "ADD A, E")},
            { 0x84, new InstructionMetaData(1, 1, "ADD A, H")},
            { 0x85, new InstructionMetaData(1, 1, "ADD A, L")},
            { 0x86, new InstructionMetaData(1, 2, "ADD A, (HL)")},
            { 0x87, new InstructionMetaData(1, 1, "ADD A, A")},
            { 0x88, new InstructionMetaData(1, 1, "ADC A, B")},
            { 0x89, new InstructionMetaData(1, 1, "ADC A, C")},
            { 0x8A, new InstructionMetaData(1, 1, "ADC A, D")},
            { 0x8B, new InstructionMetaData(1, 1, "ADC A, E")},
            { 0x8C, new InstructionMetaData(1, 1, "ADC A, H")},
            { 0x8D, new InstructionMetaData(1, 1, "ADC A, L")},
            { 0x8E, new InstructionMetaData(1, 2, "ADC A, (HL)")},
            { 0x8F, new InstructionMetaData(1, 1, "ADC A, A")},
            { 0x90, new InstructionMetaData(1, 1, "SUB B")},
            { 0x91, new InstructionMetaData(1, 1, "SUB C")},
            { 0x92, new InstructionMetaData(1, 1, "SUB D")},
            { 0x93, new InstructionMetaData(1, 1, "SUB E")},
            { 0x94, new InstructionMetaData(1, 1, "SUB H")},
            { 0x95, new InstructionMetaData(1, 1, "SUB L")},
            { 0x97, new InstructionMetaData(1, 1, "SUB A")},
            { 0x98, new InstructionMetaData(1, 1, "SBC B")},
            { 0x99, new InstructionMetaData(1, 1, "SBC C")},
            { 0x9A, new InstructionMetaData(1, 1, "SBC D")},
            { 0x9B, new InstructionMetaData(1, 1, "SBC E")},
            { 0x9C, new InstructionMetaData(1, 1, "SBC H")},
            { 0x9D, new InstructionMetaData(1, 1, "SBC L")},
            { 0x9F, new InstructionMetaData(1, 1, "SBC A")},
            { 0xA0, new InstructionMetaData(1, 1, "AND B")},
            { 0xA1, new InstructionMetaData(1, 1, "AND C")},
            { 0xA2, new InstructionMetaData(1, 1, "AND D")},
            { 0xA3, new InstructionMetaData(1, 1, "AND E")},
            { 0xA4, new InstructionMetaData(1, 1, "AND H")},
            { 0xA5, new InstructionMetaData(1, 1, "AND L")},
            { 0xA7, new InstructionMetaData(1, 1, "AND A")},
            { 0xA8, new InstructionMetaData(1, 1, "XOR B")},
            { 0xAA, new InstructionMetaData(1, 1, "XOR D")},
            { 0xAB, new InstructionMetaData(1, 1, "XOR E")},
            { 0xAC, new InstructionMetaData(1, 1, "XOR H")},
            { 0xAD, new InstructionMetaData(1, 1, "XOR L")},
            { 0xAE, new InstructionMetaData(1, 2, "XOR (HL)")},
            { 0xAF, new InstructionMetaData(1, 1, "XOR A")},
            { 0xB0, new InstructionMetaData(1, 1, "OR B")},
            { 0xB1, new InstructionMetaData(1, 1, "OR C")},
            { 0xB2, new InstructionMetaData(1, 1, "OR D")},
            { 0xB3, new InstructionMetaData(1, 1, "OR E")},
            { 0xB4, new InstructionMetaData(1, 1, "OR H")},
            { 0xB5, new InstructionMetaData(1, 1, "OR L")},
            { 0xB6, new InstructionMetaData(1, 2, "OR (HL)")},
            { 0xB7, new InstructionMetaData(1, 1, "OR A")},
            { 0xB8, new InstructionMetaData(1, 1, "CP B")},
            { 0xB9, new InstructionMetaData(1, 1, "CP C")},
            { 0xBA, new InstructionMetaData(1, 1, "CP D")},
            { 0xBB, new InstructionMetaData(1, 1, "CP E")},
            { 0xBC, new InstructionMetaData(1, 1, "CP H")},
            { 0xBD, new InstructionMetaData(1, 1, "CP L")},
            { 0xBE, new InstructionMetaData(1, 2, "CP (HL)")},
            { 0xBF, new InstructionMetaData(1, 1, "CP A")},
            { 0xC0, new InstructionMetaData(0, 0, "RET NZ")},
            { 0xC1, new InstructionMetaData(1, 3, "POP BC")},
            { 0xC2, new InstructionMetaData(0, 0, "JP NZ, nn")},
            { 0xC3, new InstructionMetaData(0, 4, "JP, nn")},
            { 0xC4, new InstructionMetaData(0, 0, "CALL NZ, nn")},
            { 0xC5, new InstructionMetaData(1, 4, "PUSH BC")},
            { 0xC6, new InstructionMetaData(2, 2, "ADD A, n")},
            { 0xC7, new InstructionMetaData(0, 4, "RST 00H")},
            { 0xC8, new InstructionMetaData(0, 0, "RET Z")},
            { 0xC9, new InstructionMetaData(0, 4, "RET")},
            { 0xCA, new InstructionMetaData(0, 0, "JP Z, nn")},
            { 0xCC, new InstructionMetaData(0, 0, "CALL Z, nn")},
            { 0xCD, new InstructionMetaData(0, 6, "CALL, nn")},
            { 0xCE, new InstructionMetaData(2, 2, "ADC n")},
            { 0xCF, new InstructionMetaData(0, 4, "RST 08H")},
            { 0xD0, new InstructionMetaData(0, 0, "RET NC")},
            { 0xD1, new InstructionMetaData(1, 3, "POP DE")},
            { 0xD2, new InstructionMetaData(0, 0, "JP NC, nn")},
            { 0xD4, new InstructionMetaData(0, 0, "CALL NC, nn")},
            { 0xD5, new InstructionMetaData(1, 4, "PUSH DE")},
            { 0xD6, new InstructionMetaData(2, 2, "SUB n")},
            { 0xD7, new InstructionMetaData(0, 4, "RST 10H")},
            { 0xD8, new InstructionMetaData(0, 0, "RET C")},
            { 0xD9, new InstructionMetaData(0, 4, "RETI")},
            { 0xDA, new InstructionMetaData(0, 0, "JP C, nn")},
            { 0xDC, new InstructionMetaData(0, 0, "CALL C, nn")},
            { 0xDF, new InstructionMetaData(0, 4, "RST 18H")},
            { 0xE0, new InstructionMetaData(2, 3, "LD (FFn), A")},
            { 0xE1, new InstructionMetaData(1, 3, "POP HL")},
            { 0xE2, new InstructionMetaData(1, 2, "LD (C), A")},
            { 0xE5, new InstructionMetaData(1, 4, "PUSH HL")},
            { 0xE7, new InstructionMetaData(0, 4, "RST 20H")},
            { 0xE8, new InstructionMetaData(2, 4, "ADD SP, n")},
            { 0xE9, new InstructionMetaData(0, 1, "JP HL")},
            { 0xEA, new InstructionMetaData(3, 4, "LD (nn), A")},
            { 0xEE, new InstructionMetaData(2, 2, "XOR n")},
            { 0xEF, new InstructionMetaData(0, 4, "RST 28H")},
            { 0xF0, new InstructionMetaData(2, 3, "LD A, (n)")},
            { 0xF1, new InstructionMetaData(1, 3, "POP AF")},
            { 0xF2, new InstructionMetaData(1, 2, "LD A, (C)")},
            { 0xF3, new InstructionMetaData(1, 1, "DI")},
            { 0xF5, new InstructionMetaData(1, 4, "PUSH AF")},
            { 0xF6, new InstructionMetaData(2, 2, "OR n")},
            { 0xF7, new InstructionMetaData(0, 4, "RST 30H")},
            { 0xF8, new InstructionMetaData(2, 3, "LD HL, SP+e")},
            { 0xF9, new InstructionMetaData(1, 2, "LD SP, HL")},
            { 0xFA, new InstructionMetaData(3, 4, "LD A, (nn)")},
            { 0xFB, new InstructionMetaData(1, 1, "EI")},
            { 0xFE, new InstructionMetaData(2, 2, "CP n")},
            { 0xFF, new InstructionMetaData(0, 4, "RST 38H")},
        };

        private readonly IMmu _mmu;

        public Cpu(IMmu mmu)
        {
            _mmu = mmu;
        }

        private void LD_r_r(Register target, Register source)
        {
            _registers[target] = _registers[source];
        }

        private void LD_r_n(Register target)
        {
            var n = _mmu.GetByte((ushort)(ProgramCounter + 1));
            _registers[target] = n;
        }

        private void INC_r(Register register)
        {
            HC = (byte)((((_registers[register] & 0xF) + 1) & 0x10) == 0 ? 0 : 1);
            _registers[register] = (byte)(_registers[register] + 1);
            Z = (byte)(_registers[register] == 0 ? 1 : 0);
            N = 0;
        }

        public void Execute(byte opcode)
        {
            var maskedIE = IE & 0x1F;
            var maskedIF = IF & 0x1F;

            var interrupted = maskedIE & maskedIF;
            if (Halted && interrupted == 0)
            {
                Cycles += 1;
                return;
            }

            if (Halted)
            {
                Halted = false;
            }

            if (IME && interrupted > 0)
            {
                IME = false;
                //Push program counter to stack
                _mmu.SetByte((ushort)(SP - 1), (byte)(ProgramCounter >> 8));
                _mmu.SetByte((ushort)(SP - 2), (byte)ProgramCounter);
                SP -= 2;

                const int vblank = 0x01;
                if ((interrupted & vblank) == vblank)
                {
                    ProgramCounter = 0x40;
                    IF = (byte)(IF & ~vblank);
                    return;
                }

                const int lcdStat = 0x02;
                if ((interrupted & lcdStat) == lcdStat)
                {
                    ProgramCounter = 0x48;
                    IF = (byte) (IF & ~lcdStat);
                    return;
                }

                const int timer = 0x04;
                if ((interrupted & timer) == timer)
                {
                    ProgramCounter = 0x50;
                    IF = (byte)(IF & ~timer);
                    return;
                }

                const int serial = 0x08;
                if ((interrupted & serial) == serial)
                {
                    ProgramCounter = 0x58;
                    IF = (byte) (IF & ~serial);
                    return;
                }

                const int joyPad = 0x10;
                if ((interrupted & joyPad) == joyPad)
                {
                    ProgramCounter = 0x60;
                    IF = (byte) (IF & ~joyPad);
                    return;
                }
            }

            switch (opcode)
            {
                case 0x00:
                    break;
                case 0x01:
                    C = _mmu.GetByte((ushort)(ProgramCounter + 1));
                    B = _mmu.GetByte((ushort)(ProgramCounter + 2));
                    break;
                case 0x02:
                    _mmu.SetByte((ushort) (B << 8 | C), A);
                    break;
                case 0x03:
                    var newValue = (B << 8 | C) + 1;
                    B = (byte)((newValue >> 8) & 0xFF);
                    C = (byte)(newValue & 0xFF);
                    break;
                case 0x04:
                    INC_r(Register.B);
                    break;
                case 0x05:
                    DEC_r(Register.B);
                    break;
                case 0x06:
                    LD_r_n(Register.B);
                    break;
                case 0x07:
                    RLCA();
                    break;
                case 0x08:
                    LD_NN_SP();
                    break;
                case 0x09:
                    ADD_HL_ss((ushort)(_registers[Register.B] << 8 | _registers[Register.C]));
                    break;
                case 0x0A:
                    LD_A_BCm();
                    break;
                case 0x0B:
                    DEC_ss(Register.B, Register.C);
                    break;
                case 0x0c:
                    INC_r(Register.C);
                    break;
                case 0x0D:
                    DEC_r(Register.C);
                    break;
                case 0x0E:
                    LD_r_n(Register.C);
                    break;
                case 0x0F:
                    RRCA();
                    break;
                case 0x11:
                    E = _mmu.GetByte((ushort)(ProgramCounter + 1));
                    D = _mmu.GetByte((ushort)(ProgramCounter + 2));
                    break;
                case 0x12:
                    var target = (D << 8 | E);
                    _mmu.SetByte((ushort)target, A);
                    break;
                case 0x13:
                    var res2 = (D << 8 | E) + 1;
                    D = (byte)((res2 >> 8) & 0xFF);
                    E = (byte)(res2 & 0xFF);
                    break;
                case 0x14:
                    INC_r(Register.D);
                    break;
                case 0x15:
                    DEC_r(Register.D);
                    break;
                case 0x16:
                    LD_r_n(Register.D);
                    break;
                case 0x17:
                    RLA();
                    break;
                case 0x18:
                    JR_e();
                    break;
                case 0x19:
                    ADD_HL_ss((ushort)(_registers[Register.D] << 8 | _registers[Register.E]));
                    break;
                case 0x1A:
                    A = _mmu.GetByte((ushort)(D << 8 | E));
                    break;
                case 0x1B:
                    DEC_ss(Register.D, Register.E);
                    break;
                case 0x1C:
                    INC_r(Register.E);
                    break;
                case 0x1D:
                    DEC_r(Register.E);
                    break;
                case 0x1E:
                    LD_r_n(Register.E);
                    break;
                case 0x1F:
                    RRA();
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
                case 0x22:
                    _mmu.SetByte(HL, A);
                    var nextHL = HL + 1;
                    H = (byte)(nextHL >> 8);
                    L = (byte)nextHL;
                    break;
                case 0x23:
                    var result = HL + 1;
                    H = (byte)((result >> 8) & 0xFF);
                    L = (byte)(result & 0xFF);
                    break;
                case 0x24:
                    INC_r(Register.H);
                    break;
                case 0x25:
                    DEC_r(Register.H);
                    break;
                case 0x26:
                    LD_r_n(Register.H);
                    break;
                case 0x28:
                    var n = ((sbyte)_mmu.GetByte((ushort)(ProgramCounter + 1))) + 2;
                    ProgramCounter += (ushort)(Z == 1 ? n : 2);
                    Cycles += Z == 1 ? 3 : 2;
                    break;
                case 0x29:
                    ADD_HL_ss(HL);
                    break;
                case 0x2A:
                    A = _mmu.GetByte(HL);
                    var increment = HL + 1;
                    L = (byte)(increment & 0xFF);
                    H = (byte)((increment >> 8) & 0xFF);
                    break;
                case 0x2B:
                    DEC_ss(Register.H, Register.L);
                    break;
                case 0x2C:
                    INC_r(Register.L);
                    break;
                case 0x2D:
                    DEC_r(Register.L);
                    break;
                case 0x2E:
                    LD_r_n(Register.L);
                    break;
                case 0x2F:
                    CPL();
                    break;
                case 0x30:
                    JR_NC();
                    break;
                case 0x31:
                    var low = _mmu.GetByte((ushort)(ProgramCounter + 1));
                    var high = _mmu.GetByte((ushort)(ProgramCounter + 2));
                    SP = (ushort)((high << 8) | low);
                    break;
                case 0x32:
                    _mmu.SetByte(HL, A);
                    var nextHLD = HL - 1;
                    H = (byte)(nextHLD >> 8);
                    L = (byte)nextHLD;
                    break;
                case 0x33:
                    INC_SP();
                    break;
                case 0x34:
                    INC_HLm();
                    break;
                case 0x35:
                    DEC_HL();
                    break;
                case 0x36:
                    LD_HLm_n();
                    break;
                case 0x37:
                    SCF();
                    break;
                case 0x38:
                    JR_C();
                    break;
                case 0x39:
                    ADD_HL_ss(SP);
                    break;
                case 0x3A:
                    LD_A_HLD();
                    break;
                case 0x3B:
                    DEC_SP();
                    break;
                case 0x3C:
                    INC_r(Register.A);
                    break;
                case 0x3D:
                    DEC_r(Register.A);
                    break;
                case 0x3E:
                    LD_r_n(Register.A);
                    break;
                case 0x3F:
                    CCF();
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
                case 0x76:
                    Halt();
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
                case 0x80:
                    ADD_r(Register.B);
                    break;
                case 0x81:
                    ADD_r(Register.C);
                    break;
                case 0x82:
                    ADD_r(Register.D);
                    break;
                case 0x83:
                    ADD_r(Register.E);
                    break;
                case 0x84:
                    ADD_r(Register.H);
                    break;
                case 0x85:
                    ADD_r(Register.L);
                    break;
                case 0x86:
                    ADD_A_HL();
                    break;
                case 0x87:
                    ADD_r(Register.A);
                    break;
                case 0x88:
                    ADC_r(Register.B);
                    break;
                case 0x89:
                    ADC_r(Register.C);
                    break;
                case 0x8A:
                    ADC_r(Register.D);
                    break;
                case 0x8B:
                    ADC_r(Register.E);
                    break;
                case 0x8C:
                    ADC_r(Register.H);
                    break;
                case 0x8D:
                    ADC_r(Register.L);
                    break;
                case 0x8E:
                    ADC_A_HLm();
                    break;
                case 0x8F:
                    ADC_r(Register.A);
                    break;
                case 0x90:
                    SUB(Register.B);
                    break;
                case 0x91:
                    SUB(Register.C);
                    break;
                case 0x92:
                    SUB(Register.D);
                    break;
                case 0x93:
                    SUB(Register.E);
                    break;
                case 0x94:
                    SUB(Register.H);
                    break;
                case 0x95:
                    SUB(Register.L);
                    break;
                case 0x97:
                    SUB(Register.A);
                    break;
                case 0x98:
                    SBC_r(Register.B);
                    break;
                case 0x99:
                    SBC_r(Register.C);
                    break;
                case 0x9A:
                    SBC_r(Register.D);
                    break;
                case 0x9B:
                    SBC_r(Register.E);
                    break;
                case 0x9C:
                    SBC_r(Register.H);
                    break;
                case 0x9D:
                    SBC_r(Register.L);
                    break;
                case 0x9F:
                    SBC_r(Register.A);
                    break;
                case 0xA0:
                    AND_r(Register.B);
                    break;
                case 0xA1:
                    AND_r(Register.C);
                    break;
                case 0xA2:
                    AND_r(Register.D);
                    break;
                case 0xA3:
                    AND_r(Register.E);
                    break;
                case 0xA4:
                    AND_r(Register.H);
                    break;
                case 0xA5:
                    AND_r(Register.L);
                    break;
                case 0xA7:
                    AND_r(Register.A);
                    break;
                case 0xAD:
                    XOR(Register.L);
                    break;
                case 0xA8:
                    XOR(Register.B);
                    break;
                case 0xA9:
                    A = (byte)(A ^ C);
                    N = 0;
                    Carry = 0;
                    HC = 0;
                    Z = (byte)(A == 0 ? 1 : 0);
                    ProgramCounter += 1;
                    Cycles += 1;
                    break;
                case 0xAA:
                    XOR(Register.D);
                    break;
                case 0xAB:
                    XOR(Register.E);
                    break;
                case 0xAC:
                    XOR(Register.H);
                    break;
                case 0xAE:
                    XOR(_mmu.GetByte(HL));
                    break;
                case 0xAF:
                    XOR(Register.A);
                    break;
                case 0xB0:
                    OR_r(Register.B);
                    break;
                case 0xB1:
                    OR_r(Register.C);
                    break;
                case 0xB2:
                    OR_r(Register.D);
                    break;
                case 0xB3:
                    OR_r(Register.E);
                    break;
                case 0xB4:
                    OR_r(Register.H);
                    break;
                case 0xB5:
                    OR_r(Register.L);
                    break;
                case 0xB6:
                    OR_HL();
                    break;
                case 0xB7:
                    OR_r(Register.A);
                    break;
                case 0xB8:
                    CP_r(Register.B);
                    break;
                case 0xB9:
                    CP_r(Register.C);
                    break;
                case 0xBA:
                    CP_r(Register.D);
                    break;
                case 0xBB:
                    CP_r(Register.E);
                    break;
                case 0xBC:
                    CP_r(Register.H);
                    break;
                case 0xBD:
                    CP_r(Register.L);
                    break;
                case 0xBE:
                    CP_HL();
                    break;
                case 0xBF:
                    CP_r(Register.A);
                    break;
                case 0xC0:
                    RET_cc(Z == 0);
                    break;
                case 0xC1:
                    C = _mmu.GetByte(SP);
                    B = _mmu.GetByte((ushort)(SP + 1));
                    SP += 2;
                    break;
                case 0xC2:
                    JP_NZ();
                    break;
                case 0xC3:
                    var l = _mmu.GetByte((ushort)(ProgramCounter + 1));
                    var h = _mmu.GetByte((ushort)(ProgramCounter + 2));
                    ProgramCounter = (ushort)((h << 8) | l);
                    break;
                case 0xC4:
                    Call_cc(Z == 0);
                    break;
                case 0xC5:
                    _mmu.SetByte((ushort)(SP - 1), B);
                    _mmu.SetByte((ushort)(SP - 2), C);
                    SP -= 2;
                    break;
                case 0xC7:
                    RST(0x00);
                    break;
                case 0xC8:
                    RET_cc(Z == 1);
                    break;
                case 0xC6:
                    ADD_n();
                    break;
                case 0xC9:
                    RET();
                    break;
                case 0xCA:
                    JP_Z();
                    break;
                case 0xCB:
                    CB();
                    break;
                case 0xCC:
                    CALL_Z();
                    break;
                case 0xCE:
                    ADC();
                    break;
                case 0xCD:
                    Call();
                    break;
                case 0xCF:
                    RST(0x08);
                    break;
                case 0xD0:
                    RET_cc(Carry == 0);
                    break;
                case 0xD1:
                    E = _mmu.GetByte(SP);
                    D = _mmu.GetByte((ushort)(SP + 1));
                    SP += 2;
                    break;
                case 0xD2:
                    JP_NC();
                    break;
                case 0xD4:
                    Call_cc(Carry == 0);
                    break;
                case 0xD5:
                    _mmu.SetByte((ushort)(SP - 1), D);
                    _mmu.SetByte((ushort)(SP - 2), E);
                    SP -= 2;
                    break;
                case 0xD6:
                    SUB_n();
                    break;
                case 0xD7:
                    RST(0x10);
                    break;
                case 0xD8:
                    RET_cc(Carry == 1);
                    break;
                case 0xD9:
                    RETI();
                    break;
                case 0xDA:
                    JP_C();
                    break;
                case 0xDC:
                    Call_cc(Carry == 1);
                    break;
                case 0xDF:
                    RST(0x18);
                    break;
                case 0xE0:
                    _mmu.SetByte((ushort)(0xFF00 | _mmu.GetByte((ushort)(ProgramCounter + 1))), A);
                    break;
                case 0xE1:
                    L = _mmu.GetByte(SP);
                    H = _mmu.GetByte((ushort)(SP + 1));
                    SP += 2;
                    break;
                case 0xE2:
                    _mmu.SetByte((ushort)(0xFF00 + _registers[Register.C]), _registers[Register.A]);
                    break;
                case 0xE5:
                    _mmu.SetByte((ushort)(SP - 1), H);
                    _mmu.SetByte((ushort)(SP - 2), L);
                    SP -= 2;
                    break;
                case 0xE6:
                    A = (byte)(A & _mmu.GetByte((ushort)(ProgramCounter + 1)));
                    Carry = 0;
                    N = 0;
                    HC = 1;
                    Z = (byte)(A == 0 ? 1 : 0);
                    ProgramCounter += 2;
                    Cycles += 2;
                    break;
                case 0xE7:
                    RST(0x20);
                    break;
                case 0xE8:
                    ADD_SP_n();
                    break;
                case 0xE9:
                    JP_HL();
                    break;
                case 0xEA:
                    _mmu.SetByte(
                        (ushort)((_mmu.GetByte((ushort)(ProgramCounter + 2)) << 8) | _mmu.GetByte((ushort)(ProgramCounter + 1))),
                        A);
                    break;
                case 0xEE:
                    A = (byte)(A ^ _mmu.GetByte((ushort)(ProgramCounter + 1)));
                    N = 0;
                    HC = 0;
                    Carry = 0;
                    Z = (byte)(A == 0 ? 1 : 0);
                    break;
                case 0xEF:
                    RST(0x28);
                    break;
                case 0xF0:
                    var offset = _mmu.GetByte((ushort)(ProgramCounter + 1));
                    A = _mmu.GetByte((ushort)(0xFF00 + offset));
                    break;
                case 0xF1:
                    F = (byte)(_mmu.GetByte(SP) & 0xF0);
                    A = _mmu.GetByte((ushort)(SP + 1));
                    SP += 2;
                    break;
                case 0xF2:
                    LD_A_C();
                    break;
                case 0xF3:
                    DI();
                    break;
                case 0xF5:
                    _mmu.SetByte((ushort)(SP - 1), A);
                    _mmu.SetByte((ushort)(SP - 2), F);
                    SP -= 2;
                    break;
                case 0xF6:
                    OR_n();
                    break;
                case 0xF7:
                    RST(0x30);
                    break;
                case 0xF8:
                    LD_HL_SP_e();
                    break;
                case 0xF9:
                    LD_SP_HL();
                    break;
                case 0xFA:
                    var address = _mmu.GetByte((ushort)(ProgramCounter + 1)) | (_mmu.GetByte((ushort)(ProgramCounter + 2)) << 8);
                    A = _mmu.GetByte((ushort)address);
                    break;
                case 0xFB:
                    EI();
                    break;
                case 0xFE:
                    var value = _mmu.GetByte((ushort)(ProgramCounter + 1));
                    CP(value);
                    break;
                case 0xFF:
                    RST(0x38);
                    break;
                default:
                    throw new IllegalOpcodeException(opcode, ProgramCounter);
            }

            if (_instructionMetaData.ContainsKey(opcode))
            {
                ProgramCounter += _instructionMetaData[opcode].Size;
                Cycles += _instructionMetaData[opcode].Cycles;
            }
        }

        private void ADC_A_HLm()
        {
            var arg = _mmu.GetByte(HL);
            ADC(arg);
        }

        private void LD_A_HLD()
        {
            A = _mmu.GetByte(HL);
            var newValue = (ushort)(HL - 1);
            H = (byte) (newValue >> 8);
            L = (byte) (newValue);
        }

        private void LD_A_BCm()
        {
            var address = (B << 8 | C);
            A = _mmu.GetByte((ushort) address);
        }

        private void OR_n()
        {
            OR_A(_mmu.GetByte((ushort) (ProgramCounter + 1)));
        }

        private void INC_HLm()
        {
            var value = _mmu.GetByte(HL);
            var result = (byte) (value + 1);
            _mmu.SetByte(HL, result);
            N = 0;
            HC = (byte) ((value & 0xF) == 0xF ? 1 : 0);
            Z = (byte) (result == 0 ? 1 : 0);
        }

        private void LD_HLm_n()
        {
            var value = _mmu.GetByte((ushort)(ProgramCounter + 1));
            _mmu.SetByte(HL, value);
        }

        private void ADD_SP_n()
        {
            N = 0;
            Z = 0;
            var e = (sbyte)_mmu.GetByte((ushort)(ProgramCounter + 1));
            var n = _mmu.GetByte((ushort)(ProgramCounter + 1));

            var result = SP + e;
            var check = ((SP & 0xFF) + n) & 0x100;
            Carry = (byte)(check == 0x100 ? 1 : 0);

            check = ((SP & 0xF) + (n & 0xF)) & 0x10;
            HC = (byte)(check == 0x10 ? 1 : 0);

            SP = (ushort)result;
        }

        private void ADD_HL_ss(ushort value)
        {
            N = 0;
            var result = HL + value;
            HC = (byte)((((HL & 0xFFF) + (value & 0xFFF)) & 0x1000) == 0x1000 ? 1 : 0);
            Carry = (byte)(result > 0xFFFF ? 1 : 0);
            H = (byte)(result >> 8);
            L = (byte)(result & 0xFF);
        }

        private void DEC_ss(Register h, Register l)
        {
            var result = ((_registers[h] << 8) | _registers[l]) - 1;
            _registers[h] = (byte)(result >> 8);
            _registers[l] = (byte)(result & 0xFF);
        }

        private void DEC_SP()
        {
            SP -= 1;
        }

        private void INC_SP()
        {
            SP += 1;
        }

        private void Halt()
        {
            Halted = true;
        }

        private void LD_HL_SP_e()
        {
            Z = 0;
            N = 0;
            var n = _mmu.GetByte((ushort)(ProgramCounter + 1));
            var e = (sbyte)n;
            var result = SP + e;
            H = (byte)((result >> 8) & 0xFF);
            L = (byte)(result & 0xFF);
            Carry = (byte)((SP & 0xFF) + n > 0xFF ? 1 : 0);
            HC = (byte)((SP & 0xF) + (n & 0xF) > 0xF ? 1 : 0);
        }

        private void EI()
        {
            IME = true;
        }

        private void DI()
        {
            IME = false;
        }

        private void RRA()
        {
            RR_r(Register.A);
            Z = 0;
        }

        private void RRCA()
        {
            var bit0 = A & 0x01;
            A = (byte)((bit0 << 7) | (A >> 1));
            Carry = (byte)bit0;
            Z = 0;
            HC = 0;
            N = 0;
        }

        private void RLA()
        {
            RL_r(Register.A);
            Z = 0;
        }

        private void RLCA()
        {
            var bit8 = (A & 0x80) >> 7;
            A = (byte)((A << 1) | bit8);
            Carry = (byte)bit8;
            N = 0;
            Z = 0;
            HC = 0;
        }

        private void AND_r(Register register)
        {
            var result = A & _registers[register];
            A = (byte)result;
            Z = (byte)(result == 0 ? 1 : 0);
            N = 0;
            HC = 1;
            Carry = 0;
        }

        private void SBC_r(Register register)
        {
            SBC(_registers[register]);
        }

        private void SBC(byte value)
        {
            var result = A - value - Carry;
            HC = (byte)((A & 0x0F) < ((value & 0x0F) + Carry) ? 1 : 0);
            A = (byte)result;
            Z = (byte)(A == 0 ? 1 : 0);
            N = 1;
            Carry = (byte)(result < 0 ? 1 : 0);
        }

        private void ADC_r(Register register)
        {
            var arg = _registers[register];
            ADC(arg);
        }

        private void ADC(byte value)
        {
            var result = A + value + Carry;
            HC = (byte)(((0x0F & A) + (0x0F & value) + Carry) > 0xF ? 1 : 0);
            A = (byte)result;
            N = 0;
            Z = (byte)(A == 0 ? 1 : 0);
            Carry = (byte)(result > 255 ? 1 : 0);
        }

        private void CCF()
        {
            N = 0;
            HC = 0;
            Carry = (byte)~Carry;
        }

        private void CPL()
        {
            HC = 1;
            N = 1;
            A = (byte)~A;
        }

        private void SUB_n()
        {
            var arg = _mmu.GetByte((ushort)(ProgramCounter + 1));
            SUB(arg);
        }

        private void ADD_n()
        {
            var arg = _mmu.GetByte((ushort)(ProgramCounter + 1));
            Add(arg);
        }

        private void SCF()
        {
            N = 0;
            HC = 0;
            Carry = 1;
        }

        private void LD_A_C()
        {
            var result = _mmu.GetByte((ushort)(0xFF00 + _registers[Register.C]));
            A = result;
        }

        private void ADD_A_HL()
        {
            var value = _mmu.GetByte(HL);
            Add(value);
        }

        private void CP_HL()
        {
            var value = _mmu.GetByte(HL);
            CP(value);
        }

        private void ADD_r(Register register)
        {
            var value = _registers[register];
            Add(value);
        }

        private void Add(byte value)
        {
            Carry = (byte)(A + value > 255 ? 1 : 0);
            HC = (byte)((value & 0x0F) + (A & 0x0F) > 15 ? 1 : 0);
            A += value;
            N = 0;

            Z = (byte)(A == 0 ? 1 : 0);
        }

        private void SUB(Register register)
        {
            var value = _registers[register];
            SUB(value);
        }

        private void SUB(byte value)
        {
            HC = (byte)((A & 0xF) < (value & 0xF) ? 1 : 0);
            var result = A - value;
            A = (byte)result;
            Z = (byte)(result == 0 ? 1 : 0);
            Carry = (byte)(result < 0 ? 1 : 0);
            N = 1;
        }

        private void CP_r(Register register)
        {
            var value = _registers[register];
            CP(value);
        }

        private void CP(byte value)
        {
            var result = A - value;
            Z = (byte)(result == 0 ? 1 : 0);
            Carry = (byte)(result < 0 ? 1 : 0);
            HC = (byte)((A & 0xF) < (value & 0xF) ? 1 : 0);
            N = 1;
        }

        private void RETI()
        {
            RET();
            IME = true;
        }

        private void CALL_Z()
        {
            Call_cc(Z == 1);
        }

        private void Call_cc(bool condition)
        {
            if (condition)
            {
                Call();
                Cycles += 6;
            }
            else
            {
                ProgramCounter += 3;
                Cycles += 3;
            }
        }

        private void JP_C()
        {
            JP_cc(Carry == 1);
        }

        private void JP_NC()
        {
            JP_cc(Carry == 0);
        }

        private void JP_Z()
        {
            JP_cc(Z == 1);
        }

        private void JP_NZ()
        {
            JP_cc(Z == 0);
        }

        public void JP_cc(bool jump)
        {
            var h = _mmu.GetByte((ushort)(ProgramCounter + 2));
            var l = _mmu.GetByte((ushort)(ProgramCounter + 1));
            ProgramCounter = (ushort)(jump ? (ushort)(h << 8 | l) : ProgramCounter + 3);
            Cycles += jump ? 4 : 3;
        }

        private void JR_C()
        {
            JR_cc(Carry == 1);
        }

        private void JR_NC()
        {
            JR_cc(Carry == 0);
        }

        private void JR_cc(bool jump)
        {
            var e = (sbyte)(_mmu.GetByte((ushort)(ProgramCounter + 1)) + 2);
            ProgramCounter += (ushort)(jump ? e : 2);
            Cycles += jump ? 3 : 2;
        }

        private void RST(byte p)
        {
            var programCounterToPush = ProgramCounter + 1;
            _mmu.SetByte((ushort)(SP - 1), (byte)(programCounterToPush >> 8));
            _mmu.SetByte((ushort)(SP - 2), (byte)programCounterToPush);
            SP -= 2;
            ProgramCounter = (ushort)(0x0000 | p);
        }

        private void LD_SP_HL()
        {
            SP = HL;
        }

        private void LD_NN_SP()
        {
            var target = _mmu.GetByte((ushort)(ProgramCounter + 2)) << 8 | _mmu.GetByte((ushort)(ProgramCounter + 1));
            _mmu.SetByte((ushort)target, (byte)(SP & 0xFF));
            _mmu.SetByte((ushort)(target + 1), (byte)((SP >> 8) & 0xFF));
        }

        private void XOR(Register register)
        {
            XOR(_registers[register]);
        }

        private void JP_HL()
        {
            ProgramCounter = HL;
        }

        private void OR_HL()
        {
            var value = _mmu.GetByte(HL);
            OR_A(value);
        }

        private void RET_cc(bool condition)
        {
            if (condition)
            {
                var l = _mmu.GetByte(SP);
                var h = _mmu.GetByte((ushort)(SP + 1));
                ProgramCounter = (ushort)(h << 8 | l);
                Cycles += 5;
                SP += 2;
            }
            else
            {
                ProgramCounter += 1;
                Cycles += 2;
            }
        }

        private void ADC()
        {
            var arg = _mmu.GetByte((ushort)(ProgramCounter + 1));
            var result = A + arg + Carry;
            HC = (byte)((((A & 0x0F) + (arg & 0x0F) + Carry) & 0x10) == 0x10 ? 1 : 0);
            A = (byte)result;
            Carry = (byte)(result > 0xFF ? 1 : 0);
            Z = (byte)(A == 0 ? 1 : 0);
            N = 0;
        }

        private void CB()
        {
            var opCode = _mmu.GetByte((ushort)(ProgramCounter + 1));
            switch (opCode)
            {
                case 0x00:
                    RLC_r(Register.B);
                    break;
                case 0x01:
                    RLC_r(Register.C);
                    break;
                case 0x02:
                    RLC_r(Register.D);
                    break;
                case 0x03:
                    RLC_r(Register.E);
                    break;
                case 0x04:
                    RLC_r(Register.H);
                    break;
                case 0x05:
                    RLC_r(Register.L);
                    break;
                case 0x07:
                    RLC_r(Register.A);
                    break;
                case 0x08:
                    RRC_r(Register.B);
                    break;
                case 0x09:
                    RRC_r(Register.C);
                    break;
                case 0x0A:
                    RRC_r(Register.D);
                    break;
                case 0x0B:
                    RRC_r(Register.E);
                    break;
                case 0x0C:
                    RRC_r(Register.H);
                    break;
                case 0x0D:
                    RRC_r(Register.L);
                    break;
                case 0x0F:
                    RRC_r(Register.A);
                    break;
                case 0x10:
                    RL_r(Register.B);
                    break;
                case 0x11:
                    RL_r(Register.C);
                    break;
                case 0x12:
                    RL_r(Register.D);
                    break;
                case 0x13:
                    RL_r(Register.E);
                    break;
                case 0x14:
                    RL_r(Register.H);
                    break;
                case 0x15:
                    RL_r(Register.L);
                    break;
                case 0x17:
                    RL_r(Register.A);
                    break;
                case 0x18:
                    RR_r(Register.B);
                    break;
                case 0x19:
                    RR_r(Register.C);
                    break;
                case 0x1A:
                    RR_r(Register.D);
                    break;
                case 0x1B:
                    RR_r(Register.E);
                    break;
                case 0x1C:
                    RR_r(Register.H);
                    break;
                case 0x1D:
                    RR_r(Register.L);
                    break;
                case 0x1F:
                    RR_r(Register.A);
                    break;
                case 0x20:
                    SLA_r(Register.B);
                    break;
                case 0x21:
                    SLA_r(Register.C);
                    break;
                case 0x22:
                    SLA_r(Register.D);
                    break;
                case 0x23:
                    SLA_r(Register.E);
                    break;
                case 0x24:
                    SLA_r(Register.H);
                    break;
                case 0x25:
                    SLA_r(Register.L);
                    break;
                case 0x27:
                    SLA_r(Register.A);
                    break;
                case 0x28:
                    SRA_r(Register.B);
                    break;
                case 0x29:
                    SRA_r(Register.C);
                    break;
                case 0x2A:
                    SRA_r(Register.D);
                    break;
                case 0x2B:
                    SRA_r(Register.E);
                    break;
                case 0x2C:
                    SRA_r(Register.H);
                    break;
                case 0x2D:
                    SRA_r(Register.L);
                    break;
                case 0x2F:
                    SRA_r(Register.A);
                    break;
                case 0x30:
                    SWAP_r(Register.B);
                    break;
                case 0x31:
                    SWAP_r(Register.C);
                    break;
                case 0x32:
                    SWAP_r(Register.D);
                    break;
                case 0x33:
                    SWAP_r(Register.E);
                    break;
                case 0x34:
                    SWAP_r(Register.H);
                    break;
                case 0x35:
                    SWAP_r(Register.L);
                    break;
                case 0x37:
                    SWAP_r(Register.A);
                    break;
                case 0x38:
                    SRL_r(Register.B);
                    break;
                case 0x39:
                    SRL_r(Register.C);
                    break;
                case 0x3A:
                    SRL_r(Register.D);
                    break;
                case 0x3B:
                    SRL_r(Register.E);
                    break;
                case 0x3C:
                    SRL_r(Register.H);
                    break;
                case 0x3D:
                    SRL_r(Register.L);
                    break;
                case 0x3F:
                    SRL_r(Register.A);
                    break;
                case 0x40:
                    BIT(0, Register.B);
                    break;
                case 0x41:
                    BIT(0, Register.C);
                    break;
                case 0x42:
                    BIT(0, Register.D);
                    break;
                case 0x43:
                    BIT(0, Register.E);
                    break;
                case 0x44:
                    BIT(0, Register.H);
                    break;
                case 0x45:
                    BIT(0, Register.L);
                    break;
                case 0x47:
                    BIT(0, Register.A);
                    break;
                case 0x48:
                    BIT(1, Register.B);
                    break;
                case 0x49:
                    BIT(1, Register.C);
                    break;
                case 0x4A:
                    BIT(1, Register.D);
                    break;
                case 0x4B:
                    BIT(1, Register.E);
                    break;
                case 0x4C:
                    BIT(1, Register.H);
                    break;
                case 0x4D:
                    BIT(1, Register.L);
                    break;
                case 0x50:
                    BIT(2, Register.B);
                    break;
                case 0x51:
                    BIT(2, Register.C);
                    break;
                case 0x53:
                    BIT(2, Register.E);
                    break;
                case 0x54:
                    BIT(2, Register.H);
                    break;
                case 0x55:
                    BIT(2, Register.L);
                    break;
                case 0x58:
                    BIT(3, Register.B);
                    break;
                case 0x4F:
                    BIT(1, Register.A);
                    break;
                case 0x52:
                    BIT(2, Register.D);
                    break;
                case 0x57:
                    BIT(2, Register.A);
                    break;
                case 0x59:
                    BIT(3, Register.C);
                    break;
                case 0x5A:
                    BIT(3, Register.D);
                    break;
                case 0x5B:
                    BIT(3, Register.E);
                    break;
                case 0x5C:
                    BIT(3, Register.H);
                    break;
                case 0x5D:
                    BIT(3, Register.L);
                    break;
                case 0x5F:
                    BIT(3, Register.A);
                    break;
                case 0x60:
                    BIT(4, Register.B);
                    break;
                case 0x61:
                    BIT(4, Register.C);
                    break;
                case 0x62:
                    BIT(4, Register.D);
                    break;
                case 0x63:
                    BIT(4, Register.E);
                    break;
                case 0x64:
                    BIT(4, Register.H);
                    break;
                case 0x65:
                    BIT(4, Register.L);
                    break;
                case 0x67:
                    BIT(4, Register.A);
                    break;
                case 0x68:
                    BIT(5, Register.B);
                    break;
                case 0x69:
                    BIT(5, Register.C);
                    break;
                case 0x6A:
                    BIT(5, Register.D);
                    break;
                case 0x6B:
                    BIT(5, Register.E);
                    break;
                case 0x6C:
                    BIT(5, Register.H);
                    break;
                case 0x6D:
                    BIT(5, Register.L);
                    break;
                case 0x6F:
                    BIT(5, Register.A);
                    break;
                case 0x70:
                    BIT(6, Register.B);
                    break;
                case 0x71:
                    BIT(6, Register.C);
                    break;
                case 0x72:
                    BIT(6, Register.D);
                    break;
                case 0x73:
                    BIT(6, Register.E);
                    break;
                case 0x74:
                    BIT(6, Register.H);
                    break;
                case 0x75:
                    BIT(6, Register.L);
                    break;
                case 0x77:
                    BIT(6, Register.A);
                    break;
                case 0x78:
                    BIT(7, Register.B);
                    break;
                case 0x79:
                    BIT(7, Register.C);
                    break;
                case 0x7A:
                    BIT(7, Register.D);
                    break;
                case 0x7B:
                    BIT(7, Register.E);
                    break;
                case 0x7C:
                    BIT(7, Register.H);
                    break;
                case 0x7D:
                    BIT(7, Register.L);
                    break;
                case 0x7E:
                    BIT_HLm(7);
                    break;
                case 0x7f:
                    BIT(7, Register.A);
                    break;
                case 0x80:
                    RES(0, Register.B);
                    break;
                case 0x81:
                    RES(0, Register.C);
                    break;
                case 0x82:
                    RES(0, Register.D);
                    break;
                case 0x83:
                    RES(0, Register.E);
                    break;
                case 0x84:
                    RES(0, Register.H);
                    break;
                case 0x85:
                    RES(0, Register.L);
                    break;
                case 0x86:
                    RES_HLm(0);
                    break;
                case 0x87:
                    RES(0, Register.A);
                    break;
                default:
                    throw new IllegalOpcodeException(opCode, ProgramCounter);
            }

            var instructionMetaData = _cbInstructions[opCode];
            ProgramCounter += instructionMetaData.Size;
            Cycles += instructionMetaData.Cycles;
        }

        private void BIT_HLm(int bit)
        {
            var value = _mmu.GetByte(HL);
            Z = (byte) ((value & 0x80) == 0x80 ? 0 : 1);
            HC = 1;
            N = 0;
        }

        private void RES_HLm(int bit)
        {
            var value = _mmu.GetByte(HL);
            _mmu.SetByte(HL, (byte) (value & 0xFE));
        }

        private void RES(byte bit, Register register)
        {
            _registers[register] = (byte)(_registers[register] & ~(1 << bit));
        }

        private readonly Dictionary<byte, InstructionMetaData> _cbInstructions = new Dictionary<byte, InstructionMetaData>
        {
            {0x00, new InstructionMetaData(2, 2, "RLC B")},
            {0x01, new InstructionMetaData(2, 2, "RLC C")},
            {0x02, new InstructionMetaData(2, 2, "RLC D")},
            {0x03, new InstructionMetaData(2, 2, "RLC E")},
            {0x04, new InstructionMetaData(2, 2, "RLC H")},
            {0x05, new InstructionMetaData(2, 2, "RLC L")},
            {0x07, new InstructionMetaData(2, 2, "RLC A")},
            {0x08, new InstructionMetaData(2, 2, "RRC B")},
            {0x09, new InstructionMetaData(2, 2, "RRC C")},
            {0x0A, new InstructionMetaData(2, 2, "RRC D")},
            {0x0B, new InstructionMetaData(2, 2, "RRC E")},
            {0x0C, new InstructionMetaData(2, 2, "RRC H")},
            {0x0D, new InstructionMetaData(2, 2, "RRC L")},
            {0x0F, new InstructionMetaData(2, 2, "RRC A")},
            {0x10, new InstructionMetaData(2, 2, "RL B")},
            {0x11, new InstructionMetaData(2, 2, "RL C")},
            {0x12, new InstructionMetaData(2, 2, "RL D")},
            {0x13, new InstructionMetaData(2, 2, "RL E")},
            {0x14, new InstructionMetaData(2, 2, "RL H")},
            {0x15, new InstructionMetaData(2, 2, "RL L")},
            {0x17, new InstructionMetaData(2, 2, "RL A")},
            {0x18, new InstructionMetaData(2, 2, "RR B")},
            {0x19, new InstructionMetaData(2, 2, "RR C")},
            {0x1A, new InstructionMetaData(2, 2, "RR D")},
            {0x1B, new InstructionMetaData(2, 2, "RR E")},
            {0x1C, new InstructionMetaData(2, 2, "RR H")},
            {0x1D, new InstructionMetaData(2, 2, "RR L")},
            {0x1F, new InstructionMetaData(2, 2, "RR A")},
            {0x20, new InstructionMetaData(2, 2, "SLA B")},
            {0x21, new InstructionMetaData(2, 2, "SLA C")},
            {0x22, new InstructionMetaData(2, 2, "SLA D")},
            {0x23, new InstructionMetaData(2, 2, "SLA E")},
            {0x24, new InstructionMetaData(2, 2, "SLA H")},
            {0x25, new InstructionMetaData(2, 2, "SLA L")},
            {0x27, new InstructionMetaData(2, 2, "SLA A")},
            {0x28, new InstructionMetaData(2, 2, "SRA B")},
            {0x29, new InstructionMetaData(2, 2, "SRA C")},
            {0x2A, new InstructionMetaData(2, 2, "SRA D")},
            {0x2B, new InstructionMetaData(2, 2, "SRA E")},
            {0x2C, new InstructionMetaData(2, 2, "SRA H")},
            {0x2D, new InstructionMetaData(2, 2, "SRA L")},
            {0x2F, new InstructionMetaData(2, 2, "SRA A")},
            {0x30, new InstructionMetaData(2, 2, "SWAP B")},
            {0x31, new InstructionMetaData(2, 2, "SWAP C")},
            {0x32, new InstructionMetaData(2, 2, "SWAP D")},
            {0x33, new InstructionMetaData(2, 2, "SWAP E")},
            {0x34, new InstructionMetaData(2, 2, "SWAP H")},
            {0x35, new InstructionMetaData(2, 2, "SWAP L")},
            {0x37, new InstructionMetaData(2, 2, "SWAP A")},
            {0x38, new InstructionMetaData(2, 2, "SRL B")},
            {0x39, new InstructionMetaData(2, 2, "SRL C")},
            {0x3A, new InstructionMetaData(2, 2, "SRL D")},
            {0x3B, new InstructionMetaData(2, 2, "SRL E")},
            {0x3C, new InstructionMetaData(2, 2, "SRL H")},
            {0x3D, new InstructionMetaData(2, 2, "SRL L")},
            {0x3F, new InstructionMetaData(2, 2, "SRL A")},
            {0x40, new InstructionMetaData(2, 2, "BIT 0, B")},
            {0x41, new InstructionMetaData(2, 2, "BIT 0, C")},
            {0x42, new InstructionMetaData(2, 2, "BIT 0, D")},
            {0x43, new InstructionMetaData(2, 2, "BIT 0, E")},
            {0x44, new InstructionMetaData(2, 2, "BIT 0, H")},
            {0x45, new InstructionMetaData(2, 2, "BIT 0, L")},
            {0x47, new InstructionMetaData(2, 2, "BIT 0, A")},
            {0x48, new InstructionMetaData(2, 2, "BIT 1, B")},
            {0x49, new InstructionMetaData(2, 2, "BIT 1, C")},
            {0x4A, new InstructionMetaData(2, 2, "BIT 1, D")},
            {0x4B, new InstructionMetaData(2, 2, "BIT 1, E")},
            {0x4C, new InstructionMetaData(2, 2, "BIT 1, H")},
            {0x4D, new InstructionMetaData(2, 2, "BIT 1, L")},
            {0x50, new InstructionMetaData(2, 2, "BIT 2, B")},
            {0x4F, new InstructionMetaData(2, 2, "BIT 1, A")},
            {0x51, new InstructionMetaData(2, 2, "BIT 2, C")},
            {0x52, new InstructionMetaData(2, 2, "BIT 2, D")},
            {0x53, new InstructionMetaData(2, 2, "BIT 2, E")},
            {0x54, new InstructionMetaData(2, 2, "BIT 2, H")},
            {0x55, new InstructionMetaData(2, 2, "BIT 2, L")},
            {0x57, new InstructionMetaData(2, 2, "BIT 2, A")},
            {0x58, new InstructionMetaData(2, 2, "BIT 3, B")},
            {0x59, new InstructionMetaData(2, 2, "BIT 3, C")},
            {0x5A, new InstructionMetaData(2, 2, "BIT 3, D")},
            {0x5B, new InstructionMetaData(2, 2, "BIT 3, E")},
            {0x5C, new InstructionMetaData(2, 2, "BIT 3, H")},
            {0x5D, new InstructionMetaData(2, 2, "BIT 3, L")},
            {0x5F, new InstructionMetaData(2, 2, "BIT 3, A")},
            {0x60, new InstructionMetaData(2, 2, "BIT 4, B")},
            {0x61, new InstructionMetaData(2, 2, "BIT 4, C")},
            {0x62, new InstructionMetaData(2, 2, "BIT 4, D")},
            {0x63, new InstructionMetaData(2, 2, "BIT 4, E")},
            {0x64, new InstructionMetaData(2, 2, "BIT 4, H")},
            {0x65, new InstructionMetaData(2, 2, "BIT 4, L")},
            {0x67, new InstructionMetaData(2, 2, "BIT 4, A")},
            {0x68, new InstructionMetaData(2, 2, "BIT 5, B")},
            {0x69, new InstructionMetaData(2, 2, "BIT 5, C")},
            {0x6A, new InstructionMetaData(2, 2, "BIT 5, D")},
            {0x6B, new InstructionMetaData(2, 2, "BIT 5, E")},
            {0x6C, new InstructionMetaData(2, 2, "BIT 5, H")},
            {0x6D, new InstructionMetaData(2, 2, "BIT 6, L")},
            {0x6F, new InstructionMetaData(2, 2, "BIT 5, A")},
            {0x70, new InstructionMetaData(2, 2, "BIT 6, B")},
            {0x71, new InstructionMetaData(2, 2, "BIT 6, C")},
            {0x72, new InstructionMetaData(2, 2, "BIT 6, D")},
            {0x73, new InstructionMetaData(2, 2, "BIT 6, E")},
            {0x74, new InstructionMetaData(2, 2, "BIT 6, H")},
            {0x75, new InstructionMetaData(2, 2, "BIT 6, L")},
            {0x77, new InstructionMetaData(2, 2, "BIT 6, A")},
            {0x78, new InstructionMetaData(2, 2, "BIT 7, B")},
            {0x79, new InstructionMetaData(2, 2, "BIT 7, C")},
            {0x7A, new InstructionMetaData(2, 2, "BIT 7, D")},
            {0x7B, new InstructionMetaData(2, 2, "BIT 7, E")},
            {0x7C, new InstructionMetaData(2, 2, "BIT 7, H")},
            {0x7D, new InstructionMetaData(2, 2, "BIT 7, L")},
            {0x7E, new InstructionMetaData(2, 4, "BIT 7, (HL)")},
            {0x7F, new InstructionMetaData(2, 2, "BIT 7, A")},
            {0x80, new InstructionMetaData(2, 2, "RES 0, B")},
            {0x81, new InstructionMetaData(2, 2, "RES 0, C")},
            {0x82, new InstructionMetaData(2, 2, "RES 0, D")},
            {0x83, new InstructionMetaData(2, 2, "RES 0, E")},
            {0x84, new InstructionMetaData(2, 2, "RES 0, H")},
            {0x85, new InstructionMetaData(2, 2, "RES 0, L")},
            {0x86, new InstructionMetaData(2, 4, "RES 0, (HL)")},
            {0x87, new InstructionMetaData(2, 2, "RES 0, A")},
        };

        private void SLA_r(Register register)
        {
            HC = 0;
            N = 0;
            Carry = (byte)((_registers[register] & 0x80) >> 7);
            var result = (byte)(_registers[register] << 1);
            _registers[register] = result;
            Z = (byte)(result == 0 ? 1 : 0);
        }

        private void SRA_r(Register register)
        {
            N = 0;
            HC = 0;
            var value = _registers[register];
            var result = (byte)((value >> 1) | (value & 0x80));
            _registers[register] = result;
            Carry = (byte)(value & 0x01);
            Z = (byte)(result == 0 ? 1 : 0);
        }

        private void RRC_r(Register register)
        {
            N = 0;
            HC = 0;
            var bit0 = (_registers[register] & 0x01);

            var result = (byte)((_registers[register] >> 1) | (bit0 << 7));
            _registers[register] = result;
            Carry = (byte)bit0;
            Z = (byte)(result == 0 ? 1 : 0);
        }

        private void RLC_r(Register register)
        {
            var bit7 = (_registers[register] & 0x80) >> 7;
            var result = (byte)((_registers[register] << 1) | bit7);

            _registers[register] = result;
            Carry = (byte)bit7;
            Z = (byte)(result == 0 ? 1 : 0);
            N = 0;
            HC = 0;
        }

        private void SWAP_r(Register register)
        {
            var value = _registers[register];
            _registers[register] = (byte)(((value >> 4) & 0x0F) | ((value << 4) & 0xF0));

            Z = (byte)(value == 0 ? 1 : 0);
            Carry = 0;
            N = 0;
            HC = 0;
        }

        private void RL_r(Register register)
        {
            HC = 0;
            N = 0;
            var result = (byte)((_registers[register] << 1) | Carry);
            Carry = (byte)((_registers[register] & 0x80) == 0 ? 0 : 1);
            _registers[register] = result;
            Z = (byte)(result == 0 ? 1 : 0);
        }

        private void BIT(int bit, Register register)
        {
            Z = (byte)((~(_registers[register])) >> bit & 0x01);
            N = 0;
            HC = 1;
        }

        private void RR_r(Register register)
        {
            var old = _registers[register];
            _registers[register] = (byte)(old >> 1 | (Carry << 7));
            Carry = (byte)((old & 0x01) == 0x01 ? 1 : 0);
            Z = (byte)(_registers[register] == 0 ? 1 : 0);
            HC = 0;
            N = 0;
        }

        private void SRL_r(Register register)
        {
            Carry = (byte)((_registers[register] & 0x01) == 0x01 ? 1 : 0);
            _registers[register] = (byte)(_registers[register] >> 1);
            Z = (byte)(_registers[register] == 0 ? 1 : 0);
            N = 0;
            HC = 0;
        }

        private void XOR(byte value)
        {
            A = (byte)(A ^ value);
            Z = (byte)(A == 0 ? 1 : 0);
            N = 0;
            HC = 0;
            Carry = 0;
        }

        private void OR_r(Register register)
        {
            OR_A(_registers[register]);
        }

        private void OR_A(byte value)
        {
            A = (byte)(A | value);
            HC = 0;
            N = 0;
            Carry = 0;
            Z = (byte)(A == 0 ? 1 : 0);
        }

        private void DEC_r(Register register)
        {
            N = 1;
            HC = (byte)((_registers[register] & 0x0F) == 0 ? 1 : 0);
            _registers[register] = (byte)(_registers[register] - 1);
            Z = (byte)(_registers[register] == 0 ? 1 : 0);
        }

        private void DEC_HL()
        {
            var value = _mmu.GetByte(HL);
            _mmu.SetByte(HL, (byte)(value - 1));
            Z = (byte)((value - 1) == 0 ? 1 : 0);
            N = 1;
            HC = (byte)((value & 0x0F) == 0 ? 1 : 0);
        }

        private void RET()
        {
            var low = _mmu.GetByte(SP);
            var high = _mmu.GetByte((ushort)(SP + 1));

            ProgramCounter = (ushort)(high << 8 | low);
            SP = (ushort)(SP + 2);
        }

        private void JR_e()
        {
            var eMinusTwo = (sbyte)_mmu.GetByte((ushort)(ProgramCounter + 1));
            var e = eMinusTwo + 2;

            ProgramCounter = (ushort)(ProgramCounter + e);
        }

        private void Call()
        {
            var high = _mmu.GetByte((ushort)(ProgramCounter + 2)) << 8;
            var low = _mmu.GetByte((ushort)(ProgramCounter + 1));
            var subroutineAddress = (ushort)(high | low);

            var returnAddress = ProgramCounter + 3;
            _mmu.SetByte((ushort)(SP - 1), (byte)(returnAddress >> 8));
            _mmu.SetByte((ushort)(SP - 2), (byte)returnAddress);

            SP = (ushort)(SP - 2);
            ProgramCounter = subroutineAddress;
        }

        private void LD_HL_r(Register register)
        {
            var target = HL;
            var value = _registers[register];
            _mmu.SetByte(target, value);
        }

        private void LD_r_HL(Register register)
        {
            var n = _mmu.GetByte(HL);
            _registers[register] = n;
        }

        private ushort HL
        {
            get { return (ushort)(H << 8 | L); }
        }

        public byte A
        {
            get { return _registers[Register.A]; }
            set { _registers[Register.A] = value; }
        }

        public byte B
        {
            get { return _registers[Register.B]; }
            set { _registers[Register.B] = value; }
        }

        public byte C
        {
            get { return _registers[Register.C]; }
            set { _registers[Register.C] = value; }
        }

        public byte D
        {
            get { return _registers[Register.D]; }
            set { _registers[Register.D] = value; }
        }

        public byte E
        {
            get { return _registers[Register.E]; }
            set { _registers[Register.E] = value; }
        }

        public byte H
        {
            get { return _registers[Register.H]; }
            set { _registers[Register.H] = value; }
        }

        public byte L
        {
            get { return _registers[Register.L]; }
            set { _registers[Register.L] = value; }
        }

        private byte _f;
        public byte F
        {
            get
            {
                return _f;
            }
            set { _f = value; }
        }

        public bool IME;
        public byte IE;
        public byte IF;
        public bool Halted;
        public ushort SP { get; set; }

        public ushort ProgramCounter { get; set; }
        public long Cycles { get; set; }

        public byte Z
        {
            get { return (byte)((0x80 & _f) == 0x80 ? 1 : 0); }
            set { _f = (byte)((_f & 0x7F) | (value << 7)); }
        }

        public byte N
        {
            get { return (byte)((0x40 & _f) == 0x40 ? 1 : 0); }
            set
            {
                _f = (byte)((_f & 0xBF) | ((value & 0x01) << 6));
            }
        }

        public byte HC
        {
            get { return (byte)((0x20 & _f) == 0x20 ? 1 : 0); }
            set { _f = (byte)((_f & 0xDF) | ((value & 0x01) << 5)); }
        }

        public byte Carry
        {
            get { return (byte)((0x10 & _f) == 0x10 ? 1 : 0); }
            set { _f = (byte)((_f & 0xEF) | ((value & 0x01) << 4)); }
        }
    }

    public class IllegalOpcodeException : Exception
    {
        private readonly byte _opcode;
        private readonly ushort _programCounter;

        public IllegalOpcodeException(byte opcode, ushort programCounter)
        {
            _programCounter = programCounter;
            _opcode = opcode;
        }

        public override string Message
        {
            get { return string.Format("Illegal opcode 0x{0:x2} at {1:x}", _opcode, _programCounter); }
        }
    }
}
