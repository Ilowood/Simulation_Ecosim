using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ecosim
{
    [Serializable]
    public class EntitySnapshot
    {
        public long SpecId;
        public long InstanceId;

        public Vector3 Position;
        public Quaternion Rotation;

        public ITaskSnapshot Task;
        public List<IComponentSnapshot> Components = new();
    }
}
