using System;
using UnityEngine;

namespace BugColony
{
    public class AttackTask : IEntityTask
    {
        private readonly Entity _entity;
        private readonly Entity _target;

        private readonly float _attackRange;
        private readonly float _attackDuration;
        private readonly Action<Entity> _onAttack;

        private float _timer;
        private bool _isComplete = false;

        public bool IsComplete => _isComplete;
        public TaskVariants Variants => TaskVariants.Attack;

        public AttackTask(Entity entity, Entity target, float attackRange, float attackDuration, Action<Entity> onAttack)
        {
            _entity = entity;
            _target = target;
            _attackRange = attackRange;
            _attackDuration = attackDuration;
            _onAttack = onAttack;
        }

        public void Start()
        {
            _timer = 0f;
        }

        public void Tick(float deltaTime)
        {
            if (_isComplete) return;

            _timer += deltaTime;

            if (_timer >= _attackDuration || !_entity || _entity.IsDead || !_target || _target.IsDead)
            {
                EndAttacking();
            }
        }

        public void End()
        {
            if (_isComplete)
                return;

            _isComplete = true;
        }

        private void EndAttacking()
        {
            if (_isComplete) return;

            if (Vector3.Distance(_target.transform.position, _entity.transform.position) < _attackRange)
            {
                _onAttack?.Invoke(_target);
            }

            _isComplete = true; 
        }
    }
}
