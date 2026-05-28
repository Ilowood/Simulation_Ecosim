using System.Collections.Generic;
using System.Linq;

namespace Ecosim
{
    public class StorageComponent : IEntityComponent
    {
        public readonly StorageSlot[] Slots;
        public readonly int PossibleStackSize;

        public StorageComponent(int slotCount, int cellsPerSlot, int possibleStackSize)
        {
            Slots = new StorageSlot[slotCount];
            PossibleStackSize = possibleStackSize;

            for (int i = 0; i < slotCount; i++) 
            {
                Slots[i] = new(cellsPerSlot);
            }
        }

        public IComponentSnapshot GetSnapshot()
        {
            var slotSnapshots = new StorageSlotSnapshot[Slots.Length];

            for (var i = 0; i < Slots.Length; i++)
            {
                var slot = Slots[i];
                var cells = new CellSnapshot[slot.Cells.Length];

                for (var j = 0; j < cells.Length; j++)
                {
                    cells[j] = new CellSnapshot(slot.Cells[j].Amount);
                }

                slotSnapshots[i] = new StorageSlotSnapshot(slot.SpecId, cells);
            }

            return new StorageComponentSnapshot(slotSnapshots);
        }

        public void Restore(IComponentSnapshot snapshot)
        {
            if (snapshot is StorageComponentSnapshot data)
            {
                var slots = data.Slots;
                for (var i = 0; i < slots.Count(); i++)
                {
                    Slots[i].Restore(slots[i].SpecId, slots[i].Cells);
                }
            }
        }

        public void Reset()
        {
            for (int i = 0; i < Slots.Count(); i++) 
            {
                Slots[i].Reset();
            }
        }
    }
}
