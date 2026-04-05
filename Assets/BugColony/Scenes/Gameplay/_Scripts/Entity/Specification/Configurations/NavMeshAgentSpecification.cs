using UnityEngine;
using UnityEngine.AI;

namespace BugColony
{
    [CreateAssetMenu(menuName = "Entity/Variants/NavMeshAgent", fileName = "NavMeshAgentSpecification")]
    public class NavMeshAgentSpecification : Specification
    {
        public override void Apply(Entity entity)
        {
            var agent = entity.gameObject.AddComponent<NavMeshAgent>();
            entity.SetNavMeshAgent(agent);
        }
    }
}
