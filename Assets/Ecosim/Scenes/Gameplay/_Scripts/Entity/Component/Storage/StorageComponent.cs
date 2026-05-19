using System.Collections.Generic;
using System.Linq;

namespace Ecosim
{
    public class StorageComponent : IEntityComponent
    {
        public StorageSlot[] Slots { get; private set; }
        public readonly int StackSize;

        public StorageComponent(int slotCount, int cellsPerSlot, int stackSize)
        {
            Slots = new StorageSlot[slotCount];
            StackSize = stackSize;

            for (int i = 0; i < slotCount; i++) 
            {
                Slots[i] = new(cellsPerSlot);
            }
        }

        public IComponentSnapshot GetSnapshot()
        {
            var slotSnapshots = new StorageSlotSnapshot[Slots.Length];

            for (int i = 0; i < Slots.Length; i++)
            {
                var sourceSlot = Slots[i];
                var cells = new List<CellSnapshot>(sourceSlot.MaxCells);

                for (var j = 0; j < sourceSlot.CountCells; j++)
                {
                    cells.Add(new CellSnapshot(sourceSlot.GetCell(j).Amount));
                }

                slotSnapshots[i] = new StorageSlotSnapshot(sourceSlot.SpecId, cells);
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
