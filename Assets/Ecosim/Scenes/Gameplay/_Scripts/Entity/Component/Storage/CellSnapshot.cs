using System;

namespace Ecosim
{
    [Serializable]
    public class CellSnapshot
    {
        public readonly int Amount;

        public CellSnapshot(int amount)
        {
            Amount = amount;
        }
    }
}
