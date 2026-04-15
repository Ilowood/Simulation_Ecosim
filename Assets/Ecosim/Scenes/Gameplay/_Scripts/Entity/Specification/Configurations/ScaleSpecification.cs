using UnityEngine;

namespace Ecosim
{
    [CreateAssetMenu(menuName = "Entity/Variants/Scale", fileName = "ScaleSpecification")]
    public class ScaleSpecification : Specification
    {
        [SerializeField] private Vector3 _scale;

        public override void Apply(Entity entity)
        {
            var model = entity.GetComponentInChildren<EntityModel>();
            
            model.SetScale(_scale);
            model.SetLocalPosition(new (0f, 0.5f * _scale.y, 0f));
        }
    }
}
