using System.Collections.Generic;
using UnityEngine;

namespace Ecosim
{
    [CreateAssetMenu(menuName = "Ecosim/Configs/Spawner", fileName = "SpawnerConfig")]
    public class SpawnerConfig : ScriptableObject
    {
        [field: SerializeField] public int MAX_PRE_FRAME { get; private set; } = 50;
        [field: SerializeField] public List<PoolConfig> PoolConfigs { get; private set; }

#if UNITY_EDITOR
        public const string PATH = "Assets/Ecosim/Data";

        public bool SyncWithDatabase(IReadOnlyList<EntitySpecification> specs)
        {
            var anyChanges = false;

            var removedCount = PoolConfigs.RemoveAll(x => !ContainsSpecWithId(specs, x.SpecId));
            if (removedCount > 0) anyChanges = true;

            foreach (var spec in specs)
            {
                var hasPool = PoolConfigs.Exists(x => x.SpecId == spec.Id);
                
                if (!hasPool)
                {
                    PoolConfigs.Add(new PoolConfig(spec, 1));
                    anyChanges = true;
                }
            }

            return anyChanges;
        }

        private bool ContainsSpecWithId(IReadOnlyList<EntitySpecification> specs, long id)
        {
            foreach (var spec in specs)
            {
                if (spec.Id == id) return true;
            }

            return false;
        }
#endif
    }
}
