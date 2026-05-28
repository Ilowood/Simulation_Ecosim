using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ecosim.Editor
{
    [CustomEditor(typeof(ItemDatabase))]
    public class ItemRegistryEditor : UnityEditor.Editor
    {
        private const string EDITOR_PATH = "Assets/Ecosim/Editor/Window/ItemRegistry";
        private const string INSPECTOR_UXML = EDITOR_PATH + "/ItemRegistryEditor.uxml";
        private const string ROW_UXML = EDITOR_PATH + "/ItemRowTemplate.uxml";

        private ScrollView _listContainer;
        private TextField _searchField;

        private VisualTreeAsset _inspector;
        private VisualTreeAsset _rowTemplate;

        private ItemDatabase _registry;

        private void OnEnable()
        {
            _inspector = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(INSPECTOR_UXML);
            _rowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(ROW_UXML);

            _registry = target as ItemDatabase;
        }

        public override VisualElement CreateInspectorGUI()
        {
            var root = _inspector.Instantiate();

            _searchField = root.Q<TextField>("SearchField");
            _listContainer = root.Q<ScrollView>("ListContainer");

            _searchField.RegisterValueChangedCallback(filter => FilterChanged(filter));
            FilterList(_searchField.value);

            return root;
        }

        private void FilterChanged(ChangeEvent<string> filter) => FilterList(filter.newValue);

        private void FilterList(string filter)
        {
            if (!string.IsNullOrEmpty(filter)) FilterListView(filter, WithFiltredView); 
            else FilterListView(filter, AllView);
        }

        private void FilterListView(string filter, Action<string, ItemConfig, SerializedProperty> view)
        {
            _listContainer.Clear();

            var propertyies = serializedObject.FindProperty("_configs");

            for (var i = 0; i < _registry.Configs.Count; i++)
            {
                var property = propertyies?.GetArrayElementAtIndex(i);
                view?.Invoke(filter, _registry.Configs[i], property);
            }

            if (_listContainer.childCount == 0)
            {
                var emptyLabel = new Label("No items found...");

                emptyLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
                emptyLabel.style.marginTop = 10;
                emptyLabel.style.color = Color.gray;

                _listContainer.Add(emptyLabel);
            }
        }

        private void WithFiltredView(string filter, ItemConfig config, SerializedProperty property)
        {
            if (config.SpecId.ToString().Contains(filter))
            {
                View(config, property);
            }
        }

        private void AllView(string filter, ItemConfig config, SerializedProperty property)
        {
            View(config, property);
        }

        private void View(ItemConfig config, SerializedProperty property)
        {
            var row = _rowTemplate.Instantiate();
            
            var idLabel = row.Q<Label>("IdLabel");
            idLabel.text = $"ID: {config.SpecId}";

            var propertyField = row.Q<PropertyField>("StackableField");
            if (propertyField != null && property != null)
            {
                var isStackableProp = property.FindPropertyRelative("_isStackable");
                propertyField.BindProperty(isStackableProp);
            }

            _listContainer.Add(row);
        }
    }
}
