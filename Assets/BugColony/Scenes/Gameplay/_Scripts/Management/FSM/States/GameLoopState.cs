using Untils;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Threading.Tasks;

namespace BugColony
{
    public class GameLoopState : ISuspendFSMState<StateGameplay>
    {
        private readonly FSMGameplay _fsm;
        private readonly HUDView _view;
        private readonly Simulation _simulation;

        private readonly float[] _speedSteps = { 1.0f, 0.2f };

        private int _currentSpeedIndex = 0;
        private float _currentTimeScale;

        public GameLoopState(FSMGameplay fsm, HUDView view, Simulation simulation)
        {
            _fsm = fsm;
            _view = view;
            _simulation = simulation;
            
            UI.SaveArea(view.SaveArea);
            view.Init(this, simulation);
        }

        public StateGameplay State => StateGameplay.GameLoopState;

        public void Enter()
        {
            _view.Open();
            GameLoop().Forget();
        }

        public void Exit()
        {
            _view.Close();
            _view.Deinit(_simulation);
        }

        public void Resume()
        {
            _view.Open();
        }

        public void Suspend()
        {
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
            while(true)
            {
                _simulation.Tick(Time.deltaTime);
                await UniTask.Yield(PlayerLoopTiming.Update);
            }
        }
    }
}
