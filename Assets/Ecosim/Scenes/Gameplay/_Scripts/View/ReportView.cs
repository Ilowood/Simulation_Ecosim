using UnityEngine;
using UnityEngine.UI;

namespace Ecosim
{
    public class ReportView : Screen
    {
        [Header("Buttons")]
        [SerializeField] private Button _restart;
        [SerializeField] private Button _menu;

        public void Init(ReportState state)
        {
            _restart.onClick.AddListener(() => state.Restar());
            _menu.onClick.AddListener(() => state.MenuScreen());
        }
    }
}
