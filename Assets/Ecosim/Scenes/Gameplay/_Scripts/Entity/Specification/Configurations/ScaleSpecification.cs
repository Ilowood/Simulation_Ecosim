using UnityEngine;

namespace Ecosim
{
    [CreateAssetMenu(menuName = "Entity/Variants/Scale", fileName = "ScaleSpecification")]
    public class ScaleSpecification : Specification
    {
        [SerializeField] private Vector3 _scale;

        public override void Apply(Entity entity)
        {
            entity.Model.SetScale(_scale);
            entity.Model.SetLocalPosition(new(0f, 0.5f * _scale.y, 0f));
        }
    }
}
