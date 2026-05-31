using Untils;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Threading;

namespace Ecosim
{
    public class WorldState : ISuspendFSMState<StateGameplay>
    {
        private readonly FSMGameplay _fsm;
        private readonly ISaveService _saveService;
        private readonly HUDView _view;
        private readonly World _world;

        private readonly float[] _speedSteps = { 0.5f, 2.0f, 10.0f };

        private int _currentSpeedIndex = 1;
        private float _currentTimeScale;
        private CancellationTokenSource _puaseSource;

        public WorldState(FSMGameplay fsm, HUDView view, World world, ISaveService saveService)
        {
            _fsm = fsm;
            _saveService = saveService;

            _view = view;
            _world = world;
            _currentTimeScale = _speedSteps[_currentSpeedIndex];
            
            UIHelper.SaveArea(view.SaveArea);
            view.Init(this);
        }

        public StateGameplay State => StateGameplay.SimulationState;

        public void Enter()
        {
            _view.Open(_world);

            // var entity = _world.AddEntity(5072577398122940851);
            // _world.AddEntity();
            // var storage = entity.Get<StorageComponent>();
            // StorageService.TryAdd(storage, 5644808556783927713, 30, true);

            _world.SetPause(false);
            

            StartLoop();
        }

        public void Exit()
        {
            EndLoop();

            _view.Close(_world);
            _view.ResetView();
        }

        public void Resume()
        {
            _world.SetPause(false);
            _view.Open();
            StartLoop();
        }

        public void Suspend()
        {
            EndLoop();
            _world.SetPause(true);
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

        public void SaveWorld()
        {
            _world.SetPause(true);
            _saveService.SaveWorld(_world.GetSnapshot(), "Ecosim");
            _world.SetPause(false);
        }

        private void StartLoop()
        {
            _puaseSource = new CancellationTokenSource();
            Loop().Forget();
        }

        private void EndLoop()
        {
            _puaseSource?.Cancel();
            _puaseSource?.Dispose();
        }

        private async UniTaskVoid Loop()
        {
            while(!_puaseSource.IsCancellationRequested)
            {
                _world.Tick(Time.deltaTime, _currentTimeScale);

                // if (_world.GetTrackedCount() == 0)
                // {
                //     _fsm.EnterIn(StateGameplay.ReportState);
                //     return;
                // }

                await UniTask.Yield(PlayerLoopTiming.Update, _puaseSource.Token);
            }
        }
    }
}
