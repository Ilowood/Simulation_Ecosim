using System;
using System.Collections.Generic;

namespace Ecosim
{
    [Serializable]
    public class StorageSlotSnapshot
    {
        public readonly long? SpecId;
        public readonly List<CellSnapshot> Cells;

        public StorageSlotSnapshot(long? specId, List<CellSnapshot> cells)
        {
            SpecId = specId;
            Cells = cells;
        }
    }
}
