using UnityEngine;

namespace BugColony
{
    [CreateAssetMenu(menuName = "Config/SimulationConfig", fileName = "SimulationConfig")]
    public class SimulationConfig : ScriptableObject
    {
        [field: SerializeField] public int WorkerStartCount { get; private set; }
        [field: SerializeField] public int PredatorStartCount { get; private set; }
        [field: SerializeField] public int ResourceCount { get; private set; }
    }
}
