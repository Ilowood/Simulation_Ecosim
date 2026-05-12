using System;
using System.Collections.Generic;
using Untils;

namespace Ecosim
{
    [Serializable]
    public class WorldSnapshot
    {
        public IdGeneratorSnapshot IdGenerator;
        public List<EntitySnapshot> Entities = new();
    }
}
