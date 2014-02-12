using System.Collections.Generic;
using System.Linq;
using Core;

namespace Test.Display
{
    public class FakeDisplayDataTransferService : IDisplayDataTransferService
    {
        public FakeDisplayDataTransferService()
        {
            TransferedScanLines = new List<int>();
        }

        public void TransferScanLine(int line)
        {
            TransferedScanLines.Add(line);
        }

        public void FinishFrame()
        {
            throw new System.NotImplementedException();
        }

        public List<int> TransferedScanLines { get; private set; }
        public int LastTransferedScanLine { get { return TransferedScanLines.Last(); } }
    }
}