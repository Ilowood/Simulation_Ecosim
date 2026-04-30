using System;
using System.Collections.Generic;

namespace Ecosim
{
    [Serializable]
    public class StorageComponentSnapshot
    {
        public List<StorageSlotSnapshot> Slots = new();
        public int CurrentTotalAmount;
    }
}
