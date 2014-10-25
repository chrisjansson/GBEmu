namespace Core
{
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
}