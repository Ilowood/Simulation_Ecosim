using System;
using UnityEngine;

namespace Ecosim
{
    [Serializable]
    public class PoolConfig
    {
        [field: SerializeField] public EntitySpecification Specification { get; private set; }
        [field: SerializeField] public Transform Parent { get; private set; }
        [field: SerializeField] public int Size { get; private set; }
    }
}
