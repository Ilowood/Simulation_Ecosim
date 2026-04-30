using System;

namespace Ecosim
{
    [Serializable]
    public struct ResourceSnapshot
    {
        public readonly int Amount;

        public ResourceSnapshot(int amount)
        {
            Amount = amount;
        }
    }
}
