using Untils;

namespace Ecosim
{
    public class RestartState : IFSMState<StateGameplay>
    {
        private readonly FSMGameplay _fsm;
        private readonly ISaveService _saveService;
        private readonly World _world;
        private readonly SelectionBuffer _buffer;
        
        public RestartState(FSMGameplay fsm, World world, ISaveService saveService, SelectionBuffer buffer)
        {
            _fsm = fsm;
            _world = world;
            _saveService = saveService;
            _buffer = buffer;
        }

        public StateGameplay State => StateGameplay.RestartState;

        public void Enter()
        {
            _world.Deinit();
            _world.Restore(_saveService.LoadWorld("Ecosim"));
            _buffer.Reset();
            _buffer.Restore(_world.Registry.SelectableEntities);

            _fsm.EnterIn(StateGameplay.GameplayState);
        }

        public void Exit()
        {
            
        }
    }
}
