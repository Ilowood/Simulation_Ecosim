using UnityEngine;
using UnityEngine.UI;

namespace BugColony
{
    public class PauseView : Screen
    {
        [Header("Buttons")]
        [SerializeField] private Button _resume;
        [SerializeField] private Button _menu;

        public void Init(PauseState state)
        {
            _resume.onClick.AddListener(() => state.Resume());
            _menu.onClick.AddListener(() => state.MenuScreen());
        }
    }
}
