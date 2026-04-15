using UnityEngine;

namespace Ecosim
{
    [CreateAssetMenu(menuName = "Entity/Variants/Color", fileName = "ColorSpecification")]
    public class ColorSpecification : Specification
    {
        [SerializeField] private Color _color;

        public override void Apply(Entity entity)
        {
            entity.GetComponentInChildren<EntityModel>().SetColor(_color);
        }
    }
}
