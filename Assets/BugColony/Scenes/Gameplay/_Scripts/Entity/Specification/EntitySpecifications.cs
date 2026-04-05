using UnityEngine;
using System.Collections.Generic;

namespace BugColony
{
    [CreateAssetMenu(menuName = "Entity/AllSpecification", fileName = "AllSpecification")]
    public class EntitySpecifications : ScriptableObject
    {
        [field: SerializeField] public List<EntitySpecification> Specifications { get; private set; }
    }
}
