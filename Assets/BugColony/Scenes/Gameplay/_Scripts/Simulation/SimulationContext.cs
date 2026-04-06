using System.Collections.Generic;

namespace BugColony
{
    public interface IReadOnlyEntityStorage
    {
        IReadOnlyList<Entity> Get(EntityType type);
        int GetCount(EntityType type);
    }

    public struct SimulationContext
    {
        private readonly Simulation _simulation;

        public IReadOnlyEntityStorage Entities { get; private set; }

        public SimulationContext(Simulation simulation)
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

        public void SpawnEntityCompand(int count, EntityType type)
        {
            _simulation.AddCommand(new SpawnEntityCommand(count, type));
        }
    }
}
