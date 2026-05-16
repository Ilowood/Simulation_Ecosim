using System.Collections.Generic;
using UnityEngine;

namespace Ecosim
{
    public class ItemRegistry : ScriptableObject
    {
        [SerializeField] private List<ItemConfig> _configs = new();

        private Dictionary<long, int> _locations = new();

        public IReadOnlyList<ItemConfig> Configs => _configs;
        
        private void OnEnable()
        {
            UpdateCache();
        }

        public ItemConfig GetById(long id)
        {
            return _configs[_locations[id]];
        }

        public void UpdateCache()
        {
            _locations.Clear();
            for (int i = 0; i < _configs.Count; i++)
            {
                if (_configs[i] != null) 
                    _locations[_configs[i].SpecId] = i;
            }
        }

#if UNITY_EDITOR
        public const string PATH = "Assets/Ecosim/Data";

        public void Remove(long id)
        {
            if (!_locations.TryGetValue(id, out int index)) return;

            _configs[index] = null;
            UpdateCache();

            UnityEditor.EditorUtility.SetDirty(this);
        }

        public void Add(ItemConfig config)
        {
            if (config == null || _configs.Contains(config)) return;

            _configs.Add(config);
            UpdateCache();

            UnityEditor.EditorUtility.SetDirty(this);
        }

        public void Clear()
        {
            _configs = new();
            _locations = new();
        }
#endif
    }
}
