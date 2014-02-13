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
            FinishedFrames++;
        }

        public int FinishedFrames { get; private set; }
        public List<int> TransferedScanLines { get; private set; }
        public int LastTransferedScanLine { get { return TransferedScanLines.Last(); } }
    }
}