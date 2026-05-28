using UnityEngine;
using Zenject;

namespace Ecosim
{
    [CreateAssetMenu(menuName = "Ecosim/Entity/Behaviours/WarehouseBehaviour", fileName = "WarehouseBehaviour")]
    public class WarehouseBehaviourSpecification : BehaviourSpecification
    {
        public override IBehaviour Create(Entity owner, DiContainer container)
        {
            return new WarehouseBehaviour(owner);
        }
    }
}
