using UnityEngine;

namespace BugColony
{
    [CreateAssetMenu(menuName = "Entity/Variants/Behaviour/PredatorBehaviour", fileName = "PredatorBehaviour")]
    public class PredatorBehaviourSpecification : BehaviourSpecification
    {
        [SerializeField] private int _eatToSplit;
        [SerializeField] private float _lifeTimeInSeconds;
        [SerializeField] private float _attackDistance;
        [SerializeField] private float _attackDuration;
        [SerializeField] private float _stunDurationTimer;
        [SerializeField] private float _speed;

        public override IBehaviour Create()
        {
            return new PredatorBehaviour(_eatToSplit, _lifeTimeInSeconds, _attackDuration, _attackDistance, _stunDurationTimer, _speed);
        }
    }
}
