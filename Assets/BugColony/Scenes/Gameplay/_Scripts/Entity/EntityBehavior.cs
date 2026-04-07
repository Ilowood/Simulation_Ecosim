namespace BugColony
{
    public interface IBehaviour
    {
        void Tick(Entity entity, SimulationContext context, float deltaTime, float scale);
    }

    public class EntityBehavior
    {
        private readonly IBehaviour _behaviour;

        public IEntityTask Task { get; private set; }

        public EntityBehavior(BehaviourSpecification behaviour)
        {
            _behaviour = behaviour.Create();
        }

        public void Tick(Entity entity, SimulationContext context, float deltaTime, float scale)
        {
            _behaviour?.Tick(entity, context, deltaTime, scale);
            Task?.Tick(deltaTime, scale);
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

        public void SetPause(bool isPause)
        {
            if (isPause) Task?.Puase();
            else Task?.Resume();
        }
    }
}
