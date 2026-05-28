namespace Ecosim
{
    public class RemoveEntityWithCoolbackCommand : IWorldCommand
    {
        private readonly Entity _entity;

        public int Priority { get; private set; } = 0;

        public RemoveEntityWithCoolbackCommand(Entity entity)
        {
            _entity = entity;
        }

        public void Execute(World simulation)
        {
            simulation.RemoveEntityWithCallback(_entity);
        }
    }
}
