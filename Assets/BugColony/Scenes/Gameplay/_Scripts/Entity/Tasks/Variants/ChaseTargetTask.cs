using UnityEngine;
using UnityEngine.AI;

namespace BugColony
{
    public class ChaseTargetTask : IEntityTask
    {
        private readonly Entity _entity;
        private readonly Entity _target;
        private readonly float _updateInterval;
        private readonly float _startEntitySpeed;

        private float _timer;
        private bool _isComplete = false;

        public bool IsComplete => _isComplete;
        public TaskVariants Variants => TaskVariants.Chase;

        public ChaseTargetTask(Entity entity, Entity target, float updateInterval = 0.2f)
        {
            _entity = entity;
            _target = target;
            _updateInterval = updateInterval;
            _startEntitySpeed = _entity.Agent.speed;
        }

        public void Start()
        {
            _entity.Agent.isStopped = false;
            _entity.Agent.SetDestination(_target.transform.position);

            _timer = 0f;
        }

        public void Tick(float deltaTime, float scale)
        {
            if (_isComplete) return;
            _entity.Agent.speed = _startEntitySpeed * scale;

            if (!IsTargetValid())
            {
                End();
                return;
            }

            _timer += deltaTime * scale;

            if (_timer >= _updateInterval)
            {
                _entity.Agent.SetDestination(_target.transform.position);
                _timer = 0f;
            }

            if (IsReachedTarget(_entity.Agent))
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
            return _target != null && !_target.IsDead;
        }

        private bool IsReachedTarget(NavMeshAgent agent)
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
