using UnityEngine;

namespace BugColony
{
    [CreateAssetMenu(menuName = "Entity/Variants/Behaviour/WorkerBehaviour", fileName = "WorkerBehaviour")]
    public class WorkerBehaviourSpecification : BehaviourSpecification
    {
        [SerializeField] private float _speed = 5;
        [SerializeField] private int _eatToSplit = 2;

        public override IBehaviour Create()
        {
            return new WorkerBehaviour(_speed, _eatToSplit);
        }
    }
}
