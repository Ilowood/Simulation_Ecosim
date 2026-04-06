using System;
using System.Collections.Generic;
using UnityEngine;

namespace BugColony
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

        public event Action<Entity> OnEntityAdded;
        public event Action<Entity> OnEntityRemoved;

        public IReadOnlyList<Entity> Get(EntityType type) => _entitiesByType[type];
        public int GetCount(EntityType type) =>  _entitiesByType[type].Count;

        public void Init()
        {
            SpawnInitial();
        }

        public void Tick(float deltaTime)
        {
            foreach (var pair in _entitiesByType)
            {
                var list = pair.Value;

                for (var i = list.Count - 1; i >= 0; i--)
                {
                    var entity = list[i];

                    if (entity && !entity.IsDead)
                    {
                        entity.Tick(_context, deltaTime);
                    }
                }
            }

            ApplyCommands();
            MaintainEntities(EntityType.Food, _config.ResourceCount);
        }

        public void AddCommand(ISimulationCommand command)
        {
            _commands.Enqueue(command);
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
            OnEntityRemoved?.Invoke(entity);
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

        private void MaintainEntities(EntityType type, int targetCount)
        {
            if (!_entitiesByType.TryGetValue(type, out var list))
                return;

            int current = list.Count;

            if (current < targetCount)
            {
                int toSpawn = targetCount - current;
                SpawnAndRegister(type, toSpawn);
            }
        }

        private void SpawnInitial()
        {
            SpawnAndRegister(EntityType.Worker, _config.WorkerStartCount);
            SpawnAndRegister(EntityType.Predator, _config.PredatorStartCount);
            SpawnAndRegister(EntityType.Food, _config.ResourceCount);
        }

        private void RegisterEntity(Entity entity)
        {
            if (!_entitiesByType.TryGetValue(entity.Type, out var list))
            {
                list = new List<Entity>();
                _entitiesByType[entity.Type] = list;
            }

            list.Add(entity);
            OnEntityAdded?.Invoke(entity);
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
