using Cysharp.Threading.Tasks;
using UnityEngine;
using Untils;

namespace Ecosim
{
    public class InitState : IFSMState<StateGameplay>
    {
        private readonly FSMGameplay _fsm;
        private readonly World _world;
        private readonly LoadView _view;
        private readonly ISaveService _saveService;

        public InitState(FSMGameplay fsm, World world, LoadView view, ISaveService saveService)
        {
            _fsm = fsm;
            _saveService = saveService;
            _world = world;
            _view = view;
        }

        public StateGameplay State => StateGameplay.InitState;

        public void Enter()
        {
            EnterAsync().Forget();
        }

        private async UniTaskVoid EnterAsync()
        {
            _view.Open();

            var startTime = Time.realtimeSinceStartup;

            await _world.InitAsync(_saveService.LoadWorld("Ecosim")); 

            var elapsed = Time.realtimeSinceStartup - startTime;
            var minDuration = 2.0f;

            if (elapsed < minDuration)
            {
                var delaySeconds = minDuration - elapsed;
                await UniTask.Delay(System.TimeSpan.FromSeconds(delaySeconds));
            }

            _fsm.EnterIn(StateGameplay.SimulationState);
        }

        public void Exit()
        {
            _view.Close();
        }
    }
}
