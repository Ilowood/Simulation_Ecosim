using UnityEngine;

namespace Ecosim
{
    [CreateAssetMenu(menuName = "Ecosim/Entity/Behaviours/TreeBehaviour", fileName = "TreeBehaviour")]
    public class TreeBehaviourSpecification : BehaviourSpecification
    {
        public override IBehaviour Create()
        {
            return new TreeBehaviour();
        }
    }
}
