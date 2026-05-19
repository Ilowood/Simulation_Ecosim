using UnityEngine;

namespace Ecosim
{
    [CreateAssetMenu(menuName = "Ecosim/Entity/Specifications/StorageComponent", fileName = "StorageComponent (Spec)")]
    public class StorageComponentSpecification : Specification
    {
        [SerializeField] private int _slotCount = 16;
        [SerializeField] private int _cellsPerSlot = 20;
        [SerializeField] private int _stackSize = 8;

        public override void Apply(Entity entity)
        {
            entity.AddComponent(new StorageComponent(_slotCount, _cellsPerSlot, _stackSize));
        }
    }
}
