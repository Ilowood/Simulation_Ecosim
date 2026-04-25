using System.Linq;

namespace Ecosim
{
    public class StorageSlot
    {
        public long? ResourceId { get; private set; }
        public int Capacity { get; private set; }
        public int Amount { get; private set; } = 0;

        public bool IsEmpty => ResourceId == null;
        public bool IsFull => Amount == Capacity;

        public StorageSlot(int capacity)
        {
            Capacity = capacity;
        }

        public void Reset()
        {
            ResourceId = null;
            Amount = 0;
        }
    }

    public class StorageComponent : IEntityComponent
    {
        public StorageSlot[] Slots { get; private set; }
        public int MaxTotalCapacity { get; private set; }
        public int CurrentTotalAmount { get; private set; } = 0;

        public bool IsFull => Slots.All(x => x.IsFull);

        public StorageComponent(int countSlots, int slotCapacity)
        {
            Slots = new StorageSlot[countSlots];
            for (int i = 0; i < countSlots; i++) 
            {
                Slots[i] = new(slotCapacity);
            }

            MaxTotalCapacity = countSlots * slotCapacity;
        }

        public void Reset()
        {
            for (int i = 0; i < Slots.Count(); i++) 
            {
                Slots[i].Reset();
            }

            CurrentTotalAmount = 0;
        }
    }
}
