using System.Collections.Generic;

namespace Core
{
    public partial class Cpu
    {
        private readonly Dictionary<byte, InstructionMetaData> _cbInstructions = new Dictionary<byte, InstructionMetaData>
        {
            {0x00, new InstructionMetaData(2, 2, "RLC B")},
            {0x01, new InstructionMetaData(2, 2, "RLC C")},
            {0x02, new InstructionMetaData(2, 2, "RLC D")},
            {0x03, new InstructionMetaData(2, 2, "RLC E")},
            {0x04, new InstructionMetaData(2, 2, "RLC H")},
            {0x05, new InstructionMetaData(2, 2, "RLC L")},
            {0x06, new InstructionMetaData(2, 4, "RLC (HL)")},
            {0x07, new InstructionMetaData(2, 2, "RLC A")},
            {0x08, new InstructionMetaData(2, 2, "RRC B")},
            {0x09, new InstructionMetaData(2, 2, "RRC C")},
            {0x0A, new InstructionMetaData(2, 2, "RRC D")},
            {0x0B, new InstructionMetaData(2, 2, "RRC E")},
            {0x0C, new InstructionMetaData(2, 2, "RRC H")},
            {0x0D, new InstructionMetaData(2, 2, "RRC L")},
            {0x0E, new InstructionMetaData(2, 4, "RRC (HL)")},
            {0x0F, new InstructionMetaData(2, 2, "RRC A")},
            {0x10, new InstructionMetaData(2, 2, "RL B")},
            {0x11, new InstructionMetaData(2, 2, "RL C")},
            {0x12, new InstructionMetaData(2, 2, "RL D")},
            {0x13, new InstructionMetaData(2, 2, "RL E")},
            {0x14, new InstructionMetaData(2, 2, "RL H")},
            {0x15, new InstructionMetaData(2, 2, "RL L")},
            {0x16, new InstructionMetaData(2, 4, "RL (HL)")},
            {0x17, new InstructionMetaData(2, 2, "RL A")},
            {0x18, new InstructionMetaData(2, 2, "RR B")},
            {0x19, new InstructionMetaData(2, 2, "RR C")},
            {0x1A, new InstructionMetaData(2, 2, "RR D")},
            {0x1B, new InstructionMetaData(2, 2, "RR E")},
            {0x1C, new InstructionMetaData(2, 2, "RR H")},
            {0x1D, new InstructionMetaData(2, 2, "RR L")},
            {0x1E, new InstructionMetaData(2, 4, "RR (HL)")},
            {0x1F, new InstructionMetaData(2, 2, "RR A")},
            {0x20, new InstructionMetaData(2, 2, "SLA B")},
            {0x21, new InstructionMetaData(2, 2, "SLA C")},
            {0x22, new InstructionMetaData(2, 2, "SLA D")},
            {0x23, new InstructionMetaData(2, 2, "SLA E")},
            {0x24, new InstructionMetaData(2, 2, "SLA H")},
            {0x25, new InstructionMetaData(2, 2, "SLA L")},
            {0x26, new InstructionMetaData(2, 4, "SLA (HL)")},
            {0x27, new InstructionMetaData(2, 2, "SLA A")},
            {0x28, new InstructionMetaData(2, 2, "SRA B")},
            {0x29, new InstructionMetaData(2, 2, "SRA C")},
            {0x2A, new InstructionMetaData(2, 2, "SRA D")},
            {0x2B, new InstructionMetaData(2, 2, "SRA E")},
            {0x2C, new InstructionMetaData(2, 2, "SRA H")},
            {0x2D, new InstructionMetaData(2, 2, "SRA L")},
            {0x2E, new InstructionMetaData(2, 4, "SRA (HL)")},
            {0x2F, new InstructionMetaData(2, 2, "SRA A")},
            {0x30, new InstructionMetaData(2, 2, "SWAP B")},
            {0x31, new InstructionMetaData(2, 2, "SWAP C")},
            {0x32, new InstructionMetaData(2, 2, "SWAP D")},
            {0x33, new InstructionMetaData(2, 2, "SWAP E")},
            {0x34, new InstructionMetaData(2, 2, "SWAP H")},
            {0x35, new InstructionMetaData(2, 2, "SWAP L")},
            {0x36, new InstructionMetaData(2, 4, "SWAP (HL)")},
            {0x37, new InstructionMetaData(2, 2, "SWAP A")},
            {0x38, new InstructionMetaData(2, 2, "SRL B")},
            {0x39, new InstructionMetaData(2, 2, "SRL C")},
            {0x3A, new InstructionMetaData(2, 2, "SRL D")},
            {0x3B, new InstructionMetaData(2, 2, "SRL E")},
            {0x3C, new InstructionMetaData(2, 2, "SRL H")},
            {0x3D, new InstructionMetaData(2, 2, "SRL L")},
            {0x3E, new InstructionMetaData(2, 4, "SRL (HL)")},
            {0x3F, new InstructionMetaData(2, 2, "SRL A")},
            {0x40, new InstructionMetaData(2, 2, "BIT 0, B")},
            {0x41, new InstructionMetaData(2, 2, "BIT 0, C")},
            {0x42, new InstructionMetaData(2, 2, "BIT 0, D")},
            {0x43, new InstructionMetaData(2, 2, "BIT 0, E")},
            {0x44, new InstructionMetaData(2, 2, "BIT 0, H")},
            {0x45, new InstructionMetaData(2, 2, "BIT 0, L")},
            {0x46, new InstructionMetaData(2, 4, "BIT 0, (HL)")},
            {0x47, new InstructionMetaData(2, 2, "BIT 0, A")},
            {0x48, new InstructionMetaData(2, 2, "BIT 1, B")},
            {0x49, new InstructionMetaData(2, 2, "BIT 1, C")},
            {0x4A, new InstructionMetaData(2, 2, "BIT 1, D")},
            {0x4B, new InstructionMetaData(2, 2, "BIT 1, E")},
            {0x4C, new InstructionMetaData(2, 2, "BIT 1, H")},
            {0x4D, new InstructionMetaData(2, 2, "BIT 1, L")},
            {0x4E, new InstructionMetaData(2, 4, "BIT 1, (HL)")},
            {0x50, new InstructionMetaData(2, 2, "BIT 2, B")},
            {0x4F, new InstructionMetaData(2, 2, "BIT 1, A")},
            {0x51, new InstructionMetaData(2, 2, "BIT 2, C")},
            {0x52, new InstructionMetaData(2, 2, "BIT 2, D")},
            {0x53, new InstructionMetaData(2, 2, "BIT 2, E")},
            {0x54, new InstructionMetaData(2, 2, "BIT 2, H")},
            {0x55, new InstructionMetaData(2, 2, "BIT 2, L")},
            {0x56, new InstructionMetaData(2, 4, "BIT 2, (HL)")},
            {0x57, new InstructionMetaData(2, 2, "BIT 2, A")},
            {0x58, new InstructionMetaData(2, 2, "BIT 3, B")},
            {0x59, new InstructionMetaData(2, 2, "BIT 3, C")},
            {0x5A, new InstructionMetaData(2, 2, "BIT 3, D")},
            {0x5B, new InstructionMetaData(2, 2, "BIT 3, E")},
            {0x5C, new InstructionMetaData(2, 2, "BIT 3, H")},
            {0x5D, new InstructionMetaData(2, 2, "BIT 3, L")},
            {0x5E, new InstructionMetaData(2, 4, "BIT 3, (HL)")},
            {0x5F, new InstructionMetaData(2, 2, "BIT 3, A")},
            {0x60, new InstructionMetaData(2, 2, "BIT 4, B")},
            {0x61, new InstructionMetaData(2, 2, "BIT 4, C")},
            {0x62, new InstructionMetaData(2, 2, "BIT 4, D")},
            {0x63, new InstructionMetaData(2, 2, "BIT 4, E")},
            {0x64, new InstructionMetaData(2, 2, "BIT 4, H")},
            {0x65, new InstructionMetaData(2, 2, "BIT 4, L")},
            {0x66, new InstructionMetaData(2, 4, "BIT 4, (HL)")},
            {0x67, new InstructionMetaData(2, 2, "BIT 4, A")},
            {0x68, new InstructionMetaData(2, 2, "BIT 5, B")},
            {0x69, new InstructionMetaData(2, 2, "BIT 5, C")},
            {0x6A, new InstructionMetaData(2, 2, "BIT 5, D")},
            {0x6B, new InstructionMetaData(2, 2, "BIT 5, E")},
            {0x6C, new InstructionMetaData(2, 2, "BIT 5, H")},
            {0x6D, new InstructionMetaData(2, 2, "BIT 5, L")},
            {0x6E, new InstructionMetaData(2, 4, "BIT 5, (HL)")},
            {0x6F, new InstructionMetaData(2, 2, "BIT 5, A")},
            {0x70, new InstructionMetaData(2, 2, "BIT 6, B")},
            {0x71, new InstructionMetaData(2, 2, "BIT 6, C")},
            {0x72, new InstructionMetaData(2, 2, "BIT 6, D")},
            {0x73, new InstructionMetaData(2, 2, "BIT 6, E")},
            {0x74, new InstructionMetaData(2, 2, "BIT 6, H")},
            {0x75, new InstructionMetaData(2, 2, "BIT 6, L")},
            {0x76, new InstructionMetaData(2, 4, "BIT 6, (HL)")},
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
            {0x88, new InstructionMetaData(2, 2, "RES 1, B")},
            {0x89, new InstructionMetaData(2, 2, "RES 1, C")},
            {0x8A, new InstructionMetaData(2, 2, "RES 1, D")},
            {0x8E, new InstructionMetaData(2, 4, "RES 1, (HL)")},
            {0x8F, new InstructionMetaData(2, 2, "RES 1, A")},
            {0x90, new InstructionMetaData(2, 2, "RES 2, B")},
            {0x91, new InstructionMetaData(2, 2, "RES 2, C")},
            {0x92, new InstructionMetaData(2, 2, "RES 2, D")},
            {0x96, new InstructionMetaData(2, 4, "RES 2, (HL)")},
            {0x97, new InstructionMetaData(2, 2, "RES 2, A")},
            {0x98, new InstructionMetaData(2, 2, "RES 3, B")},
            {0x99, new InstructionMetaData(2, 2, "RES 3, C")},
            {0x9A, new InstructionMetaData(2, 2, "RES 3, D")},
            {0x9E, new InstructionMetaData(2, 4, "RES 3, (HL)")},
            {0x9F, new InstructionMetaData(2, 2, "RES 3, A")},
            {0xA0, new InstructionMetaData(2, 2, "RES 4, B")},
            {0xA1, new InstructionMetaData(2, 2, "RES 4, C")},
            {0xA2, new InstructionMetaData(2, 2, "RES 4, D")},
            {0xA6, new InstructionMetaData(2, 4, "RES 4, (HL)")},
            {0xA7, new InstructionMetaData(2, 2, "RES 4, A")},
            {0xA8, new InstructionMetaData(2, 2, "RES 5, B")},
            {0xA9, new InstructionMetaData(2, 2, "RES 5, C")},
            {0xAA, new InstructionMetaData(2, 2, "RES 5, D")},
            {0xAE, new InstructionMetaData(2, 4, "RES 5, (HL)")},
            {0xAF, new InstructionMetaData(2, 2, "RES 5, A")},
            {0xB0, new InstructionMetaData(2, 2, "RES 6, B")},
            {0xB1, new InstructionMetaData(2, 2, "RES 6, C")},
            {0xB2, new InstructionMetaData(2, 2, "RES 6, D")},
            {0xB6, new InstructionMetaData(2, 4, "RES 6, (HL)")},
            {0xB7, new InstructionMetaData(2, 2, "RES 6, A")},
            {0xB8, new InstructionMetaData(2, 2, "RES 7, B")},
            {0xB9, new InstructionMetaData(2, 2, "RES 7, C")},
            {0xBA, new InstructionMetaData(2, 2, "RES 7, D")},
            {0xBE, new InstructionMetaData(2, 4, "RES 7, (HL)")},
            {0xBF, new InstructionMetaData(2, 2, "RES 7, A")},
            {0xC6, new InstructionMetaData(2, 4, "SET 0, (HL)")},
            {0xCE, new InstructionMetaData(2, 4, "SET 1, (HL)")},
            {0xD6, new InstructionMetaData(2, 4, "SET 2, (HL)")},
            {0xDE, new InstructionMetaData(2, 4, "SET 3, (HL)")},
            {0xE6, new InstructionMetaData(2, 4, "SET 4, (HL)")},
            {0xEE, new InstructionMetaData(2, 4, "SET 5, (HL)")},
            {0xF6, new InstructionMetaData(2, 4, "SET 6, (HL)")},
            {0xFE, new InstructionMetaData(2, 4, "SET 7, (HL)")},
        };

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
                case 0x06:
                    RLC_HLm();
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
                case 0x0E:
                    RRC_HLm();
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
                case 0x16:
                    RL_HLm();
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
                case 0x1E:
                    RR_HLm();
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
                case 0x26:
                    SLA_HLm();
                    break;
                case 0x2E:
                    SRA_HLm();
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
                case 0x36:
                    SWAP_HLm();
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
                case 0x3E:
                    SRL_HLm();
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
                case 0x46:
                    BIT_HLm(0);
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
                case 0x4E:
                    BIT_HLm(1);
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
                case 0x56:
                    BIT_HLm(2);
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
                case 0x5E:
                    BIT_HLm(3);
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
                case 0x66:
                    BIT_HLm(4);
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
                case 0x6E:
                    BIT_HLm(5);
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
                case 0x76:
                    BIT_HLm(6);
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
                case 0x7F:
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
                case 0x88:
                    RES(1, Register.B);
                    break;
                case 0x89:
                    RES(1, Register.C);
                    break;
                case 0x8A:
                    RES(1, Register.D);
                    break;
                case 0x8E:
                    RES_HLm(1);
                    break;
                case 0x8F:
                    RES(1, Register.A);
                    break;
                case 0x90:
                    RES(2, Register.B);
                    break;
                case 0x91:
                    RES(2, Register.C);
                    break;
                case 0x92:
                    RES(2, Register.D);
                    break;
                case 0x96:
                    RES_HLm(2);
                    break;
                case 0x97:
                    RES(2, Register.A);
                    break;
                case 0x98:
                    RES(3, Register.B);
                    break;
                case 0x99:
                    RES(3, Register.C);
                    break;
                case 0x9A:
                    RES(3, Register.D);
                    break;
                case 0x9E:
                    RES_HLm(3);
                    break;
                case 0x9F:
                    RES(3, Register.A);
                    break;
                case 0xA0:
                    RES(4, Register.B);
                    break;
                case 0xA1:
                    RES(4, Register.C);
                    break;
                case 0xA2:
                    RES(4, Register.D);
                    break;
                case 0xA6:
                    RES_HLm(4);
                    break;
                case 0xA7:
                    RES(4, Register.A);
                    break;
                case 0xA8:
                    RES(5, Register.B);
                    break;
                case 0xA9:
                    RES(5, Register.C);
                    break;
                case 0xAA:
                    RES(5, Register.D);
                    break;
                case 0xAE:
                    RES_HLm(5);
                    break;
                case 0xAF:
                    RES(5, Register.A);
                    break;
                case 0xB0:
                    RES(6, Register.B);
                    break;
                case 0xB1:
                    RES(6, Register.C);
                    break;
                case 0xB2:
                    RES(6, Register.D);
                    break;
                case 0xB6:
                    RES_HLm(6);
                    break;
                case 0xB7:
                    RES(6, Register.A);
                    break;
                case 0xB8:
                    RES(7, Register.B);
                    break;
                case 0xB9:
                    RES(7, Register.C);
                    break;
                case 0xBA:
                    RES(7, Register.D);
                    break;
                case 0xBE:
                    RES_HLm(7);
                    break;
                case 0xBF:
                    RES(7, Register.A);
                    break;
                case 0xC6:
                    SET_HLm(0);
                    break;
                case 0xCE:
                    SET_HLm(1);
                    break;
                case 0xD6:
                    SET_HLm(2);
                    break;
                case 0xDE:
                    SET_HLm(3);
                    break;
                case 0xE6:
                    SET_HLm(4);
                    break;
                case 0xEE:
                    SET_HLm(5);
                    break;
                case 0xF6:
                    SET_HLm(6);
                    break;
                case 0xFE:
                    SET_HLm(7);
                    break;
                default:
                    throw new IllegalOpcodeException(opCode, ProgramCounter);
            }

            if (!_cbInstructions.ContainsKey(opCode))
                throw new IllegalOpcodeException(opCode, ProgramCounter);
            var instructionMetaData = _cbInstructions[opCode];
            ProgramCounter += instructionMetaData.Size;
            Cycles += instructionMetaData.Cycles;
        }

        private void SET_HLm(int bit)
        {
            var value = _mmu.GetByte(HL);
            var result = value | (1 << bit);
            _mmu.SetByte(HL, (byte)result);
        }

        private void RES_HLm(int bit)
        {
            var value = _mmu.GetByte(HL);
            var mask = ~(1 << bit);
            _mmu.SetByte(HL, (byte)(value & mask));
        }

        private void RES(byte bit, Register register)
        {
            _registers[register] = (byte)(_registers[register] & ~(1 << bit));
        }
    }
}