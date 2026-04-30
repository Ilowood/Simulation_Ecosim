using System.Linq;

namespace Ecosim
{
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

        public object GetSnapshot()
        {
            var snapshot = new StorageComponentSnapshot();
            snapshot.CurrentTotalAmount = CurrentTotalAmount;

            foreach (var slot in Slots)
            {
                snapshot.Slots.Add(new StorageSlotSnapshot 
                { 
                    ResourceId = slot.ResourceId, 
                    Amount = slot.Amount 
                });
            }
            return snapshot;
        }

        public void Restore(object snapshot)
        {
            if (snapshot is StorageComponentSnapshot data)
            {
                CurrentTotalAmount = data.CurrentTotalAmount;

                for (int i = 0; i < Slots.Length; i++)
                {
                    if (i < data.Slots.Count)
                    {
                        var slotData = data.Slots[i];
                        Slots[i].Restore(slotData.ResourceId, slotData.Amount);
                    }
                }
            }
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
