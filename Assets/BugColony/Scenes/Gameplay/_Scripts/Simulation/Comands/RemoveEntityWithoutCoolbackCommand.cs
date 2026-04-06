namespace BugColony
{
    public class RemoveEntityWithoutCoolbackCommand : ISimulationCommand
    {
        private readonly Entity _entity;

        public int Priority { get; private set; }

        public RemoveEntityWithoutCoolbackCommand(Entity entity)
        {
            Priority = 0;
            _entity = entity;
        }

        public void Execute(Simulation simulation)
        {
            simulation.RemoveEntityWithoutCoolback(_entity);
        }
    }
}
