using Untils;

namespace Ecosim
{
    public class RestartState : IFSMState<StateGameplay>
    {
        private readonly FSMGameplay _fsm;
        private readonly World _world;
        
        public RestartState(FSMGameplay fsm, World world)
        {
            _fsm = fsm;
            _world = world;
        }

        public StateGameplay State => StateGameplay.RestartState;

        public void Enter()
        {
            // _simulation.Deinit();
            // _simulation.Init();
            // _fsm.EnterIn(StateGameplay.SimulationState);
        }

        public void Exit()
        {
            
        }
    }
}
