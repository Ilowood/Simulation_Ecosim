using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BugColony
{
    public class MoveToTask : IEntityTask
    {
        private readonly Transform _target;
        private Vector3 _lastPathPosition;

        private bool _isComplete = false;
        public bool IsComplete => _isComplete;
        public TaskVariants Variants => TaskVariants.Move;

        public MoveToTask(Transform targetTransform)
        {
            _target = targetTransform;
        }

        public void Start()
        {
            Task().Forget();
        }

        private async UniTask Task()
        {
            
        }

        public void End()
        {
            
        }
    }
}