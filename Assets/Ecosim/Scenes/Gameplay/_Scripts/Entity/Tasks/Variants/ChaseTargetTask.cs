using UnityEngine;
using UnityEngine.AI;

namespace Ecosim
{
    public class ChaseTargetTask : IEntityTask
    {
        private readonly NavMeshAgent _agent;

        private readonly Entity _target;

        private readonly float _updateInterval;
        private readonly float _startEntitySpeed;

        private float _timer;
        private bool _isComplete = false;

        public bool IsComplete => _isComplete;
        public TaskVariants Variants => TaskVariants.Chase;

        public ChaseTargetTask(Entity entity, Entity target, float updateInterval = 0.2f)
        {
            _agent = entity.GetComponent<NavMeshAgent>();
            _target = target;

            _updateInterval = updateInterval;
            _startEntitySpeed = _agent.speed;
        }

        public void Start()
        {
            _agent.isStopped = false;
            _agent.SetDestination(_target.transform.position);

            _timer = 0f;
        }

        public void Tick(float deltaTime, float scale)
        {
            if (_isComplete) return;
            _agent.speed = _startEntitySpeed * scale;

            if (!IsTargetValid())
            {
                End();
                return;
            }

            _timer += deltaTime * scale;

            if (_timer >= _updateInterval)
            {
                _agent.SetDestination(_target.transform.position);
                _timer = 0f;
            }

            if (IsReachedTarget(_agent))
            {
                End();
            }
        }

        public void End()
        {
            if (_isComplete)
                return;

            _agent.speed = _startEntitySpeed;
            _agent.isStopped = true;
            _agent.ResetPath();
            _agent.velocity = Vector3.zero;

            _isComplete = true;
        }

        public void Puase()
        {
            _agent.isStopped = true;
        }

        public void Resume()
        {
            _agent.isStopped = false;
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
