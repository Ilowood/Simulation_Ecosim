using UnityEngine;

namespace Ecosim
{
    [CreateAssetMenu(menuName = "Entity/Variants/LocalPos", fileName = "LocalPosSpecification")]
    public class LocalPositionSpecification : Specification
    {
        [SerializeField] private Vector3 _offset;

        public override void Apply(Entity entity)
        {
            entity.Model.SetLocalPosition(_offset);
        }
    }
}
