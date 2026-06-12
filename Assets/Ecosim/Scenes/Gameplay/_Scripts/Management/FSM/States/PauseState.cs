using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using Untils;

namespace Ecosim
{
    public class PauseState : IFSMState<StateGameplay>
    {
        private readonly FSMGameplay _fsm;
        private readonly PauseView _view;
        private readonly IInputDeviceProvider _input;
        private readonly ResumeSystem _resumeSystem;

        private CancellationTokenSource _cts;

        public PauseState(FSMGameplay fsm, PauseView view, IInputDeviceProvider input)
        {
            _fsm = fsm;
            _view = view;
            _input = input;
            
            UIHelper.SaveArea(view.SaveArea);
            view.Init(this);

            _resumeSystem = new ResumeSystem(_input, Resume);
        }

        public StateGameplay State => StateGameplay.PauseState;

        public void Enter()
        {
            _input.OnMenuEnable();
            _view.Open();

            StartLoop();
        }

        public void Exit()
        {
            EndLoop();
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

        private void StartLoop()
        {
            _cts = new CancellationTokenSource();
            Loop().Forget();
        }

        private void EndLoop()
        {
            if (_cts != null)
            {
                _cts?.Cancel();
                _cts?.Dispose(); 
                _cts = null;
            }
        }

        private async UniTaskVoid Loop()
        {
            while(!_cts.IsCancellationRequested)
            {
                _input.Sync();
                _input.Tick();
                _resumeSystem.Tick();

                await UniTask.Yield(PlayerLoopTiming.Update, _cts.Token);
            }
        }
    }
}
