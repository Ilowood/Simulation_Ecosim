namespace Ecosim
{
    public class StorageSlot
    {
        public readonly Cell[] Cells;

        public long? SpecId;

        public StorageSlot(int cellCount)
        {
            Cells = new Cell[cellCount];
            for (int i = 0; i < cellCount; i++) Cells[i] = new Cell(0);
        }

        public void Restore(long? resourceId, CellSnapshot[] cells)
        {
            SpecId = resourceId;

            for (var i = 0; i < cells.Length; i++)
            {
                Cells[i] = new Cell(cells[i].Amount);
            }
        }

        public void Reset()
        {
            SpecId = null;
            
            foreach (var cell in Cells) 
            {
                cell.Amount = 0;
            }
        }
    }
}
