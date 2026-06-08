using Untils;

namespace Ecosim
{
    public class RestartState : IFSMState<StateGameplay>
    {
        private readonly FSMGameplay _fsm;
        private readonly World _world;
        private readonly ISaveService _saveService;
        
        public RestartState(FSMGameplay fsm, World world, ISaveService saveService)
        {
            _fsm = fsm;
            _world = world;
            _saveService = saveService;
        }

        public StateGameplay State => StateGameplay.RestartState;

        public void Enter()
        {
            _world.Deinit();
            _world.Restore(_saveService.LoadWorld("Ecosim"));

            _fsm.EnterIn(StateGameplay.GameplayState);
        }

        public void Exit()
        {
            
        }
    }
}
