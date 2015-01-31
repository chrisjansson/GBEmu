namespace Core
{
    public interface IDisplayRenderer
    {
        void TransferScanLine(int line);
        void FinishFrame();
    }
}