using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Untils;

namespace Ecosim
{
    public class World : IReadOnlyEntityStorage
    {
        private readonly Spawner _spawner;
        private readonly IdGenerator _idGenerator = new();
        
        private readonly Dictionary<long, Entity> _entities = new(capacity: 2048);
        private readonly Dictionary<EntityType, List<Entity>> _entitiesByType = new();
        
        private readonly Queue<ISimulationCommand> _commands = new();
        private readonly WorldContext _context;

        public World(Spawner spawner)
        {
            _spawner = spawner;
            _context = new WorldContext(this);

            foreach (EntityType type in Enum.GetValues(typeof(EntityType)))
            {
                _entitiesByType[type] = new List<Entity>();
            }
        }

        public event Action<EntityType> OnEntityAdded;
        public event Action<EntityType> OnEntityRemoved;

        public IReadOnlyList<Entity> Get(EntityType type) => _entitiesByType[type];

        public async UniTask InitAsync(WorldSnapshot data)
        {
            await _spawner.InitAsync(); 
            Restore(data);
        }

        public void Deinit()
        {
            AllDestroy();
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

        public Entity GetById(long instanceId)
        {
            if (_entities.TryGetValue(instanceId, out var entity))
            {
                return entity;
            }
            
            return null;
        }

        public WorldSnapshot GetSnapshot()
        {
            var snapshot = new WorldSnapshot();

            snapshot.IdGenerator = _idGenerator.GetSnapshot();

            foreach (var entityList in _entitiesByType.Values)
            {
                foreach (var entity in entityList)
                {
                    if (entity != null && entity.IsActive)
                    {
                        snapshot.Entities.Add(entity.GetSnapshot());
                    }
                }
            }

            return snapshot;
        }

        public void Restore(WorldSnapshot data)
        {
            AllDestroy();

            _idGenerator.Reset(); 
            _idGenerator.Restore(data.IdGenerator);

            var entities = new List<Entity>(data.Entities.Count);

            foreach (var snapshot in data.Entities)
            {
                var entity = _spawner.Spawn(snapshot.InstanceId, snapshot.SpecId);

                entities.Add(entity);
                entity.Restore(snapshot);
                RegisterEntity(entity);
            }

            for (var i = 0; i < data.Entities.Count; i++)
            {
                if (data.Entities[i].Task != null)
                {
                    entities[i].Behavior.SetAndStartTask(data.Entities[i].Task.CreateTask(entities[i]));
                    entities[i].Behavior.SetPause(true);
                }
            }
        }

        public void AddCommand(ISimulationCommand command)
        {
            _commands.Enqueue(command);
        }

        public int GetCount(EntityType type) 
        {
            return _entitiesByType[type].Count;
        }

        public int GetTrackedCount()
        {
            // var result = 0;

            // foreach (var entities in _entitiesByType)
            // {
            //     if ((_config.TrackedLiveEntities & entities.Key) != 0)
            //     {
            //         result += entities.Value.Count;
            //     }
            // }
            
            // return result;
            return 10;
        }

        public void RemoveEntityWithoutCallback(Entity entity)
        {
            RemoveEntity(entity);
            _spawner.Despawn(entity);
        }

        public void RemoveEntityWithCallback(Entity entity)
        {
            RemoveEntity(entity);
            _spawner.Despawn(entity);

            OnEntityRemoved?.Invoke(entity.Type);
        }

        public void SpawnAndRegister(long specId)
        {
            var entity = _spawner.Spawn(_idGenerator.GetNext(), specId);
            RegisterEntity(entity);
        }

        private void RemoveEntity(Entity entity)
        {
            if (_entitiesByType.TryGetValue(entity.Type, out var entities))
            {
                entities.Remove(entity);

                _idGenerator.Release(entity.Id);
                _entities.Remove(entity.Id);
            }
        }

        private void AllDestroy()
        {
            foreach (var entities in _entitiesByType.Values)
            {
                for (var i = 0; i < entities.Count; i++)
                {
                    _spawner.Despawn(entities[i]);
                }

                entities.Clear();
            }

            _entities.Clear();
            _idGenerator.Reset();
        }

        private void RegisterEntity(Entity entity)
        {
            if (!_entitiesByType.TryGetValue(entity.Type, out var list))
            {
                list = new List<Entity>();
                _entitiesByType[entity.Type] = list;
            }

            list.Add(entity);
            _entities[entity.Id] = entity;
            OnEntityAdded?.Invoke(entity.Type);
        }

        private void ForEachAllEntities(Action<Entity> action)
        {
            foreach (var entities in _entitiesByType.Values)
            {
                for (var i = entities.Count - 1; i >= 0; i--)
                {
                    if (entities[i] && entities[i].IsActive)
                    {
                        action(entities[i]);
                    }
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
