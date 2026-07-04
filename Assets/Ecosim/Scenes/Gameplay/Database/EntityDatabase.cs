using System.Collections.Generic;
using UnityEngine;

namespace Ecosim
{
    public class EntityDatabase : ScriptableObject
    {
        [HideInInspector, SerializeField] private List<EntitySpecification> _specifications = new();
        private Dictionary<long, int> _locations = new();

        public IReadOnlyList<EntitySpecification> Specifications => _specifications;
        
        private void OnEnable()
        {
            UpdateCache();
        }

        public EntitySpecification GetById(long id)
        {
            return _specifications[_locations[id]];
        }

        public void UpdateCache()
        {
            _locations.Clear();
            for (int i = 0; i < _specifications.Count; i++)
            {
                if (_specifications[i] != null) 
                    _locations[_specifications[i].Id] = i;
            }
        }

#if UNITY_EDITOR
        public const string PATH = "Assets/Ecosim/Data";
        public const string PATH_SPECIFICATION = "Assets/Ecosim/Data/Entities";

        public bool HasSpecification(long id) => _locations.ContainsKey(id);

        public void Remove(long id)
        {
            if (!_locations.TryGetValue(id, out int index)) return;

            _specifications.RemoveAt(index);

            UpdateCache();
            Save(this);
        }

        public void Add(EntitySpecification specification)
        {
            if (specification == null || _specifications.Contains(specification)) return;

            var isIdTaken = false;
            foreach (var spec in _specifications)
            {
                if (spec != null && spec.Id == specification.Id)
                {
                    isIdTaken = true;
                    break;
                }
            }

            if (isIdTaken)
            {
                specification.GenerateUniqueId(); 
                Save(specification);
            }

            _specifications.Add(specification);

            UpdateCache();
            Save(this);
        }

        public void CleanupNullReferences()
        {
            _specifications.RemoveAll(spec => spec == null);

            UpdateCache();
            Save(this);
        }

        private void Save(Object @object)
        {
            UnityEditor.EditorUtility.SetDirty(@object);
            UnityEditor.AssetDatabase.SaveAssetIfDirty(@object);
        }
#endif
    }
}
