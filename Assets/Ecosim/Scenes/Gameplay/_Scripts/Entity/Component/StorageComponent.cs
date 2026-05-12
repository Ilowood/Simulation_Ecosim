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

        public IComponentSnapshot GetSnapshot()
        {
            var slotSnapshots = new StorageSlotSnapshot[Slots.Length];

            for (int i = 0; i < Slots.Length; i++)
            {
                var sourceSlot = Slots[i];
                
                slotSnapshots[i] = new StorageSlotSnapshot 
                { 
                    ResourceId = sourceSlot.ResourceId, 
                    Amount = sourceSlot.Amount 
                };
            }

            return new StorageComponentSnapshot(slotSnapshots, CurrentTotalAmount);
        }

        public void Restore(IComponentSnapshot snapshot)
        {
            if (snapshot is StorageComponentSnapshot data)
            {
                CurrentTotalAmount = data.CurrentTotalAmount;

                int i = 0;
                foreach (var slotSnapshot in data.Slots)
                {
                    Slots[i].Restore(slotSnapshot.ResourceId, slotSnapshot.Amount);
                    i++;
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
