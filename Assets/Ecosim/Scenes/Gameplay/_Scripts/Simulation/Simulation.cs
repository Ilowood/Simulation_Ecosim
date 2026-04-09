using System;
using System.Collections.Generic;

namespace Ecosim
{
    public class Simulation : IReadOnlyEntityStorage
    {
        private readonly Spawner _spawner;
        private readonly Dictionary<EntityType, List<Entity>> _entitiesByType = new();
        private readonly Queue<ISimulationCommand> _commands = new();
        private readonly SimulationConfig _config;

        private SimulationContext _context;

        public Simulation(Spawner spawner, SimulationConfig config)
        {
            _spawner = spawner;
            _config = config;

            _context = new SimulationContext(this);

            foreach (EntityType type in Enum.GetValues(typeof(EntityType)))
            {
                _entitiesByType[type] = new List<Entity>();
            }
        }

        public event Action<EntityType> OnEntityAdded;
        public event Action<EntityType> OnEntityRemoved;

        public IReadOnlyList<Entity> Get(EntityType type) => _entitiesByType[type];

        public void Init()
        {
            SpawnInitial();
        }

        public void Deinit()
        {
            DestroyDeinitial();
        }

        public void Tick(float deltaTime, float scale)
        {
            ForEachEntity(entity => entity.Tick(_context, deltaTime, scale));
            ApplyCommands();
            MaintainAllEntities();
        }

        public void SetPause(bool isPaused)
        {
            ForEachEntity(entity => entity.Behavior.SetPause(isPaused));
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
            var result = 0;

            foreach (var entities in _entitiesByType)
            {
                if ((_config.TrackedLiveEntities & entities.Key) != 0)
                {
                    result += entities.Value.Count;
                }
            }
            
            return result;
        }

        public void RemoveEntityWithoutCoolback(Entity entity)
        {
            RemoveEntity(entity);
            _spawner.Delete(entity);
        }

        public void RemoveEntityWithCoolback(Entity entity)
        {
            RemoveEntity(entity);
            _spawner.Delete(entity);
            OnEntityRemoved?.Invoke(entity.Type);
        }

        public void SpawnAndRegister(EntityType type, int count)
        {
            var entities = _spawner.Spawn(type, count);

            foreach (var entity in entities)
            {
                RegisterEntity(entity);
            }
        }

        private void RemoveEntity(Entity entity)
        {
            if (_entitiesByType.TryGetValue(entity.Type, out var list))
            {
                list.Remove(entity);
            }
        }

        private void MaintainAllEntities()
        {
            foreach (var data in _config.EntitiesSettings)
            {
                if (data.TargetPopulation > 0)
                {
                    MaintainEntities(data.Type, data.TargetPopulation);
                }
            }
        }

        private void MaintainEntities(EntityType type, int targetCount)
        {
            if (!_entitiesByType.TryGetValue(type, out var list))
                return;

            var current = list.Count;

            if (current < targetCount)
            {
                int toSpawn = targetCount - current;
                SpawnAndRegister(type, toSpawn);
            }
        }

        private void SpawnInitial()
        {
            foreach (var data in _config.EntitiesSettings)
            {
                if (data.StartCount > 0)
                {
                    SpawnAndRegister(data.Type, data.StartCount);
                }
            }
        }

        private void DestroyDeinitial()
        {
            foreach (var (type, entities) in _entitiesByType)
            {
                for (var i = 0; i < entities.Count; i++)
                {
                    _spawner.Delete(entities[i]);
                }

                entities.Clear();
            }
        }

        private void RegisterEntity(Entity entity)
        {
            if (!_entitiesByType.TryGetValue(entity.Type, out var list))
            {
                list = new List<Entity>();
                _entitiesByType[entity.Type] = list;
            }

            list.Add(entity);
            OnEntityAdded?.Invoke(entity.Type);
        }

        private void ForEachEntity(Action<Entity> action)
        {
            foreach (var list in _entitiesByType.Values)
            {
                for (var i = list.Count - 1; i >= 0; i--)
                {
                    var entity = list[i];
                    if (entity && !entity.IsDead)
                    {
                        action(entity);
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
