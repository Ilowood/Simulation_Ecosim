using System;
using UnityEngine;

namespace Ecosim
{
    [Serializable]
    public class ItemConfig
    {
        [ReadOnly, SerializeField] private long _specId;
        [SerializeField] private bool _isStackable = true;

        public long SpecId => _specId;
        public bool IsStackable => _isStackable;

#if UNITY_EDITOR
        public void Setup(long specId, bool isStackable = true)
        {
            _specId = specId;
            _isStackable = isStackable;
        }
#endif
    }
}
