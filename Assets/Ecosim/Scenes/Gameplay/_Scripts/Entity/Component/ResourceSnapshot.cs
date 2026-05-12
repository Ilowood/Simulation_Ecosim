using System;

namespace Ecosim
{
    [Serializable]
    public struct ResourceSnapshot : IComponentSnapshot
    {
        public readonly int Amount;

        public Type ComponentType => typeof(ResourceComponent);

        public ResourceSnapshot(int amount)
        {
            Amount = amount;
        }
    }
}
