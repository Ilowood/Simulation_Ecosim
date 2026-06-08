using UnityEngine;
using Zenject;

namespace Ecosim
{
    [CreateAssetMenu(menuName = "Ecosim/Entity/Behaviours/UnitBehaviour", fileName = "UnitBehaviour")]
    public class UnitBehaviourSpecification : BehaviourSpecification
    {
        [SerializeField] private EntitySpecification _specIdWarehouses;
        [SerializeField] private EntitySpecification _specIdTree;
        [SerializeField] private EntitySpecification _specIdWood;

        public override IBehaviour Create(Entity owner, DiContainer container)
        {
            var storageSystem = container.Instantiate<StorageSystem>();
            return new UnitBehaviour(owner, storageSystem, _specIdWarehouses.Id, _specIdTree.Id, _specIdWood.Id);
        }
    }
}
