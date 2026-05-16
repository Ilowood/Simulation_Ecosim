using System.Collections.Generic;

namespace Ecosim
{
    public interface IReadOnlyEntityStorage
    {
        IReadOnlyList<Entity> Get(long specId);
        int GetCount(long specId);
        int GetTrackedCount();
    }

    public struct WorldContext
    {
        private readonly World _simulation;

        public IReadOnlyEntityStorage Entities { get; private set; }

        public WorldContext(World simulation)
        {
            _simulation = simulation;
            Entities = simulation;
        }

        public void RemoveEntityWithoutCoolbackCommand(Entity entity)
        {
            _simulation.AddCommand(new RemoveEntityWithoutCoolbackCommand(entity));
        }

        public void RemoveEntityWithCoolbackCommand(Entity entity)
        {
            _simulation.AddCommand(new RemoveEntityWithCoolbackCommand(entity));
        }

        public void SpawnEntityCompand(long specId)
        {
            _simulation.AddCommand(new SpawnEntityCommand(specId));
        }
    }
}
