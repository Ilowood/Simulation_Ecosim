using UnityEngine;

namespace Ecosim
{
    [CreateAssetMenu(menuName = "Entity/Variants/Behaviour/UnitBehaviour", fileName = "UnitBehaviour")]
    public class UnitBehaviourSpecification : BehaviourSpecification
    {
        public override IBehaviour Create()
        {
            return new UnitBehaviour();
        }
    }
}
