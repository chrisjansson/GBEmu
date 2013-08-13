using Core;

namespace Test
{
    public class FakeMmu : IMmu
    {
        public readonly byte[] Memory = new byte[ushort.MaxValue];

        public byte GetByte(ushort address)
        {
            return Memory[address];
        }
    }
}