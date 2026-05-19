using System;

namespace Ecosim
{
    public static class StorageService
    {
        public static bool IsAllSlotsReserved(this StorageComponent storage)
        {
            for (int i = 0; i < storage.Slots.Length; i++)
            {
                if (storage.Slots[i].IsEmpty) return false;
            }

            return true;
        }

        public static bool CanAdd(this StorageComponent storage, long specId, bool isStackable)
        {
            var effectiveStackSize = isStackable ? storage.StackSize : 1;

            for (var i = 0; i < storage.Slots.Length; i++)
            {
                var slot = storage.Slots[i];

                if (slot.SpecId == specId)
                {
                    if (isStackable)
                    {
                        for (int j = 0; j < slot.CountCells; j++)
                        {
                            if (slot.GetCell(j).Amount < effectiveStackSize)
                            {
                                return true;
                            }
                        }
                    }

                    if (slot.CountCells < slot.MaxCells)
                    {
                        return true;
                    }
                }
                else if (slot.IsEmpty)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Пытается добавить предметы на склад с учетом того, стакаются они или нет.
        /// Возвращает количество предметов, которые НЕ ПОМЕСТИЛИСЬ (0, если все добавлено).
        /// </summary>
        public static int TryAdd(this StorageComponent storage, long specId, int amount, bool isStackable)
        {
            var effectiveStackSize = isStackable ? storage.StackSize : 1;

            for (var i = 0; i < storage.Slots.Length; i++)
            {
                var slot = storage.Slots[i];

                if (slot.SpecId == specId)
                {
                    amount = DistributeToSlot(slot, effectiveStackSize, amount, isStackable);
                    if (amount == 0) return 0;
                }
            }

            for (int i = 0; i < storage.Slots.Length; i++)
            {
                var slot = storage.Slots[i];

                if (slot.IsEmpty)
                {
                    slot.SetSpecId(specId);
                    amount = DistributeToSlot(slot, effectiveStackSize, amount, isStackable);
                    if (amount == 0) return 0;
                }
            }

            return amount;
        }

        private static int DistributeToSlot(StorageSlot slot, int effectiveStackSize, int amount, bool isStackable)
        {
            if (isStackable)
            {
                for (int j = 0; j < slot.CountCells; j++)
                {
                    var cell = slot.GetCell(j);
                    int spaceLeft = effectiveStackSize - cell.Amount;

                    if (spaceLeft > 0)
                    {
                        int toAdd = Math.Min(spaceLeft, amount);
                        cell.Amount += (ushort)toAdd;
                        amount -= toAdd;

                        if (amount == 0) return 0;
                    }
                }
            }

            while (slot.CountCells < slot.MaxCells && amount > 0)
            {
                int toAdd = Math.Min(effectiveStackSize, amount);
                slot.AddCell((ushort)toAdd);
                amount -= toAdd;
            }

            return amount;
        }

        /// <summary>
        /// Пытается удалить предметы со склада. Автоматически очищает пустые ячейки и слоты.
        /// Возвращает количество предметов, которые НЕ НАШЛИСЬ (0, если успешно удалено всё).
        /// </summary>
        public static int TryRemove(this StorageComponent storage, long specId, int amount)
        {
            if (amount <= 0 || specId == 0) return amount;

            for (int i = storage.Slots.Length - 1; i >= 0; i--)
            {
                var slot = storage.Slots[i];

                if (slot.SpecId == specId)
                {
                    amount = RemoveFromSlot(slot, amount);
                    if (amount == 0) return 0;
                }
            }

            return amount;
        }

        private static int RemoveFromSlot(StorageSlot slot, int amount)
        {
            for (int j = slot.CountCells - 1; j >= 0; j--)
            {
                var cell = slot.GetCell(j);

                if (cell.Amount <= amount)
                {
                    amount -= cell.Amount;
                    slot.RemoveCellAt(j);
                }
                else
                {
                    cell.Amount -= (ushort)amount;
                    amount = 0;
                    break;
                }
            }

            if (slot.CountCells == 0)
            {
                slot.Reset();
            }

            return amount;
        }
    }
}
