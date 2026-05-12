namespace Ecosim
{
    public class ResourceComponent : IEntityComponent
    {
        public int Amount { get; set; }
        public int Capacity { get; private set; }

        public ResourceComponent(int capacity)
        {
            Capacity = capacity;
            Amount = capacity;
        }

        public IComponentSnapshot GetSnapshot()
        {
            return new ResourceSnapshot(Amount);
        }

        public void Restore(IComponentSnapshot snapshot)
        {
            if (snapshot is ResourceSnapshot data)
            {
                Amount = data.Amount;
            }
        }

        public void Reset()
        {
            Amount = 0;
        }
    }
}
