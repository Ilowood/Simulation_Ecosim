using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ecosim
{
    public class HUDView : Window
    {
        [Inject] private SelectionBuffer _buffer;

        [SerializeField] private SelectionBoxView _selectionBoxView = new();

        [Header("Buttons")]
        [SerializeField] private Button _speed;
        [SerializeField] private Button _pause;
        [SerializeField] private Button _save;

        public void Init(GameplayState state)
        {
            _speed.onClick.AddListener(() => state.ToggleSpeed());
            _pause.onClick.AddListener(() => state.PauseState());
            _save.onClick.AddListener(() => state.SaveWorld());
        }

        private void OnGUI()
        {
            _selectionBoxView?.Render(_buffer);
        }
    }
}
