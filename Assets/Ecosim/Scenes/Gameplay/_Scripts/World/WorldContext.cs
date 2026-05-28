namespace Ecosim
{
    public struct WorldContext
    {
        private readonly World _world;

        public TaskFactory TaskFactory { get; }
        public IEntityRegistry Registry { get; }

        public WorldContext(World world)
        {
            _world = world;
            
            TaskFactory = world.TaskFactory;
            Registry = world.Registry;
        }

        public void RemoveEntityWithoutCoolbackCommand(Entity entity)
        {
            entity.Deactivate();
            _world.AddCommand(new RemoveEntityWithoutCoolbackCommand(entity));
        }

        public void RemoveEntityWithCoolbackCommand(Entity entity)
        {
            entity.Deactivate();
            _world.AddCommand(new RemoveEntityWithCoolbackCommand(entity));
        }

        public void SpawnEntityCompand(long specId)
        {
            _world.AddCommand(new SpawnEntityCommand(specId));
        }
    }
}


