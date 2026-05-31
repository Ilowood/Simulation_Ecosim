using UnityEngine;
using UnityEngine.AI;

namespace Ecosim
{
    [CreateAssetMenu(menuName = "Ecosim/Entity/Specifications/NavMeshAgent", fileName = "NavMeshAgent (Spec)")]
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
        }
    }
}
