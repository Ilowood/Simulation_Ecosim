using System;

namespace Ecosim
{
    public class SelectableSnapshot : IComponentSnapshot
    {
        public readonly bool IsSelected;

        public Type ComponentType => typeof(SelectableComponent);

        public SelectableSnapshot(bool isSelected)
        {
            IsSelected = isSelected;
        }
    }
}
