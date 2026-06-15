using System;

namespace Ecosim
{
    public class StorageServic
    {
        private readonly ItemDatabase _registry;

        public StorageServic(ItemDatabase registry)
        {
            _registry = registry;
        }

        public bool IsFull(StorageComponent storage) => GetStorageInfo(storage).IsFull;
        public bool CanAddAny(StorageComponent storage, long specId) => 0 < GetAvailableSpaceFor(storage, specId);
        public bool CanAddHalf(StorageComponent storage, long specId, int amount) => CanAddFraction(storage, specId, amount, 0.5f);
        public bool CanAddAll(StorageComponent storage, long specId, int amount) => amount <= GetAvailableSpaceFor(storage, specId);

        public bool CanAddFraction(StorageComponent storage, long specId, int totalAmount, float fraction)
        {
            var availableSpace = GetAvailableSpaceFor(storage, specId);
            var requiredAmount = (int)Math.Ceiling(totalAmount * fraction);
            
            return requiredAmount <= availableSpace;
        }

        public bool IsAllSlotsReserved(StorageComponent storage)
        {
            foreach (var slot in storage.Slots) 
            {
                if (!slot.SpecId.HasValue) return false;
            }

            return true;
        }

        public int GetAvailableSpaceFor(StorageComponent storage, long specId)
        {
            var stackSize = _registry.GetById(specId).IsStackable ? storage.PossibleStackSize : 1;
            var freeSpace = GetFreeSpaceFor(storage.Slots, stackSize);
            var reservedSpace = GetReservedSpaceFor(storage.Slots, specId, stackSize);

            return freeSpace + reservedSpace;
        }

        public bool HasItem(StorageComponent storage, long specId)
        {
            foreach (var slot in storage.Slots)
            {
                if (slot.SpecId.HasValue && slot.SpecId.Value == specId)
                    return true;
            }

            return false;
        }

        public int GetItemCount(StorageComponent storage, long specId)
        {
            var totalCount = 0;
            foreach (var slot in storage.Slots)
            {
                if (slot.SpecId.HasValue && slot.SpecId.Value == specId)
                {
                    totalCount += CountItemsInCells(slot.Cells);
                }
            }
            return totalCount;
        }

        public StorageInfo GetStorageInfo(StorageComponent storage)
        {
            var reservedCount = 0;
            var freeCount = 0;
            var totalAmount = 0;
            var maxReservedCapacity = 0;

            foreach (var slot in storage.Slots)
            {
                if (slot.SpecId.HasValue)
                {
                    reservedCount++;
                    
                    var slotAmount = GetCurrentAmount(slot);
                    totalAmount += slotAmount;
                    maxReservedCapacity += GetReservedSlotCapacity(storage, slot);
                }
                else
                {
                    freeCount++;
                }
            }

            var reservedFillPercentage = maxReservedCapacity == 0 ? 0f : (float)totalAmount / maxReservedCapacity;

            return new StorageInfo(
                reservedCount, 
                freeCount,
                totalAmount, 
                maxReservedCapacity, 
                reservedFillPercentage
            );
        }

        public int TryTransfer(StorageComponent from, StorageComponent to, long specId, int amount)
        {
            var availableInSource = GetItemCount(from, specId);
            var freeSpaceInTarget = GetAvailableSpaceFor(to, specId);

            var amountToMove = Math.Min(amount, availableInSource);
            var finalTransferAmount = Math.Min(amountToMove, freeSpaceInTarget);

            var leftoverInSource = TryRemove(from, specId, finalTransferAmount);
            var actualRemoved = finalTransferAmount - leftoverInSource;
            
            var leftoverInTarget = TryAdd(to, specId, actualRemoved);
            var successfullyAdded = actualRemoved - leftoverInTarget;

            if (successfullyAdded < actualRemoved)
            {
                var toReturn = actualRemoved - successfullyAdded;
                TryAdd(from, specId, toReturn);
            }
            
            return successfullyAdded;
        }

        public int TryRemove(StorageComponent storage, long specId, int amount)
        {
            return RemoveFromMatchingSlots(storage.Slots, specId, amount);
        }

        public int TryAdd(StorageComponent storage, long specId, int amount)
        {
            var stackSize = GetMaxStackSize(specId, storage.PossibleStackSize);

            amount = FillReservedSlots(storage.Slots, specId, amount, stackSize);
            amount = FillEmptySlots(storage.Slots, specId, amount, stackSize);

            return amount;
        }

        private int GetMaxStackSize(long specId, int possibleStackSize)
        {
            return _registry.GetById(specId).IsStackable ? possibleStackSize : 1;
        }

        private int GetSlotCapacity(StorageSlot slot, int maxStackSize)
        {
            return slot.Cells.Length * maxStackSize;
        }

        private int GetReservedSlotCapacity(StorageComponent storage, StorageSlot slot)
        {
            var maxStackSize = GetMaxStackSize(slot.SpecId.Value, storage.PossibleStackSize);
            return GetSlotCapacity(slot, maxStackSize);
        }

        private int GetCurrentAmount(StorageSlot slot)
        {
            return slot.SpecId.HasValue ? CountItemsInCells(slot.Cells) : 0;
        }

        private int CountItemsInCells(Cell[] cells)
        {
            var totalAmount = 0;
            
            foreach(var cell in cells) totalAmount += cell.Amount;
            return totalAmount;
        }

        private int GetFreeSpaceFor(StorageSlot[] slots, int stackSize)
        {
            var totalFreeSpace = 0;

            foreach (var slot in slots)
            {
                if (!slot.SpecId.HasValue)
                    totalFreeSpace += GetSlotCapacity(slot, stackSize);
            }

            return totalFreeSpace;
        }

        private int GetReservedSpaceFor(StorageSlot[] slots, long specId, int stackSize)
        {
            var space = 0;

            foreach (var slot in slots)
            {
                if (slot.SpecId.HasValue && slot.SpecId == specId)
                {
                    var amountItems = CountItemsInCells(slot.Cells);
                    space += GetSlotCapacity(slot, stackSize) - amountItems;
                }
            }

            return space;
        }

        private int FillReservedSlots(StorageSlot[] slots, long specId, int amount, int maxStackSize)
        {
            foreach (var slot in slots)
            {
                if (slot.SpecId.HasValue && slot.SpecId == specId)
                {
                    amount = FillSlotCells(slot.Cells, amount, maxStackSize);
                    if (amount == 0) return 0;
                }
            }

            return amount;
        }

        private int FillEmptySlots(StorageSlot[] slots, long specId, int amount, int maxStackSize)
        {
            foreach (var slot in slots)
            {
                if (!slot.SpecId.HasValue)
                {
                    slot.SpecId = specId;
                    amount = FillSlotCells(slot.Cells, amount, maxStackSize);
                    if (amount == 0) return 0;
                }
            }

            return amount;
        }

        private int FillSlotCells(Cell[] cells, int amount, int maxStackSize)
        {
            foreach (var cell in cells)
            {
                var freeSpace = maxStackSize - cell.Amount;
                
                if (freeSpace > 0)
                {
                    var toAdd = Math.Min(amount, freeSpace);
                    cell.Amount += toAdd;
                    amount -= toAdd;

                    if (amount == 0) return 0;
                }
            }

            return amount;
        }

        private int RemoveFromMatchingSlots(StorageSlot[] slots, long specId, int amount)
        {
            for (var i = slots.Length - 1; i >= 0 && amount > 0; i--)
            {
                var slot = slots[i];
                
                if (slot.SpecId.HasValue && slot.SpecId.Value == specId)
                {
                    amount = RemoveFromSlotCells(slot.Cells, amount, out var isSlotEmpty);

                    if (isSlotEmpty)
                    {
                        slot.SpecId = null;
                    }
                }
            }

            return amount;
        }

        private int RemoveFromSlotCells(Cell[] cells, int amount, out bool isSlotEmpty)
        {
            var hasAnyItemsLeft = false;

            for (var j = cells.Length - 1; j >= 0; j--)
            {
                if (amount > 0 && cells[j].Amount > 0)
                {
                    var countRemoved = cells[j].Amount > amount ? amount : cells[j].Amount;
                    cells[j].Amount -= countRemoved;
                    amount -= countRemoved;
                }

                if (cells[j].Amount > 0)
                {
                    hasAnyItemsLeft = true;
                }
            }

            isSlotEmpty = !hasAnyItemsLeft;
            return amount;
        }
    }
}
