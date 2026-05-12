using System.Collections.Generic;
using UnityEngine;

namespace Ecosim
{
    public class EntityRegistry : ScriptableObject
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
        public bool HasSpecification(long id) => _locations.ContainsKey(id);

        public EntitySpecification CreateNewSpecificationAsset()
        {
            var folderPath = "Assets/Ecosim/Data/Entities";
            
            if (!UnityEditor.AssetDatabase.IsValidFolder(folderPath))
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Application.dataPath, "Ecosim/Data/Entities"));
                UnityEditor.AssetDatabase.Refresh();
            }

            var fileName = "NewEntitySpecification.asset";
            var fullPath = UnityEditor.AssetDatabase.GenerateUniqueAssetPath($"{folderPath}/{fileName}");

            var specification = CreateInstance<EntitySpecification>();
            UnityEditor.AssetDatabase.CreateAsset(specification, fullPath);
            UnityEditor.AssetDatabase.SaveAssets();

            Debug.Log($"<b>Ecosim:</b> Created <color=cyan>{specification.name}</color>");
            return specification;
        }

        public void DeleteAssetFile(EntitySpecification specification)
        {
            if (specification == null) return;

            var assetPath = UnityEditor.AssetDatabase.GetAssetPath(specification);
            if (!string.IsNullOrEmpty(assetPath))
            {
                Debug.Log($"<color=red>Ecosim:</color> Specification <b>{specification.name}</b> was deleted instantly.");
                UnityEditor.AssetDatabase.DeleteAsset(assetPath);
            }
        }

        public void Remove(long id)
        {
            if (!_locations.TryGetValue(id, out int index)) return;

            _specifications.RemoveAt(index);
            _locations.Remove(id);

            UpdateCache();
            UnityEditor.EditorUtility.SetDirty(this);
        }

        public bool Add(EntitySpecification specification)
        {
            if (specification == null || _specifications.Contains(specification)) return false;

            _locations[specification.Id] = _specifications.Count;
            _specifications.Add(specification);

            UnityEditor.EditorUtility.SetDirty(this);
            return true;
        }

        public void CleanupNullReferences()
        {
            _specifications.RemoveAll(spec => spec == null);
            UpdateCache();
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}
