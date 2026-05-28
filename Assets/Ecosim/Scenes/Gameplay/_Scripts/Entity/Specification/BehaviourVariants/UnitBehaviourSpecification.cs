using UnityEngine;
using Zenject;

namespace Ecosim
{
    [CreateAssetMenu(menuName = "Ecosim/Entity/Behaviours/UnitBehaviour", fileName = "UnitBehaviour")]
    public class UnitBehaviourSpecification : BehaviourSpecification
    {
        public override IBehaviour Create(Entity owner, DiContainer container)
        {
            var storageSystem = container.Instantiate<StorageSystem>();
            return new UnitBehaviour(owner, storageSystem);
        }
    }
}
