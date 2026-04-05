using UnityEngine;

namespace BugColony
{
    public class PredatorBehaviour : IBehaviour
    {
        private readonly float _lifeTimeInSeconds;
        private float _timer;

        public PredatorBehaviour(float lifeTimeInSeconds)
        {
            _lifeTimeInSeconds = lifeTimeInSeconds;
            _timer = 0;
        }

        public void Tick(Entity entity, SimulationContext context, float deltaTime)
        {
            if (_timer > _lifeTimeInSeconds)
            {
                entity.Behavior.EndTask();
                context.RemoveEntityCommand(entity);
                return;
            }

            if (entity.Behavior.Task == null || entity.Behavior.Task.IsComplete)
            {
                
            }

            _timer += deltaTime;
        }
    }
}
