using System;

namespace Ecosim
{
    public struct ResourceTransferParams : ITaskParams
    {
        public long DestinationStorageId;
        public long ResourceId;
        public int Amount;
        public float CurrentTime;
    }
}
