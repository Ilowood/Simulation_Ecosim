using System.Collections.Generic;
using UnityEngine;

namespace Ecosim
{
    [CreateAssetMenu(menuName = "Config/SimulationConfig", fileName = "SimulationConfig")]
    public class SimulationConfig : ScriptableObject
    {
        [Header("Entity Settings")]
        [SerializeField] private List<SimulationEntityData> _entitiesSettings;
        [field: SerializeField] public EntityType TrackedLiveEntities { get; private set; }

        public IReadOnlyList<SimulationEntityData> EntitiesSettings => _entitiesSettings;
    }
}
