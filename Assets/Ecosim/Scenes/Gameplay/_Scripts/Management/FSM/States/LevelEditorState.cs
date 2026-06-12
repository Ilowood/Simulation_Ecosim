using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Untils;

namespace Ecosim
{
    public class LevelEditorState : IFSMState<StateGameplay>
    {
        private readonly FSMGameplay _fsm;
        private readonly ISaveService _saveService;
        private readonly EntityDatabase _database;

        private readonly World _world;
        private readonly ToolFactory _toolFactory;
        private readonly IInputDeviceProvider _input;

        private readonly LevelEditorView _view;

        private EditorToolPipeline _currentTool;
        private CancellationTokenSource _puaseSource;

        public LevelEditorState(FSMGameplay fsm, LevelEditorView view, EntityDatabase database, World world, ToolFactory toolFactory, ISaveService saveService, IInputDeviceProvider input)
        {
            _fsm = fsm;
            _saveService = saveService;
            _database = database;

            _world = world;
            _toolFactory = toolFactory;
            _input = input;

            _view = view;
            
            UIHelper.SaveArea(view.SaveArea);
            view.Init(this);
        }

        public StateGameplay State => StateGameplay.LevelEditorState;
        public IReadOnlyList<EntitySpecification> Specifications => _database.Specifications;

        public void Enter()
        {
            _input.OnEditorEnable();
            _view.Open();

            _world.Deinit();
            _world.Restore(_saveService.LoadWorld("Ecosim"));

            StartLoop();
        }

        public void Exit()
        {
            _view.Close();
            
            EndLoop();
        }

        public void GameState()
        {
            _fsm.EnterIn(StateGameplay.RestartState);
        }

        public void SaveWorld()
        {
            _world.SetPause(true);
            _saveService.SaveWorld(_world.GetSnapshot(), "Ecosim");
            _world.SetPause(false);
        }

        public void BuildTool(long specId)
        {
            _currentTool = _toolFactory.Create(ToolVariants.Build, new BuildToolParams(specId));
            _currentTool.OnCancelled += ResetCurrentTool;

            _currentTool.NextStep();
        }

        private void ResetCurrentTool()
        {
            _currentTool.OnCancelled -= ResetCurrentTool;
            _currentTool = null;
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
                _input.Sync();
                _currentTool?.Tick();
                _input.Tick();
                
                await UniTask.Yield(PlayerLoopTiming.Update, _puaseSource.Token);
            }
        }
    }
}
