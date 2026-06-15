using UnityEngine;

namespace Ecosim
{
    [CreateAssetMenu(menuName = "Ecosim/Entity/Specifications/Selectable Component", fileName = "SelectableComponent (Spec)")]
    public class SelectableComponentSpecification : Specification
    {
        [SerializeField] private GameObject _selectableObject;

        public override void Apply(Entity entity)
        {
            var instance = Instantiate(_selectableObject, entity.transform);
            entity.AddComponent(new SelectableComponent(instance));
        }
    }
}
