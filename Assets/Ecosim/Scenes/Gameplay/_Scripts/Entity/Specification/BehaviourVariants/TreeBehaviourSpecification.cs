using UnityEngine;
using Zenject;

namespace Ecosim
{
    [CreateAssetMenu(menuName = "Ecosim/Entity/Behaviours/TreeBehaviour", fileName = "TreeBehaviour")]
    public class TreeBehaviourSpecification : BehaviourSpecification
    {
        public override IBehaviour Create(Entity owner, DiContainer container)
        {
            return new TreeBehaviour(owner);
        }
    }
}
