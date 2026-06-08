using System.Collections.Generic;
using UnityEngine;

namespace Ecosim
{
    [CreateAssetMenu(menuName = "Ecosim/Configs/Spawner", fileName = "SpawnerConfig")]
    public class SpawnerConfig : ScriptableObject
    {
        [field: SerializeField] public int MAX_PRE_FRAME { get; private set; } = 50;
        [field: SerializeField] public List<PoolConfig> PoolConfigs { get; private set; }
    }
}
