using UnityEditor;
using UnityEngine;

namespace Ecosim.Editor
{
    public class ItemRegistryWindow : EditorWindow 
    {
        private const string ITEM_REGISTRY_SEARCH_FILTER = "t:ItemDatabase";
        private const string ENTITY_REGISTRY_SEARCH_FILTER = "t:EntityDatabase";

        [MenuItem("Ecosim/Item Registry")]
        private static void ShowEditor()
        {
            var itemAsset = EcosimEditorUtils.FindRegistry<ItemDatabase>(ITEM_REGISTRY_SEARCH_FILTER);
            var itemRegistry = itemAsset != null 
                ? itemAsset 
                : EcosimEditorUtils.CreateAsset<ItemDatabase>(ItemDatabase.PATH);

            var entityAsset = EcosimEditorUtils.FindRegistry<EntityDatabase>(ENTITY_REGISTRY_SEARCH_FILTER);
            var entityRegistry = entityAsset != null 
                ? entityAsset 
                : EcosimEditorUtils.CreateAsset<EntityDatabase>(EntityDatabase.PATH);

            AutoItemRegistry(itemRegistry, entityRegistry);
            Selection.activeObject = itemRegistry;
            EditorGUIUtility.PingObject(itemRegistry);
        }

        private static void AutoItemRegistry(ItemDatabase itemRegistry, EntityDatabase entityRegistry) 
        {
            itemRegistry.Clear();

            foreach (var spec in entityRegistry.Specifications)
            {
                if (spec.Type == EntityType.Resource)
                {
                    var config = new ItemConfig();
                    config.Setup(spec.SpecId);

                    itemRegistry.Add(config);
                }
            }

            EditorUtility.SetDirty(itemRegistry);
            AssetDatabase.SaveAssets();
        }
    }
}
