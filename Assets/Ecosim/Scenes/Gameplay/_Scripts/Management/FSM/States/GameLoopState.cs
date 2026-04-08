using Untils;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Threading;

namespace Ecosim
{
    public class GameLoopState : ISuspendFSMState<StateGameplay>
    {
        private readonly FSMGameplay _fsm;
        private readonly HUDView _view;
        private readonly Simulation _simulation;

        private readonly float[] _speedSteps = { 0.2f, 1.0f, 9.0f };

        private int _currentSpeedIndex = 1;
        private float _currentTimeScale;
        private CancellationTokenSource _puaseSource;

        public GameLoopState(FSMGameplay fsm, HUDView view, Simulation simulation)
        {
            _fsm = fsm;
            _view = view;
            _simulation = simulation;
            _currentTimeScale = _speedSteps[_currentSpeedIndex];
            
            UIHelper.SaveArea(view.SaveArea);
            view.Init(this, simulation);
        }

        public StateGameplay State => StateGameplay.GameLoopState;

        public void Enter()
        {
            _view.Open();
            StartLoop();
        }

        public void Exit()
        {
            EndLoop();

            _view.Close();
            _view.Deinit(_simulation);
        }

        public void Resume()
        {
            _simulation.SetPause(false);
            _view.Open();
            StartLoop();
        }

        public void Suspend()
        {
            EndLoop();
            _simulation.SetPause(true);
            _view.Close();
        }

        public void PauseState()
        {
            _fsm.SuspendAndEnterIn(StateGameplay.PauseState);
        }

        public void ToggleSpeed()
        {
            _currentSpeedIndex = (_currentSpeedIndex + 1) % _speedSteps.Length;
            _currentTimeScale = _speedSteps[_currentSpeedIndex];
        }

        private async UniTask GameLoop()
        {
            while(!_puaseSource.IsCancellationRequested)
            {
                _simulation.Tick(Time.deltaTime, _currentTimeScale);

                if (_simulation.GetTrackedEntityCount() == 0)
                {
                    
                }

                await UniTask.Yield(PlayerLoopTiming.Update, _puaseSource.Token);
            }
        }

        private void StartLoop()
        {
            _puaseSource = new CancellationTokenSource();
            GameLoop().Forget();
        }

        private void EndLoop()
        {
            _puaseSource?.Cancel();
            _puaseSource?.Dispose();
        }
    }
}
