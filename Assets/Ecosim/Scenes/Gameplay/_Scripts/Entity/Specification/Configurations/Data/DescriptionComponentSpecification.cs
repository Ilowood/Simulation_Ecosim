using UnityEngine;

namespace Ecosim
{
    [CreateAssetMenu(menuName = "Ecosim/Entity/Specifications/Description Component", fileName = "DescriptionComponent (Spec)")]
    public class DescriptionComponentSpecification : Specification
    {
        [SerializeField] private string _name;
        [SerializeField] private string _description;

        public override void Apply(Entity entity)
        {
            entity.AddComponent(new DescriptionComponent(_name, _description));
        }
    }
}
