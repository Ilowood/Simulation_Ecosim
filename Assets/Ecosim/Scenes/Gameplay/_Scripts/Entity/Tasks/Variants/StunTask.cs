using UnityEngine;

namespace Ecosim
{
    public class StunTask : IEntityTask
    {
        private readonly Entity _entity;
        private readonly float _duration;

        private float _elapsed;
        private bool _isComplete = false;

        public bool IsComplete => _isComplete;
        public TaskVariants Variants => TaskVariants.Stun;

        public StunTask(Entity entity, float duration)
        {
            _entity = entity;
            _duration = duration;
        }

        public void Start()
        {
            _elapsed = 0f;
        }

        public void Tick(float deltaTime, float scale)
        {
            if (_isComplete) return;

            _elapsed += deltaTime * scale;

            if (_elapsed >= _duration || !_entity || _entity.IsDead)
            {
                End();
            }
        }

        public void End()
        {
            if (_isComplete)
                return;

            _isComplete = true;
        }

        public void Puase()
        {
            
        }

        public void Resume()
        {
            
        }
    }
}
