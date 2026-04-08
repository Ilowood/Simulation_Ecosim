using UnityEngine;
using UnityEngine.AI;

namespace Ecosim
{
    [CreateAssetMenu(menuName = "Entity/Variants/NavMeshAgent", fileName = "NavMeshAgentSpecification")]
    public class NavMeshAgentSpecification : Specification
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _stopDistance;

        public override void Apply(Entity entity)
        {
            var agent = entity.gameObject.AddComponent<NavMeshAgent>();

            agent.speed = _speed;
            agent.acceleration = _acceleration;
            agent.stoppingDistance = _stopDistance;
            entity.SetNavMeshAgent(agent);
        }
    }
}
