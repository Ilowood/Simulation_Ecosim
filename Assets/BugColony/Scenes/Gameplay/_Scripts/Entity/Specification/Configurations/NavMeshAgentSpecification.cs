using UnityEngine;
using UnityEngine.AI;

namespace BugColony
{
    [CreateAssetMenu(menuName = "Entity/Variants/NavMeshAgent", fileName = "NavMeshAgentSpecification")]
    public class NavMeshAgentSpecification : Specification
    {
        [SerializeField] private float _stopDistance;

        public override void Apply(Entity entity)
        {
            var agent = entity.gameObject.AddComponent<NavMeshAgent>();

            agent.stoppingDistance = _stopDistance;
            entity.SetNavMeshAgent(agent);
        }
    }
}
