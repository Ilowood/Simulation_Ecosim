using Zenject;

namespace Ecosim
{
    public interface IBehaviour
    {
        void Tick(WorldContext context, float deltaTime, float scale);
    }

    public class EntityBehavior
    {
        private readonly IBehaviour _behaviour;

        public IEntityTask Task { get; private set; }

        public EntityBehavior(IBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        public void Tick(WorldContext context, float deltaTime, float scale)
        {
            _behaviour?.Tick(context, deltaTime, scale);
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
