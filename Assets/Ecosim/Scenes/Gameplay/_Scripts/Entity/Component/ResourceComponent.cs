namespace Ecosim
{
    public class ResourceComponent : IEntityComponent
    {
        public int Amount;
        public int Capacity { get; }

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
