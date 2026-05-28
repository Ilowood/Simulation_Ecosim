using System;

namespace Ecosim
{
    [Serializable]
    public class StorageSlotSnapshot
    {
        public readonly long? SpecId;
        public readonly CellSnapshot[] Cells;

        public StorageSlotSnapshot(long? specId, CellSnapshot[] cells)
        {
            SpecId = specId;
            Cells = cells;
        }
    }
}
