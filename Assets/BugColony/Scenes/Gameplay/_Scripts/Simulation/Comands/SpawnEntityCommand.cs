namespace BugColony
{
    public class SpawnEntityCommand : ISimulationCommand
    {
        private readonly int _count;
        private readonly EntityType _type;
        public int Priority { get; private set; }

        public SpawnEntityCommand(int count, EntityType type)
        {
            _count = count;
            _type = type;
            Priority = 0;
        }

        public void Execute(Simulation simulation)
        {
            simulation.SpawnAndRegister(_type, _count);
        }
    }
}
