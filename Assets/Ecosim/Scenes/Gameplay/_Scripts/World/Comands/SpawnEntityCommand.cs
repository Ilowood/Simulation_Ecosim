namespace Ecosim
{
    public class SpawnEntityCommand : ISimulationCommand
    {
        private readonly long _specId;

        public int Priority { get; private set; } = 0;

        public SpawnEntityCommand(long specId)
        {
            _specId = specId;
        }

        public void Execute(World simulation)
        {
            simulation.SpawnAndRegister(_specId);
        }
    }
}
