using UnityEngine;

namespace Ecosim
{
    public class NavigationComponent : IEntityComponent
    {
        public Vector3 Destination;

        public void Reset()
        {
            Destination = default;
        }

        public IComponentSnapshot GetSnapshot()
        {
            return new NavigationSnapshot(Destination);
        }

        public void Restore(IComponentSnapshot snapshot)
        {
            if (snapshot is NavigationSnapshot data)
            {
                Destination = data.Destination;
            }
        }
    }
}
