using System;
using UnityEngine;

namespace Ecosim
{
    [Serializable]
    public struct MoveToPointParams : ITaskParams
    {
        public TaskVariants Variants => TaskVariants.Move;

        public Vector3 Destination;
    }
}
