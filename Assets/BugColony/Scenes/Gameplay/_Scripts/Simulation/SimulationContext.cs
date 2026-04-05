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

        public void RemoveEntityCommand(Entity entity)
        {
            _simulation.AddCommand(new RemoveEntityCommand(entity));
        }
    }
}
