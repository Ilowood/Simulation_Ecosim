using System;

namespace Ecosim
{
    [Serializable]
    public struct SimulationEntityData
    {
        public EntityType Type;
        public int StartCount;
        public int TargetPopulation;
    }
}
