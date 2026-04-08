using UnityEngine;

namespace Ecosim
{
    [CreateAssetMenu(menuName = "Entity/Variants/Behaviour/WorkerBehaviour", fileName = "WorkerBehaviour")]
    public class WorkerBehaviourSpecification : BehaviourSpecification
    {
        [SerializeField] private int _eatToSplit = 2;
        [SerializeField] private float _eatDuration = 2;
        [SerializeField] private float _distanceToEat = 2;

        public override IBehaviour Create()
        {
            return new WorkerBehaviour(_eatToSplit, _eatDuration, _distanceToEat);
        }
    }
}
