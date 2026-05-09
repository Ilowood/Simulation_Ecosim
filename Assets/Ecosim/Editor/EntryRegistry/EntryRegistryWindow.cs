using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace Ecosim
{
    public class EntryRegistry : EditorWindow
    {
        private const string EDITOR_RESOURCES_PATH = "Assets/Ecosim/Editor/EntryRegistry";
        private const string WINDOW_UXML_PATH = EDITOR_RESOURCES_PATH + "/EntityRegistryWindow.uxml";
        private const string LIST_ENTRY_UXML_PATH = EDITOR_RESOURCES_PATH + "/ListEntry.uxml";
        private const string WARNING_UXML_PATH = EDITOR_RESOURCES_PATH + "/PlayModeWarning.uxml";

        private const string REGISTRY_SEARCH_FILTER = "t:EntityRegistry";
        private const string SPEC_SEARCH_FILTER = "t:EntitySpecification";

        [SerializeField] private VisualTreeAsset _listEntryTemplate;
        [SerializeField] private VisualTreeAsset _visualTreeUXML;
        [SerializeField] private VisualTreeAsset _playModeWarningUXML;
        
        private VisualElement _detailsContainer;
        private ScrollView _listSpecifications;
        private EntityRegistry _registry;
        
        [MenuItem("Ecosim/Entity Registry")]
        private static void Open()
        {
            var window = GetWindow<EntryRegistry>("Entity Registry");
            window.minSize = new Vector2(300, 400);
            window.maxSize = new Vector2(800, 900);
        }

        private void OnEnable()
        {
            _visualTreeUXML = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(WINDOW_UXML_PATH);
            _listEntryTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(LIST_ENTRY_UXML_PATH);
            _playModeWarningUXML = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(WARNING_UXML_PATH);
        }

        private void CreateGUI()
        {
            if (EditorApplication.isPlaying)
            {
                ShowPlayModeWarning();
                return;
            }

            var treeUXML = _visualTreeUXML.Instantiate();
            rootVisualElement.Add(treeUXML);

            _detailsContainer = rootVisualElement.Q<VisualElement>("DetailsContainer");
            _listSpecifications = rootVisualElement.Q<ScrollView>("ListSpecification");

            var dbField = rootVisualElement.Q<ObjectField>("DataBaseField");
            dbField.objectType = typeof(EntityRegistry);
            dbField.RegisterValueChangedCallback(RegistryChanged);

            _registry = FindRegistry();
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

        private EntityRegistry FindRegistry()
        {
            string[] guids = AssetDatabase.FindAssets(REGISTRY_SEARCH_FILTER);
            
            if (guids.Length > 0)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                return AssetDatabase.LoadAssetAtPath<EntityRegistry>(path);
            }
            return null;
        }

        private void RegistryChanged(ChangeEvent<Object> evt)
        {
            _registry = evt.newValue as EntityRegistry;
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
            var entry = _listEntryTemplate.Instantiate();
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

            var scriptField = _detailsContainer.Q<ObjectField>("ScriptField");
            if (scriptField != null) scriptField.value = MonoScript.FromScriptableObject(spec);

            var copyBtn = _detailsContainer.Q<Button>("CopyIdButton");
            copyBtn.clicked += () => GUIUtility.systemCopyBuffer = spec.Id.ToString();

            var deleteBtn = _detailsContainer.Q<Button>("DeleteButton");
            deleteBtn.clicked += () => {
                _registry.DeleteAssetFile(spec);
                _registry.Remove(spec.Id);
                _detailsContainer.style.display = DisplayStyle.None;
                RefreshEditorWindow();
            };

            _detailsContainer.Bind(new SerializedObject(spec));
        }

        private void CreateNewSpecification()
        {
            if (_registry == null) return;
            var spec = _registry.CreateNewSpecificationAsset();
            Selection.activeObject = spec;
            RefreshEditorWindow();
        }
    }
}
