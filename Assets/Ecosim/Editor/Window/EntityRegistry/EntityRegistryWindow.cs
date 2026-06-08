using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace Ecosim.Editor
{
    public class EntityRegistryWindow : EditorWindow
    {
        private const string EDITOR_RESOURCES_PATH = "Assets/Ecosim/Editor/Window/EntityRegistry";
        private const string WINDOW_UXML_PATH = EDITOR_RESOURCES_PATH + "/EntityRegistryWindow.uxml";
        private const string SPEC_UXML_PATH = EDITOR_RESOURCES_PATH + "/ListEntry.uxml";
        private const string WARNING_UXML_PATH = EDITOR_RESOURCES_PATH + "/PlayModeWarning.uxml";

        private const string REGISTRY_SEARCH_FILTER = "t:EntityDatabase";
        private const string SPEC_SEARCH_FILTER = "t:EntitySpecification";

        private VisualTreeAsset _specTemplate;
        private VisualTreeAsset _window;
        private VisualTreeAsset _playModeWarningUXML;
        
        private VisualElement _detailsContainer;
        private ScrollView _listSpecifications;
        private EntityDatabase _registry;

        private EntitySpecification _currentSelectedSpec;
        
        [MenuItem("Ecosim/Entity Registry")]
        private static void Open()
        {
            var window = GetWindow<EntityRegistryWindow>("Entity Registry");
            window.minSize = new Vector2(300, 400);
            window.maxSize = new Vector2(800, 900);
        }

        private void OnEnable()
        {
            _window = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(WINDOW_UXML_PATH);
            _specTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(SPEC_UXML_PATH);
            _playModeWarningUXML = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(WARNING_UXML_PATH);
        }

        private void CreateGUI()
        {
            if (EditorApplication.isPlaying)
            {
                ShowPlayModeWarning();
                return;
            }

            var treeUXML = _window.Instantiate();
            rootVisualElement.Add(treeUXML);

            _detailsContainer = rootVisualElement.Q<VisualElement>("DetailsContainer");
            _listSpecifications = rootVisualElement.Q<ScrollView>("ListSpecification");

            var dbField = rootVisualElement.Q<ObjectField>("DataBaseField");
            dbField.objectType = typeof(EntityDatabase);
            dbField.RegisterValueChangedCallback(RegistryChanged);

            var asset = EcosimEditorUtils.FindRegistry<EntityDatabase>(REGISTRY_SEARCH_FILTER);
            _registry = asset != null 
                ? asset 
                : EcosimEditorUtils.CreateAsset<EntityDatabase>(EntityDatabase.PATH);

            dbField.value = _registry;

            rootVisualElement.Q<Button>("SyncButton").clicked += RefreshEditorWindow;
            rootVisualElement.Q<Button>("CreateButton").clicked += CreateNewSpecification;

            RefreshEditorWindow();
        }

        private void ShowPlayModeWarning()
        {
            var warning = _playModeWarningUXML.Instantiate();
            warning.style.flexGrow = 1;

            var icon = warning.Q<Image>("WarningIcon");
            if (icon != null)
                icon.image = EditorGUIUtility.IconContent("console.warnicon").image;

            rootVisualElement.Add(warning);
        }

        private void RegistryChanged(ChangeEvent<Object> evt)
        {
            _registry = evt.newValue as EntityDatabase;
            RefreshEditorWindow();
        }

        private void RefreshEditorWindow()
        {
            if (_registry == null) return;
            
            SyncRegistryWithProject();
            RebuildListView();
        }

        private void SyncRegistryWithProject()
        {
            _registry.CleanupNullReferences();
            
            string[] guids = AssetDatabase.FindAssets(SPEC_SEARCH_FILTER);
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var spec = AssetDatabase.LoadAssetAtPath<EntitySpecification>(path);
                if (spec != null) _registry.Add(spec);
            }
        }

        private void RebuildListView()
        {
            _listSpecifications.Clear();
            foreach (var spec in _registry.Specifications)
            {
                if (spec != null) AddSpecificationEntry(spec);
            }
        }

        private void AddSpecificationEntry(EntitySpecification spec)
        {
            var entry = _specTemplate.Instantiate();
            var selectBtn = entry.Q<Button>("SelectButton");

            selectBtn.text = $"{spec.Name}";
            selectBtn.clicked += () => OnSpecSelected(spec);
            selectBtn.TrackName(spec);
            
            entry.Q<Button>("PingButton").clicked += () => EditorGUIUtility.PingObject(spec);

            _listSpecifications.Add(entry);
        }

        private void OnSpecSelected(EntitySpecification spec)
        {
            _detailsContainer.style.display = DisplayStyle.Flex;

            if (_currentSelectedSpec)
            {
                var oldCopyBtn = _detailsContainer.Q<Button>("CopyIdButton");
                oldCopyBtn.clickable = null;

                var oldDeleteBtn = _detailsContainer.Q<Button>("DeleteButton");
                oldDeleteBtn.clickable = null;
            }

            _currentSelectedSpec = spec;

            var scriptField = _detailsContainer.Q<ObjectField>("ScriptField");
            if (scriptField != null) scriptField.value = MonoScript.FromScriptableObject(_currentSelectedSpec);

            var id = _detailsContainer.Q<LongField>("Id");
            id.value = spec.Id;

            var copyBtn = _detailsContainer.Q<Button>("CopyIdButton");
            copyBtn.clicked += () => GUIUtility.systemCopyBuffer = _currentSelectedSpec.Id.ToString();

            var deleteBtn = _detailsContainer.Q<Button>("DeleteButton");
            deleteBtn.clicked += () => {
                _registry.Remove(_currentSelectedSpec.Id);
                EcosimEditorUtils.DeleteAssetFile(_currentSelectedSpec);
                _detailsContainer.style.display = DisplayStyle.None;
                RefreshEditorWindow();
            };

            _detailsContainer.Bind(new SerializedObject(_currentSelectedSpec));
        }

        private void CreateNewSpecification()
        {
            if (_registry == null) return;
            EcosimEditorUtils.CreateAsset<EntitySpecification>(EntityDatabase.PATH_SPECIFICATION);
            RefreshEditorWindow();
        }
    }
}
