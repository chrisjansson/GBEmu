namespace Core
{
    public interface IMBC
    {
        byte GetByte(ushort address);
        void SetByte(ushort address, byte value);
    }
}