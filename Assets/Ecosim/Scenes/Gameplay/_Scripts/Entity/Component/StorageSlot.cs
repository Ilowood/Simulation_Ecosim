namespace Ecosim
{
    public class StorageSlot
    {
        public long? ResourceId { get; private set; }
        public int Capacity { get; private set; }
        public int Amount { get; private set; } = 0;

        public bool IsEmpty => ResourceId == null;
        public bool IsFull => Amount == Capacity;

        public StorageSlot(int capacity)
        {
            Capacity = capacity;
        }

        public void Restore(long? resourceId, int amount)
        {
            ResourceId = resourceId;
            Amount = amount;
        }

        public void Reset()
        {
            ResourceId = null;
            Amount = 0;
        }
    }
}
