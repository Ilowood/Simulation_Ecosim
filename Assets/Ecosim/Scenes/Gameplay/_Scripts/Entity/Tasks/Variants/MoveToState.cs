using UnityEngine;
using UnityEngine.AI;

namespace Ecosim
{
    public class MoveToTask : IEntityTask
    {
        private readonly NavMeshAgent _thisAgent;
        private readonly Entity _target;
        private readonly Vector3 _targetPosition;
        private readonly float _startEntitySpeed;

        private bool _isComplete = false;
        public bool IsComplete => _isComplete;
        public TaskVariants Variants => TaskVariants.Move;

        public MoveToTask(Entity entity, Entity targetEntity, Vector3 targetPosition)
        {
            _thisAgent = entity.GetComponent<NavMeshAgent>();

            _target = targetEntity;
            _targetPosition = targetPosition;
            _startEntitySpeed = _thisAgent.speed;
        }

        public void Start()
        {
            _thisAgent.isStopped = false;
            _thisAgent.SetDestination(_targetPosition);
        }

        public void Tick(float deltaTime, float scale)
        {
            if (_isComplete) return;

            _thisAgent.speed = _startEntitySpeed * scale;
            if (IsDestinationReached(_thisAgent) || !IsTargetValid())
            {
                End();
            }
        }

        public void End()
        {
            if (_isComplete)
                return;

            _thisAgent.speed = _startEntitySpeed;
            _thisAgent.isStopped = true;
            _thisAgent.ResetPath();  
            _thisAgent.velocity = Vector3.zero; 

            _isComplete = true;
        }

        public void Puase()
        {
            _thisAgent.isStopped = true;
        }

        public void Resume()
        {
            _thisAgent.isStopped = false;
        }

        private bool IsTargetValid()
        {
            return _target != null && _target.IsActive;
        }

        private bool IsDestinationReached(NavMeshAgent agent)
        {
            if (agent.pathPending)
                return false;

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    return true;
            }

            return false;
        }
    }
}
