using UnityEngine;

namespace BugColony
{
    [CreateAssetMenu(menuName = "Entity/Variants/Color", fileName = "ColorSpecification")]
    public class ColorSpecification : Specification
    {
        [SerializeField] private Color _color;

        public override void Apply(Entity entity)
        {
            entity.Model.SetColor(_color);
        }
    }
}
