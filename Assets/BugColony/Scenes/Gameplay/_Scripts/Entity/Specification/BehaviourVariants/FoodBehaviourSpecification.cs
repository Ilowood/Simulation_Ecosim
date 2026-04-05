using UnityEngine;

namespace BugColony
{
    [CreateAssetMenu(menuName = "Entity/Variants/Behaviour/FoodBehaviour", fileName = "FoodBehaviour")]
    public class FoodBehaviourSpecification : BehaviourSpecification
    {
        public override IBehaviour Create()
        {
            return null;
        }
    }
}
