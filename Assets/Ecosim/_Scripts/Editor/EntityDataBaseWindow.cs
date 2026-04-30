using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace Ecosim
{
    public class EntityDataBaseWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset _listEntryTemplate;
        [SerializeField] private VisualTreeAsset _visualTreeUXML;
        [SerializeField] private VisualTreeAsset _footerTemplate;
        [SerializeField] private VisualTreeAsset _playModeWarningUXML;
        
        private VisualElement _detailsContainer;
        private ScrollView _listSpecifications;

        private EntityDataBase _dataBase;
        
        [MenuItem("Ecosim/Entity DataBase")]
        private static void Open()
        {
            var window = GetWindow<EntityDataBaseWindow>("Entity BD");
            window.minSize = new Vector2(300, 400);
            window.maxSize = new Vector2(800, 900);
        }

        private void OnEnable()
        {
            _visualTreeUXML = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Ecosim/_Scripts/Editor/EntityDataBaseWindow.uxml");
            _listEntryTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Ecosim/_Scripts/Editor/ListEntry.uxml");
            _footerTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Ecosim/_Scripts/Editor/SpecificationFooter.uxml");
            _playModeWarningUXML = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Ecosim/_Scripts/Editor/PlayModeWarning.uxml");
        }

        private void CreateGUI()
        {
            if (!EditorApplication.isPlaying)
            {
                var treeUXML = _visualTreeUXML.Instantiate();
                rootVisualElement.Add(treeUXML);

                _detailsContainer = rootVisualElement.Q<VisualElement>("DetailsContainer");
                _listSpecifications = rootVisualElement.Q<ScrollView>("ListSpecification");

                var dbField = rootVisualElement.Q<ObjectField>("DataBaseField");
                dbField.objectType = typeof(EntityDataBase);
                dbField.RegisterValueChangedCallback(x => DatabaseForWindowChanged(x));

                _dataBase = FindDatabase();
                dbField.value = _dataBase;

                var scanSpecificaton = rootVisualElement.Q<Button>("SyncButton");
                scanSpecificaton.clicked += () => RefreshEditorWindow();

                var createSpecification = rootVisualElement.Q<Button>("CreateButton");
                createSpecification.clicked += () => CreateNewSpecification();

                RefreshEditorWindow();
            }
            else
            {
                ShowPlayModeWarning();
            }
        }

        private void ShowPlayModeWarning()
        {
            var warning = _playModeWarningUXML.Instantiate();
            warning.style.flexGrow = 1;

            var icon = warning.Q<Image>("WarningIcon");
            if (icon != null)
            {
                icon.image = EditorGUIUtility.IconContent("console.warnicon").image;
            }

            rootVisualElement.Add(warning);
        }

        private EntityDataBase FindDatabase()
        {
            string[] guids = AssetDatabase.FindAssets("t:EntityDataBase");
            
            if (guids.Length > 0)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                return AssetDatabase.LoadAssetAtPath<EntityDataBase>(path);
            }

            return null;
        }

        private void RefreshEditorWindow()
        {
            SyncDatabaseWithProjectFiles();
            RebuildSpecificationsView();
        }

        private void CreateNewSpecification()
        {
            var specification = _dataBase.CreateNewSpecificationAsset();
            _dataBase.Add(specification);

            Selection.activeObject = specification;
            RefreshEditorWindow();
        }

        private void SyncDatabaseWithProjectFiles()
        {
            _dataBase.CleanupNullReferences();
            
            string[] guids = AssetDatabase.FindAssets("t:EntitySpecification");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var spec = AssetDatabase.LoadAssetAtPath<EntitySpecification>(path);

                if (spec != null) _dataBase.Add(spec);
            }
        }

        private void RebuildSpecificationsView()
        {
            _listSpecifications.Clear();

            foreach (var spec in _dataBase.Specifications)
            {
                if (spec != null)
                {
                    AddSpecificationView(spec);
                }
            }
        }

        private void DatabaseForWindowChanged(ChangeEvent<Object> @object)
        {
            _dataBase = (EntityDataBase)@object.newValue;
        }

        private void AddSpecificationView(EntitySpecification specification)
        {
            var entry = _listEntryTemplate.Instantiate();

            var selectBtn = entry.Q<Button>("SelectButton");
            var pingBtn = entry.Q<Button>("PingButton");

            selectBtn.text = specification.Name;
            selectBtn.clicked += () => OnSpecSelected(specification);
            pingBtn.clicked += () => EditorGUIUtility.PingObject(specification);

            selectBtn.TrackName(specification);
            
            _listSpecifications.Add(entry);
        }

        private void OnSpecSelected(EntitySpecification spec)
        {
            _detailsContainer.Clear();

            var inspector = new InspectorElement(spec);
            _detailsContainer.Add(inspector);

            var footer = _footerTemplate.Instantiate();
            footer.Q<Button>("DeleteButton")?.RegisterCallback<ClickEvent>(evt => 
            {
                _dataBase.Remove(spec.Id);
                _dataBase.DeleteAssetFile(spec);

                _detailsContainer.Clear();
                RefreshEditorWindow();
            });

            _detailsContainer.Add(footer);
        }
    }
}
