using UnityEngine;

namespace Ecosim
{
    [CreateAssetMenu(menuName = "Ecosim/Entity/Specifications/Navigation Component", fileName = "NavigationComponent (Spec)")]
    public class NavigationSpecification : Specification
    {
        public override void Apply(Entity entity)
        {
            entity.AddComponent(new NavigationComponent());
        }
    }
}
