using System;

namespace Ecosim
{
    [Serializable]
    public class StorageComponentSnapshot : IComponentSnapshot
    {
        public readonly StorageSlotSnapshot[] Slots;
        public readonly int CurrentTotalAmount;

        public Type ComponentType => typeof(StorageComponent);

        public StorageComponentSnapshot(StorageSlotSnapshot[] slots, int currentTotalAmount)
        {
            Slots = slots;
            CurrentTotalAmount = currentTotalAmount;
        }
    }
}
