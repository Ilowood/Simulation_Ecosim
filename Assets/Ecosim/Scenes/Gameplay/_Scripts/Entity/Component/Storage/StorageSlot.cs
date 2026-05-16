using System.Collections.Generic;

namespace Ecosim
{
    public class StorageSlot
    {
        private long? _specId;
        private List<Cell> _cells;
        
        public int MaxCells { get; private set; } 

        public bool IsEmpty => _specId == null;
        public bool IsFull(int sizeStack) => _cells.TrueForAll(x => x.Amount >= sizeStack);

        public StorageSlot(int maxCells)
        {
            MaxCells = maxCells;
            _cells = new(maxCells);
        }

        public void Restore(long? resourceId, List<Cell> cells)
        {
            _specId = resourceId;
            _cells = cells;
        }

        public void Reset()
        {
            _specId = null;
            _cells = null;
        }
    }
}
