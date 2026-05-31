using System;

namespace Ecosim
{
    [Serializable]
    public struct ResourceTransferParams : ITaskParams
    {
        public long DestinationStorageId;
        public long ResourceId;
        public int Amount;
        public float CurrentTime;

        public TaskVariants Variants => TaskVariants.TransferResource;
    }
}
