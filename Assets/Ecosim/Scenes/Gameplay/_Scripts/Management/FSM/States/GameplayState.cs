using Untils;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Threading;

namespace Ecosim
{
    public class GameplayState : ISuspendFSMState<StateGameplay>
    {
        private readonly FSMGameplay _fsm;
        private readonly ISaveService _saveService;
        private readonly IInputDeviceProvider _input;
        private readonly HUDView _view;
        private readonly World _world;

        private readonly float[] _speedSteps = { 0.5f, 2.0f, 10.0f };

        private int _currentSpeedIndex = 1;
        private float _currentTimeScale;
        private CancellationTokenSource _puaseSource;

        public GameplayState(FSMGameplay fsm, HUDView view, World world, ISaveService saveService, IInputDeviceProvider input)
        {
            _fsm = fsm;
            _saveService = saveService;
            _input = input;

            _view = view;
            _world = world;
            _currentTimeScale = _speedSteps[_currentSpeedIndex];
            
            UIHelper.SaveArea(_view.SaveArea);
            _view.Init(this);

            _input.OnPauseEvent += PauseState;
        }

        public StateGameplay State => StateGameplay.GameplayState;

        public void Enter()
        {
            _input.OnGameplayEnable();
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
            _input.OnGameplayEnable();
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
            if (_puaseSource != null)
            {
                _puaseSource?.Cancel();
                _puaseSource?.Dispose(); 
                _puaseSource = null;
            }
        }

        private async UniTaskVoid Loop()
        {
            while(!_puaseSource.IsCancellationRequested)
            {
                _world.Tick(Time.deltaTime, _currentTimeScale);
                await UniTask.Yield(PlayerLoopTiming.Update, _puaseSource.Token);
            }
        }
    }
}
