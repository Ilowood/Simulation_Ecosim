using UnityEngine;

namespace Ecosim
{
    public class UnitBehaviour : IBehaviour
    {
        public UnitBehaviour()
        {
            
        }

        public void Tick(Entity entity, SimulationContext context, float deltaTime, float scale)
        {
            if (entity.Behavior.Task == null || entity.Behavior.Task.IsComplete)
            {
                
            }
        }
    }
}
