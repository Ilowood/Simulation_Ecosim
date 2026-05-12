using System;
using UnityEngine;

namespace Ecosim
{
    public class NavMeshSnapshot : IComponentSnapshot
    {
        public readonly Vector3 Destination;

        public Type ComponentType => typeof(NavigationComponent);

        public NavMeshSnapshot(Vector3 destination)
        {
            Destination = destination;
        }
    }
}
