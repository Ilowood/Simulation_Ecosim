using UnityEngine;
using UnityEngine.AI;

namespace Ecosim
{
    public class UnitBehaviour : IBehaviour
    {
        private const float SearchRadius = 20f;
        
        public UnitBehaviour()
        {
            
        }

        public void Tick(Entity entity, WorldContext context, float deltaTime, float scale)
        {
            if (entity.Behavior.Task == null || entity.Behavior.Task.IsComplete)
            {
                var randomPos = GetRandomPoint(entity.transform.position, SearchRadius);
                entity.Behavior.SetAndStartTask(new MoveToPointTask(entity, randomPos));
            }
        }

        private Vector3 GetRandomPoint(Vector3 center, float radius)
        {
            var randomCircle = Random.insideUnitCircle * radius;
            var direction = new Vector3(randomCircle.x, 0, randomCircle.y);
            var targetPos = center + direction;

            if (NavMesh.SamplePosition(targetPos, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
            {
                return hit.position;
            }

            return center;
        }
    }
}
