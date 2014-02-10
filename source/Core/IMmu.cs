namespace Core
{
    public interface IMmu
    {
        byte GetByte(ushort address);
        void SetByte(ushort address, byte value);
    }
}