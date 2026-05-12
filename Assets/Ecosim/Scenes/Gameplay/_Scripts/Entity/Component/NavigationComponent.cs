using UnityEngine;

namespace Ecosim
{
    public class NavigationComponent : IEntityComponent
    {
        public Vector3 Destination { get; set; }

        public void Reset()
        {
            Destination = default;
        }

        public IComponentSnapshot GetSnapshot()
        {
            return new NavMeshSnapshot(Destination);
        }

        public void Restore(IComponentSnapshot snapshot)
        {
            if (snapshot is NavMeshSnapshot data)
            {
                Destination = data.Destination;
            }
        }
    }
}
