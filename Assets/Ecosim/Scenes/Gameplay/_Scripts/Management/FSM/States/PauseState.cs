using UnityEngine.SceneManagement;
using Untils;

namespace Ecosim
{
    public class PauseState : IFSMState<StateGameplay>
    {
        private readonly FSMGameplay _fsm;
        private readonly PauseView _view;
        private readonly IInputDeviceProvider _input;

        public PauseState(FSMGameplay fsm, PauseView view, IInputDeviceProvider input)
        {
            _fsm = fsm;
            _view = view;
            _input = input;
            
            UIHelper.SaveArea(view.SaveArea);
            view.Init(this);

            _input.OnResumeEvent += Resume;
        }

        public StateGameplay State => StateGameplay.PauseState;

        public void Enter()
        {
            _input.OnMenuEnable();
            _view.Open();
        }

        public void Exit()
        {
            _view.Close();
        }

        public void Resume()
        {
            _fsm.ExitAndResume();
        }

        public void LevelEditor()
        {
            _fsm.EnterIn(StateGameplay.LevelEditorState);
        }

        public void Restart()
        {
            _fsm.EnterIn(StateGameplay.RestartState);
        }

        public void MenuScreen()
        {
            SceneManager.LoadScene(Scenes.Menu);
        }
    }
}
