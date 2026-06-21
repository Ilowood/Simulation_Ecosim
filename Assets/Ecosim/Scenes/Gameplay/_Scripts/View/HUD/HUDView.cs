using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ecosim
{
    public class HUDView : Window, ITicable
    {
        [Inject] private SelectionBuffer _buffer;

        [SerializeField] private TooltipView _tooltip;
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

        public void Tick(float deltaTime, float scale)
        {
            _tooltip.Tick(deltaTime, scale);
        }

        private void OnGUI()
        {
            _selectionBoxView?.Render(_buffer);
        }
    }
}
