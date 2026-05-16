using System;
using System.Collections.Generic;

namespace Ecosim
{
    [Serializable]
    public class StorageSlotSnapshot
    {
        public readonly long? SpecId;
        public readonly List<Cell> Cells;

        public StorageSlotSnapshot(long? specId, List<Cell> cells)
        {
            SpecId = specId;
            Cells = cells;
        }
    }
}
