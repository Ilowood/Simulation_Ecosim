using System.Collections.Generic;
using UnityEngine;

namespace Ecosim
{
    [CreateAssetMenu(menuName = "Entity/Specification", fileName = "EntitySpecification")]
    public class EntitySpecification : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public EntityType Type { get; private set; }

        [Space, SerializeField] public BehaviourSpecification Behaviour;
        [SerializeField] public List<Specification> Configuration;
    }
}
