using UnityEngine;

namespace Ecosim
{
    public class SelectableComponent : IEntityComponent
    {
        public bool IsSelected;
        public readonly GameObject SelectableObject;

        public SelectableComponent(GameObject selectableObject)
        {
            SelectableObject = selectableObject;
        }

        public void Reset()
        {
            IsSelected = false;
            SelectableObject.SetActive(false);
        }

        public IComponentSnapshot GetSnapshot()
        {
            return new SelectableSnapshot(IsSelected);
        }

        public void Restore(IComponentSnapshot snapshot)
        {
            if (snapshot is SelectableSnapshot data)
            {
                IsSelected = data.IsSelected;
            }
        }
    }
}
