using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ecosim
{
    [Serializable]
    public class SpawnerConfig
    {
        [SerializeField] public List<PoolConfig> PoolConfigs;
    }
}
