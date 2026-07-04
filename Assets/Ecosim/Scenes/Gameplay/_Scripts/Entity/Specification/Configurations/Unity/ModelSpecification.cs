using UnityEngine;

namespace Ecosim
{
    [CreateAssetMenu(menuName = "Ecosim/Entity/Specifications/Model", fileName = "Model (Spec)")]
    public class ModelSpecification : Specification
    {
        [SerializeField] private GameObject _selectableObject;

        public override void Apply(Entity entity)
        {
            Instantiate(_selectableObject, entity.transform);
        }
    }
}
