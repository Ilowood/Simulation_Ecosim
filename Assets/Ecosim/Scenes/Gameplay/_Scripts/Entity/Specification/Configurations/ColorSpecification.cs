using UnityEngine;

namespace Ecosim
{
    [CreateAssetMenu(menuName = "Ecosim/Entity/Specifications/Color", fileName = "Color (Spec)")]
    public class ColorSpecification : Specification
    {
        [SerializeField] private Color _color;

        public override void Apply(Entity entity)
        {
            entity.GetComponentInChildren<EntityModel>().SetColor(_color);
        }
    }
}
