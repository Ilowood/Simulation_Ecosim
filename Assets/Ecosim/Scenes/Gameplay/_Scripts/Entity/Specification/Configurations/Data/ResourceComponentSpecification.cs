using UnityEngine;

namespace Ecosim
{
    [CreateAssetMenu(menuName = "Ecosim/Entity/Specifications/ResourceComponent", fileName = "ResourceComponent (Spec)")]
    public class ResourceComponentSpecification : Specification
    {
        [SerializeField] private int _amount = 100;

        public override void Apply(Entity entity)
        {
            entity.AddComponent(new ResourceComponent(_amount));
        }
    }
}
