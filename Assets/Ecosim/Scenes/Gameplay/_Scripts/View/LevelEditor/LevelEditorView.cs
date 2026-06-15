using UnityEngine;
using UnityEngine.UI;

namespace Ecosim
{
    public class LevelEditorView : Window
    {
        [Header("Buttons")]
        [SerializeField] private Button _save;
        [SerializeField] private Button _resetMap;
        [SerializeField] private Button _game;
        
        [Header("Build Palette")]
        [SerializeField] private GridLayoutGroup _grid;
        [SerializeField] private BuildPaletteTemplate _templatePrefab;
        
        private LevelEditorState _state;
        private BuildPaletteTemplate _selectedTemplate;

        public void Init(LevelEditorState state)
        {
            _state = state;
            _save.onClick.AddListener(() => state.SaveWorld());
            _resetMap.onClick.AddListener(() => state.ResetWorld());
            _game.onClick.AddListener(() => state.GameState());
            
            BuildPalette();
        }
        
        private void BuildPalette()
        {
            foreach (var spec in _state.Specifications)
            {
                var template = Instantiate(_templatePrefab, _grid.transform);
                template.Init(spec.Id, spec.Icon, OnEntitySelected);
            }
        }
        
        private void OnEntitySelected(BuildPaletteTemplate template)
        {
            _selectedTemplate?.SetSelected(false);

            _selectedTemplate = template;
            _selectedTemplate.SetSelected(true);
            _state.BuildTool(template.SpecId);
        }
    }
}
