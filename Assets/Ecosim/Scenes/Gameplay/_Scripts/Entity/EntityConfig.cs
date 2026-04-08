using System;
using UnityEngine;

namespace Ecosim
{
    [Serializable]
    public class EntityConfig
    {
        [field: SerializeField] public EntitySpecification Specification { get; private set; }
        [field: SerializeField] public Transform Parent { get; private set; }
    }
}
