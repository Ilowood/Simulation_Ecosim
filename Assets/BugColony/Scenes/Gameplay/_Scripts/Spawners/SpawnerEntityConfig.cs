using System;
using System.Collections.Generic;
using UnityEngine;

namespace BugColony
{
    [Serializable]
    public class EntitySpawnConfig
    {
        [field: SerializeField] public EntitySpecification Specification { get; private set; }
        [field: SerializeField] public Transform Parent { get; private set; }
    }

    [Serializable]
    public class SpawnerEntityConfig
    {
        [field: SerializeField] public List<EntitySpawnConfig> EntitySpawnConfigs;
        [field: SerializeField] public int InitialPoolSize = 10;
    }
}
