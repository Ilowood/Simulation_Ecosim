using System;
using UnityEngine;

namespace Ecosim
{
    [Serializable]
    public class PoolConfig
    {
        [SerializeField] private EntitySpecification _spec;
        
        [field: SerializeField] public int Size { get; private set; }

        public long SpecId => _spec.Id;
    }
}
