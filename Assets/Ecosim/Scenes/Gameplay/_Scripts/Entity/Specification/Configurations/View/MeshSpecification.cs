using UnityEngine;

namespace Ecosim
{
    [CreateAssetMenu(menuName = "Ecosim/Entity/Specifications/Mesh", fileName = "Mesh (Spec)")]
    public class MeshSpecification : Specification
    {
        [SerializeField] private EntityModel _entityModel;

        public override void Apply(Entity entity)
        {
            Instantiate(_entityModel, entity.transform);
        }
    }
}
