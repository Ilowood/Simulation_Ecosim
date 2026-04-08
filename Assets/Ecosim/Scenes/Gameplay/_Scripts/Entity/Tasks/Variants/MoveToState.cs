using UnityEngine;
using UnityEngine.AI;

namespace Ecosim
{
    public class MoveToTask : IEntityTask
    {
        private readonly Entity _entity;
        private readonly Entity _targetEntity;
        private readonly Vector3 _targetPosition;
        private readonly float _startEntitySpeed;

        private bool _isComplete = false;
        public bool IsComplete => _isComplete;
        public TaskVariants Variants => TaskVariants.Move;

        public MoveToTask(Entity entity, Entity targetEntity, Vector3 targetPosition)
        {
            _entity = entity;
            _targetEntity = targetEntity;
            _targetPosition = targetPosition;
            _startEntitySpeed = _entity.Agent.speed;
        }

        public void Start()
        {
            _entity.Agent.isStopped = false;
            _entity.Agent.SetDestination(_targetPosition);
        }

        public void Tick(float deltaTime, float scale)
        {
            if (_isComplete) return;

            _entity.Agent.speed = _startEntitySpeed * scale;
            if (IsDestinationReached(_entity.Agent) || !IsTargetValid())
            {
                End();
            }
        }

        public void End()
        {
            if (_isComplete)
                return;

            _entity.Agent.speed = _startEntitySpeed;
            _entity.Agent.isStopped = true;
            _entity.Agent.ResetPath();  
            _entity.Agent.velocity = Vector3.zero; 

            _isComplete = true;
        }

        public void Puase()
        {
            _entity.Agent.isStopped = true;
        }

        public void Resume()
        {
            _entity.Agent.isStopped = false;
        }

        private bool IsTargetValid()
        {
            return _targetEntity != null && !_targetEntity.IsDead;
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
