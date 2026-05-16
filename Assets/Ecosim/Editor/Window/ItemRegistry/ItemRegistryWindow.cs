using UnityEditor;
using UnityEngine;

namespace Ecosim.Editor
{
    public class ItemRegistryWindow : EditorWindow 
    {
        private const string ITEM_REGISTRY_SEARCH_FILTER = "t:ItemRegistry";
        private const string ENTITY_REGISTRY_SEARCH_FILTER = "t:EntityRegistry";

        [MenuItem("Ecosim/Item Registry")]
        private static void ShowEditor()
        {
            var itemAsset = EcosimEditorUtils.FindRegistry<ItemRegistry>(ITEM_REGISTRY_SEARCH_FILTER);
            var itemRegistry = itemAsset != null 
                ? itemAsset 
                : EcosimEditorUtils.CreateAsset<ItemRegistry>(ItemRegistry.PATH);

            var entityAsset = EcosimEditorUtils.FindRegistry<EntityRegistry>(ENTITY_REGISTRY_SEARCH_FILTER);
            var entityRegistry = entityAsset != null 
                ? entityAsset 
                : EcosimEditorUtils.CreateAsset<EntityRegistry>(EntityRegistry.PATH);

            AutoPopulateRegistry(itemRegistry, entityRegistry);
            Selection.activeObject = itemRegistry;
            EditorGUIUtility.PingObject(itemRegistry);
        }

        private static void AutoPopulateRegistry(ItemRegistry itemRegistry, EntityRegistry entityRegistry) 
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
