using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ecosim
{
    [Serializable]
    public class SpawnerConfig
    {
        [field: SerializeField] public List<EntityConfig> EntitySpawnConfigs;
        [field: SerializeField] public int InitialPoolSize = 10;
    }
}
