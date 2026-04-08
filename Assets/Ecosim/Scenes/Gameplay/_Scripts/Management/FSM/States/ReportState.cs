using UnityEngine.SceneManagement;
using Untils;

namespace Ecosim
{
    public class ReportState : IFSMState<StateGameplay>
    {
        private readonly FSMGameplay _fsm;
        private readonly PauseView _view;

        public ReportState(FSMGameplay fsm, PauseView view)
        {
            _fsm = fsm;
            _view = view;
            
            // UIHelper.SaveArea(view.SaveArea);
            // view.Init(this);
        }

        public StateGameplay State => StateGameplay.PauseState;

        public void Enter()
        {
            _view.Open();
        }

        public void Exit()
        {
            _view.Close();
        }

        public void Restar()
        {
            
        }

        public void MenuScreen()
        {
            SceneManager.LoadScene(Scenes.Menu);
        }
    }
}
