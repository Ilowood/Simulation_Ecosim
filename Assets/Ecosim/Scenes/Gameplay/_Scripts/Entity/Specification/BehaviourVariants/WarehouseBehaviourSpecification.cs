using UnityEngine;

namespace Ecosim
{
    [CreateAssetMenu(menuName = "Ecosim/Entity/Behaviours/WarehouseBehaviour", fileName = "WarehouseBehaviour")]
    public class WarehouseBehaviourSpecification : BehaviourSpecification
    {
        public override IBehaviour Create()
        {
            return new WarehouseBehaviour();
        }
    }
}
