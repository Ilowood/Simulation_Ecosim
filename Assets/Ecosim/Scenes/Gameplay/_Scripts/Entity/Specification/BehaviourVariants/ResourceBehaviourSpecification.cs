using UnityEngine;

namespace Ecosim
{
    [CreateAssetMenu(menuName = "Entity/Variants/Behaviour/ResourceBehaviour", fileName = "ResourceBehaviour")]
    public class ResourceBehaviourSpecification : BehaviourSpecification
    {
        public override IBehaviour Create()
        {
            return null;
        }
    }
}
