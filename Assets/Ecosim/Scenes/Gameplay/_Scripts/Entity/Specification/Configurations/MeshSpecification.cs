using UnityEngine;

namespace Ecosim
{
    [CreateAssetMenu(menuName = "Entity/Variants/Mesh", fileName = "MeshSpecification")]
    public class MeshSpecification : Specification
    {
        [SerializeField] private EntityModel _entityModel;

        public override void Apply(Entity entity)
        {
            Instantiate(_entityModel, entity.transform);
        }
    }
}
