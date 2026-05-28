using System;
using UnityEngine;

namespace Ecosim
{
    public struct StorageInfo
    {
        public readonly int SlotsReservedCount;
        public readonly int SlotsFreeCount;

        public readonly int CurrentTotalAmount;
        public readonly int MaxReservedCapacity;
        public readonly float ReservedFillPercentage; 

        public double SlotsReservedRatio => Math.Round((double)SlotsReservedCount / (SlotsReservedCount + SlotsFreeCount));
        public bool IsCompletelyEmpty => SlotsReservedCount == 0;
        public bool IsFull => SlotsFreeCount == 0 && ReservedFillPercentage >= 1.0f;

        public StorageInfo(int slotReservedCount, int slotsFreeCount, int currentTotalAmount, 
            int maxReservedCapacity, float reservedFillPercentage)
        {
            SlotsReservedCount = slotReservedCount;
            SlotsFreeCount = slotsFreeCount;
            CurrentTotalAmount = currentTotalAmount;
            MaxReservedCapacity = maxReservedCapacity;
            ReservedFillPercentage = reservedFillPercentage;
        }
    }
}
