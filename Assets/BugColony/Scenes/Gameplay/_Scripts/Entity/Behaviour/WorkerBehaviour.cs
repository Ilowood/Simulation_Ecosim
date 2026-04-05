using UnityEngine;

namespace BugColony
{
    public class WorkerBehaviour : IBehaviour
    {
        private readonly float _speed;
        private readonly int _eatToSplit;

        private int _eaten = 0;

        public WorkerBehaviour(float speed, int eatToSplit)
        {
            _speed = speed;
            _eatToSplit = eatToSplit;
        }

        public void Tick(Entity entity, SimulationContext context, float deltaTime)
        {
            if (entity.Behavior.Task == null || entity.Behavior.Task.IsComplete)
            {
                if (_eaten < _eatToSplit)
                {
                    // entity.Behavior.SetTask(new MoveToTask());
                }
                else
                {
                    
                }
            }
        }
    }
}
