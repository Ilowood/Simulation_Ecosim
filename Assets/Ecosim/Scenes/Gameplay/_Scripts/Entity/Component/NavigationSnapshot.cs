using System;
using UnityEngine;

namespace Ecosim
{
    public class NavigationSnapshot : IComponentSnapshot
    {
        public readonly Vector3 Destination;

        public Type ComponentType => typeof(NavigationComponent);

        public NavigationSnapshot(Vector3 destination)
        {
            Destination = destination;
        }
    }
}
