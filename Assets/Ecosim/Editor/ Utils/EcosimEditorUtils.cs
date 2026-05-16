using System.IO;
using UnityEditor;
using UnityEngine;

namespace Ecosim.Editor
{
    public static class EcosimEditorUtils
    {
        public static T FindRegistry<T>(string searchFilter) where T : ScriptableObject
        {
            var guids = AssetDatabase.FindAssets(searchFilter);

            if (guids.Length > 0)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                return AssetDatabase.LoadAssetAtPath<T>(path);
            }

            return null;
        }

        public static T CreateAsset<T>(string folderPath, string customFileName = null) where T : ScriptableObject
        {
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), folderPath));
                AssetDatabase.Refresh();
            }

            var fileName = !string.IsNullOrEmpty(customFileName) ? customFileName : $"New{typeof(T).Name}.asset";

            var combinedPath = $"{folderPath}/{fileName}";
            var uniquePath = AssetDatabase.GenerateUniqueAssetPath(combinedPath);

            var asset = ScriptableObject.CreateInstance<T>();

            AssetDatabase.CreateAsset(asset, uniquePath);
            AssetDatabase.SaveAssets();

            var cleanName = Path.GetFileNameWithoutExtension(uniquePath);
            Debug.Log($"<b>[EcosimEditor]</b> Created asset <color=cyan>{cleanName}</color> of type <b>{typeof(T).Name}</b>");

            return asset;
        }

        public static void DeleteAssetFile(ScriptableObject asset)
        {
            if (asset == null) return;

            string assetPath = AssetDatabase.GetAssetPath(asset);
            if (!string.IsNullOrEmpty(assetPath))
            {
                Debug.Log($"<color=red>Ecosim:</color> Asset <b>{asset.name}</b> (Type: {asset.GetType().Name}) was deleted instantly.");
                AssetDatabase.DeleteAsset(assetPath);
                AssetDatabase.SaveAssets();
            }
        }
    }
}

