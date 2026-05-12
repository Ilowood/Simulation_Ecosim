namespace Ecosim
{
    public class RemoveEntityWithCoolbackCommand : ISimulationCommand
    {
        private readonly Entity _entity;

        public int Priority { get; private set; }

        public RemoveEntityWithCoolbackCommand(Entity entity)
        {
            Priority = 0;
            _entity = entity;
        }

        public void Execute(World simulation)
        {
            simulation.RemoveEntityWithCallback(_entity);
        }
    }
}
