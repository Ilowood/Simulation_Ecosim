namespace BugColony
{
    public interface IBehaviour
    {
        void Tick(Entity entity, SimulationContext context, float deltaTime);
    }

    public class EntityBehavior
    {
        private readonly IBehaviour _behaviour;

        public IEntityTask Task { get; private set; }

        public EntityBehavior(BehaviourSpecification behaviour)
        {
            _behaviour = behaviour.Create();
        }

        public void Tick(Entity entity, SimulationContext context, float deltaTime)
        {
            _behaviour?.Tick(entity, context, deltaTime);
            Task?.Tick(deltaTime);
        }

        public void SetAndStartTask(IEntityTask task)
        {
            if (Task != null && !Task.IsComplete)
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
