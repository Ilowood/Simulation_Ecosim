using System.Collections.Generic;

namespace Untils
{
    [System.Serializable]
    public struct IdGeneratorSnapshot
    {
        public long LastId;
        public List<long> FreeIds;

        public IdGeneratorSnapshot(long lastId, List<long> freeIds)
        {
            LastId = lastId;
            FreeIds = new List<long>(freeIds);
        }
    }
}
