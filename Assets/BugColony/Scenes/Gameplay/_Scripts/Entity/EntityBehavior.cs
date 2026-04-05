using UnityEngine;

namespace BugColony
{
    public interface IBehaviour
    {
        void Tick(Entity entity, SimulationContext context, float deltaTime);
    }

    public class EntityBehavior
    {
        private readonly IBehaviour _behaviour;
        private readonly Entity _entity;

        public IEntityTask Task { get; private set; }

        public EntityBehavior(Entity entity, BehaviourSpecification behaviour)
        {
            _entity = entity;
            _behaviour = behaviour.Create();
        }

        public void Tick(Entity entity, SimulationContext context, float dt)
        {
            _behaviour?.Tick(_entity, context, dt);
        }

        public void SetTask(IEntityTask task)
        {
            Task?.End();
            Task = task;
            Task.Start();
        }

        public void EndTask()
        {
            Task?.End();
            Task = null;
        }
    }
}
