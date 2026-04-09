using Cysharp.Threading.Tasks;
using Untils;

namespace Ecosim
{
    public class RestartState : IFSMState<StateGameplay>
    {
        private readonly FSMGameplay _fsm;
        private readonly Simulation _simulation;
        
        public RestartState(FSMGameplay fsm, Simulation simulation)
        {
            _fsm = fsm;
            _simulation = simulation;
        }

        public StateGameplay State => StateGameplay.RestartState;

        public async void Enter()
        {
            EnterAsync().Forget();
        }

        private async UniTaskVoid EnterAsync()
        {
            _simulation.Deinit();
            _simulation.Init();
            _fsm.EnterIn(StateGameplay.SimulationState);
        }

        public void Exit()
        {
            
        }
    }
}
