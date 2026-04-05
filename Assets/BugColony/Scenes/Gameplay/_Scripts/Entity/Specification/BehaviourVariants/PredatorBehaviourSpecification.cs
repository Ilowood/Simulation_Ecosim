using UnityEngine;

namespace BugColony
{
    [CreateAssetMenu(menuName = "Entity/Variants/Behaviour/PredatorBehaviour", fileName = "PredatorBehaviour")]
    public class PredatorBehaviourSpecification : BehaviourSpecification
    {
        [SerializeField] private float _lifeTimeInSeconds;

        public override IBehaviour Create()
        {
            return new PredatorBehaviour(_lifeTimeInSeconds);
        }
    }
}
