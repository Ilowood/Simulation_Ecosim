using UnityEngine;
using UnityEngine.AI;

namespace Ecosim
{
    public class MoveToPointTask : IEntityTask
    {
        private NavigationComponent _component;
        private NavMeshAgent _agent;

        private Vector3 _destination;
        private float _startSpeed;
        
        private bool _isComplete = false;
        public bool IsComplete => _isComplete;

        public TaskVariants Variants => TaskVariants.Move;

        public MoveToPointTask(Entity entity, Vector3 targetPosition)
        {
            _component = entity.Get<NavigationComponent>();
            _agent = entity.GetComponent<NavMeshAgent>();

            _destination = targetPosition;
            _startSpeed = _agent.speed;
        }

        public void Puase() => _agent.isStopped = true;
        public void Resume() => _agent.isStopped = false;

        public void Start()
        {
            if (_agent == null) { _isComplete = true; return; }

            _agent.isStopped = false;

            _component.Destination = _destination;
            _agent.SetDestination(_destination);
        }

        public void Tick(float deltaTime, float scale)
        {
            if (_isComplete) return;

            _agent.speed = _startSpeed * scale;

            if (IsReached()) End();
        }

        public void End()
        {
            if (_isComplete) return;

            _agent.speed = _startSpeed;
            if (_agent.isOnNavMesh)
            {
                _agent.isStopped = true;
                _agent.ResetPath();
            }
            
            _isComplete = true;
        }

        public ITaskSnapshot GetSnapshot()
        {
            var snapshot = new MoveToPointSnapshot(_destination);
            return snapshot;
        }

        // public void Restore(Entity root, ITaskSnapshot snapshot)
        // {
        //     _component = root.Get<NavigationComponent>();
        //     _agent = root.GetComponent<NavMeshAgent>();

        //     _destination = _component.Destination;
        //     _startSpeed = _agent.speed;
        // }

        private bool IsReached()
        {
            if (_agent.pathPending) return false;

            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                if (!_agent.hasPath || _agent.velocity.sqrMagnitude < 0.01f)
                    return true;
            }

            return false;
        }
    }
}
