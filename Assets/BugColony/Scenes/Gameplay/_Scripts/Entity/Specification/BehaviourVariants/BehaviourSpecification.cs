using UnityEngine;

namespace BugColony
{
    public abstract class BehaviourSpecification : ScriptableObject
    {
        public abstract IBehaviour Create();
    }
}
