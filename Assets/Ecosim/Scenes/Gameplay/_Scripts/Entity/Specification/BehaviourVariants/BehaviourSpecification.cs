using UnityEngine;

namespace Ecosim
{
    public abstract class BehaviourSpecification : ScriptableObject
    {
        public abstract IBehaviour Create();
    }
}
