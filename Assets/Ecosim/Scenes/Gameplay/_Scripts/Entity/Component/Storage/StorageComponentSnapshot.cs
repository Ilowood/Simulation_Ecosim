using System;

namespace Ecosim
{
    [Serializable]
    public class StorageComponentSnapshot : IComponentSnapshot
    {
        public readonly StorageSlotSnapshot[] Slots;

        public Type ComponentType => typeof(StorageComponent);

        public StorageComponentSnapshot(StorageSlotSnapshot[] slots)
        {
            Slots = slots;
        }
    }
}
