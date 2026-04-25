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

        public void Reset()
        {
            Amount = Capacity;
        }
    }
}
