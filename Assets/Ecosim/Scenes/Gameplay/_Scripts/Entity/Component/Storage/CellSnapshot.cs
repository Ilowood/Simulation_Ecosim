using System;

namespace Ecosim
{
    [Serializable]
    public class CellSnapshot
    {
        public readonly ushort Amount;

        public CellSnapshot(ushort amount)
        {
            Amount = amount;
        }
    }
}
