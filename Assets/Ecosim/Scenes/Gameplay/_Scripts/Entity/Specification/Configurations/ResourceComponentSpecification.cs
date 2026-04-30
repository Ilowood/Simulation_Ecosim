using UnityEngine;

namespace Ecosim
{
    [CreateAssetMenu(menuName = "Ecosim/Entity/Variants/ResourceComponent", fileName = "ResourceComponent")]
    public class ResourceComponentSpecification : Specification
    {
        [SerializeField] private int _amount = 100;

        public override void Apply(Entity entity)
        {
            entity.AddComponent(new ResourceComponent(_amount));
        }
    }
}
