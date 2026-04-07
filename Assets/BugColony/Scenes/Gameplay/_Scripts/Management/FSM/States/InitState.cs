using Cysharp.Threading.Tasks;
using UnityEngine;
using Untils;

namespace BugColony
{
    public class InitState : IFSMState<StateGameplay>
    {
        private readonly FSMGameplay _fsm;
        private readonly Simulation _simulation;
        private readonly Spawner _spawner;
        private readonly LoadView _view;

        public InitState(FSMGameplay fsm, Simulation simulation, Spawner spawner, LoadView view)
        {
            _fsm = fsm;
            _simulation = simulation;
            _spawner = spawner;
            _view = view;
        }

        public StateGameplay State => StateGameplay.InitState;

        public async void Enter()
        {
            EnterAsync().Forget();
        }

        private async UniTaskVoid EnterAsync()
        {
            _view.Open();

            var startTime = Time.realtimeSinceStartup;
            await _spawner.InitAsync(); 
            _simulation.Init();

            var elapsed = Time.realtimeSinceStartup - startTime;
            var minDuration = 4.0f;

            if (elapsed < minDuration)
            {
                float delaySeconds = minDuration - elapsed;
                await UniTask.Delay(System.TimeSpan.FromSeconds(delaySeconds));
            }

            _fsm.EnterIn(StateGameplay.GameLoopState);
        }

        public void Exit()
        {
            _view.Close();
        }
    }
}
