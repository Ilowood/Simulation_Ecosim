using UnityEngine;

namespace Ecosim
{
    [CreateAssetMenu(menuName = "Ecosim/Entity/Behaviours/UnitBehaviour", fileName = "UnitBehaviour")]
    public class UnitBehaviourSpecification : BehaviourSpecification
    {
        public override IBehaviour Create()
        {
            return new UnitBehaviour();
        }
    }
}
