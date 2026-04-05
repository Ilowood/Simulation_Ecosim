using Untils;

namespace BugColony
{
    public class InitState : IFSMState<StateGameplay>
    {
        private readonly FSMGameplay _fsm;
        private readonly Simulation _simulation;

        public InitState(FSMGameplay fsm, Simulation simulation)
        {
            _fsm = fsm;
            _simulation = simulation;
        }

        public StateGameplay State => StateGameplay.InitState;

        public void Enter()
        {
            _simulation.Init();
            _fsm.EnterIn(StateGameplay.GameLoopState);
        }

        public void Exit()
        {
            
        }
    }
}
