namespace Ecosim
{
    public class RemoveEntityWithoutCoolbackCommand : IWorldCommand
    {
        private readonly Entity _entity;

        public int Priority { get; private set; } = 0;

        public RemoveEntityWithoutCoolbackCommand(Entity entity)
        {
            _entity = entity;
        }

        public void Execute(World simulation)
        {
            simulation.RemoveEntityWithoutCallback(_entity);
        }
    }
}
