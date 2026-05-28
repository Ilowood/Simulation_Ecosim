using UnityEngine;
using Zenject;

namespace Ecosim
{
    public abstract class BehaviourSpecification : ScriptableObject
    {
        public abstract IBehaviour Create(Entity owner, DiContainer container);
    }
}
