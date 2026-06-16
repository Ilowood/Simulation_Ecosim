using UnityEngine;

namespace Ecosim
{
    public class MoveCommandProvider : ICommandProvider
    {
        private readonly TaskFactory _factory;

        public MoveCommandProvider(TaskFactory factory)
        {
            _factory = factory;
        }

        public int Priority => 1;

        public bool CanExecute(Entity entity, RaycastHit hit)
        {
            var hitEntity = hit.collider.GetComponentInParent<Entity>();
            return !hitEntity && entity.IsActive;
        }

        public void Create(Entity targetEntity, RaycastHit hit)
        {
            var originalPosition = hit.point;
            var targetPosition = new Vector3(originalPosition.x, 0f, originalPosition.z);

            targetEntity.Behavior.SetAndStartTask(new MoveToPointTask(targetEntity, targetPosition));
        }
    }
}
