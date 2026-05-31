using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Ecosim
{
    public class World
    {
        private readonly SpawnSystem _spawner;
        
        private readonly Queue<IWorldCommand> _commands = new();
        private readonly WorldContext _context;
        
        public EntityRegistry Registry { get; }
        public TaskFactory TaskFactory { get; }

        public World(EntityRegistry registry, SpawnSystem spawner, TaskFactory taskFactory)
        {
            Registry = registry;
            TaskFactory = taskFactory;

            _spawner = spawner;
            _context = new WorldContext(this);
        }

        public event Action<EntityType> OnEntityAdded;
        public event Action<EntityType> OnEntityRemoved;

        public async UniTask InitAsync(WorldSnapshot data)
        {
            await _spawner.InitAsync(); 
            Restore(data);
        }

        public void Deinit()
        {
            var specIds = Registry.GetRegisteredSpecIds();
            for (var i = 0; i < specIds.Count; i++)
            {
                foreach (var entity in Registry.GetBySpecId(specIds[i]))
                    _spawner.Despawn(entity);
            }

            Registry.Clear();
        }

        public void Tick(float deltaTime, float scale)
        {
            ForEachAllEntities(entity => entity.Tick(_context, deltaTime, scale));
            ApplyCommands();
        }

        public void SetPause(bool isPaused)
        {
            ForEachAllEntities(entity => entity.Behavior.SetPause(isPaused));
        }

        public WorldSnapshot GetSnapshot()
        {
            var snapshot = new WorldSnapshot();
            snapshot.IdGenerator = Registry.GetSnapshot();

            var specIds = Registry.GetRegisteredSpecIds();
            for (var i = 0; i < specIds.Count; i++)
            {
                foreach (var entity in Registry.GetBySpecId(specIds[i]))
                    snapshot.Entities.Add(entity.GetSnapshot());
            }

            return snapshot;
        }

        public void Restore(WorldSnapshot worldSnapshot)
        {
            if (worldSnapshot != null)
            {
                Registry.Clear();
                Registry.Restore(worldSnapshot.IdGenerator);

                var entities = new List<Entity>(worldSnapshot.Entities.Count);
                foreach (var snapshot in worldSnapshot.Entities)
                {
                    var entity = _spawner.Spawn(snapshot.InstanceId, snapshot.SpecId);

                    entities.Add(entity);
                    entity.Restore(snapshot);
                    Registry.Register(entity);
                }

                for (var i = 0; i < worldSnapshot.Entities.Count; i++)
                {
                    if (worldSnapshot.Entities[i].Task != null)
                    {
                        entities[i].Behavior.SetAndStartTask(worldSnapshot.Entities[i].Task.CreateTask(_context, entities[i]));
                        entities[i].Behavior.SetPause(true);
                    }
                }
            }
        }

        public void AddCommand(IWorldCommand command)
        {
            _commands.Enqueue(command);
        }

        public void RemoveEntityWithoutCallback(Entity entity)
        {
            RemoveEntity(entity);
        }

        public void RemoveEntityWithCallback(Entity entity)
        {
            RemoveEntity(entity);
            OnEntityRemoved?.Invoke(entity.Type);
        }

        public Entity AddEntity(long specId)
        {
            var entity = _spawner.Spawn(Registry.GenerateNextId(), specId);
            Registry.Register(entity);
            
            OnEntityAdded?.Invoke(entity.Type);
            return entity;
        }

        private void RemoveEntity(Entity entity)
        {
            Registry.Unregister(entity);
            _spawner.Despawn(entity);
        }

        private void ForEachAllEntities(Action<Entity> action)
        {
            var specIds = Registry.GetRegisteredSpecIds();
            for (var i = 0; i < specIds.Count; i++)
            {
                foreach (var entity in Registry.GetBySpecId(specIds[i]))
                {
                    if (entity && entity.IsActive)
                        action(entity);
                }
            }
        }

        private void ApplyCommands()
        {
            while (_commands.Count > 0)
            {
                var command = _commands.Dequeue();
                command.Execute(this);
            }
        }
    }
}
