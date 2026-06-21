using System;

namespace Ecosim
{
    public class DescriptionComponentSnapshot : IComponentSnapshot
    {
        public Type ComponentType => typeof(DescriptionComponent);
    }

    public class DescriptionComponent : IEntityComponent
    {
        public readonly string Name;
        public readonly string Description;

        public DescriptionComponent(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public void Reset()
        {
            
        }

        public IComponentSnapshot GetSnapshot()
        {
            return new DescriptionComponentSnapshot();
        }

        public void Restore(IComponentSnapshot snapshot)
        {
            
        }
    }
}
