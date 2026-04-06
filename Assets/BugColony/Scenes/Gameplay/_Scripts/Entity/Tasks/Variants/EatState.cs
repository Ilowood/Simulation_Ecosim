using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BugColony
{
    public class EatState : IEntityTask
    {
        private readonly SimulationContext _simulationContext;

        private readonly Entity _unit;
        private readonly Entity _food;
        private readonly float _eatDuration;
        private readonly Action<Entity> _onEat;

        private float _timer;

        private bool _isComplete = false;
        public bool IsComplete => _isComplete;
        public TaskVariants Variants => TaskVariants.Eat;

        public EatState(Entity unit, Entity food, float duration, Action<Entity> onEat)
        {
            _unit = unit;
            _food = food;
            _eatDuration = duration;
            _onEat = onEat;

            _timer = 0f;
        }

        public void Start()
        {
        }

        public void Tick(float deltaTime)
        {
            if (_isComplete) return;

            _timer += deltaTime;

            if (_timer >= _eatDuration)
            {
                FinishEating();
            }
        }

        public void End()
        {
            if (_isComplete) return;

            _isComplete = true;
        }

        private void FinishEating()
        {
            if (_isComplete) return;

            _onEat?.Invoke(_food);
            _isComplete = true; 
        }
    }
}
