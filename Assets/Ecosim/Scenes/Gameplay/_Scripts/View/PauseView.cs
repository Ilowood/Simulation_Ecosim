using UnityEngine;
using UnityEngine.UI;

namespace Ecosim
{
    public class PauseView : Screen
    {
        [Header("Buttons")]
        [SerializeField] private Button _restart;
        [SerializeField] private Button _resume;
        [SerializeField] private Button _levelEditor;
        [SerializeField] private Button _menu;

        public void Init(PauseState state)
        {
            _restart.onClick.AddListener(() => state.Restart());
            _resume.onClick.AddListener(() => state.Resume());
            _levelEditor.onClick.AddListener(() => state.LevelEditor());
            _menu.onClick.AddListener(() => state.MenuScreen());
        }
    }
}
