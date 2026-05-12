using UnityEngine;

namespace Ecosim
{
    [CreateAssetMenu(menuName = "Ecosim/Entity/Specifications/StorageComponent", fileName = "StorageComponent (Spec)")]
    public class StorageComponentSpecification : Specification
    {
        [SerializeField] private int _slotsCount = 16;
        [SerializeField] private int _slotCapacity = 20;

        public override void Apply(Entity entity)
        {
            entity.AddComponent(new StorageComponent(_slotsCount, _slotCapacity));
        }
    }
}
