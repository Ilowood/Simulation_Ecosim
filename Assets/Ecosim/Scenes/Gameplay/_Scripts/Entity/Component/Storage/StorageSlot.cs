using System.Collections.Generic;
using System.Linq;

namespace Ecosim
{
    public class StorageSlot
    {
        private List<Cell> _cells;
        
        public long? SpecId { get; private set; } 
        public int MaxCells { get; private set; } 

        public int CountCells => _cells.Count;
        public bool IsEmpty => SpecId == null;

        public StorageSlot(int cellCount)
        {
            MaxCells = cellCount;
            _cells = new(cellCount);
        }

        public void SetSpecId(long specId) => SpecId = specId;
        public void AddCell(ushort amount) => _cells.Add(new Cell(amount));
        public void RemoveCellAt(int index) => _cells.RemoveAt(index);

        public bool IsFull(int sizeStack) => _cells.Count == MaxCells && _cells.TrueForAll(x => x.Amount >= sizeStack);
        public int GetTotalAmount() => _cells.Sum(x => x.Amount);

        public Cell GetCell(int index) => _cells[index];

        public void Restore(long? resourceId, List<CellSnapshot> cells)
        {
            SpecId = resourceId;

            for (var i = 0; i < cells.Count; i++)
            {
                _cells.Add(new Cell(cells[i].Amount));
            }
        }

        public void Reset()
        {
            SpecId = null;
            _cells.Clear();
        }
    }
}
