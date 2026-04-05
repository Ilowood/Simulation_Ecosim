using System;
using System.Collections.Generic;

namespace BugColony
{
    public class Simulation : IReadOnlyEntityStorage
    {
        private readonly Spawner _spawner;
        private readonly Dictionary<EntityType, List<Entity>> _entitiesByType = new();
        private readonly Queue<ISimulationCommand> _commands = new();

        private SimulationContext _context;

        public Simulation(Spawner spawner)
        {
            _spawner = spawner;
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

                for (int i = list.Count - 1; i >= 0; i--)
                {
                    var entity = list[i];

                    if (entity)
                    {
                        entity.Tick(_context, deltaTime);
                    }
                }
            }

            ApplyCommands();
        }

        public void AddCommand(ISimulationCommand command)
        {
            _commands.Enqueue(command);
        }

        public void RemoveEntityImmediate(Entity entity)
        {
            if (_entitiesByType.TryGetValue(entity.Type, out var list))
            {
                list.Remove(entity);
            }

            OnEntityRemoved?.Invoke(entity);
            _spawner.Delete(entity);
        }

        private void SpawnInitial()
        {
            SpawnAndRegister(EntityType.Worker, 10);
            SpawnAndRegister(EntityType.Predator, 2);
            SpawnAndRegister(EntityType.Food, 2);
        }

        private void SpawnAndRegister(EntityType type, int count, bool registerAsResource = false)
        {
            var entities = _spawner.Spawn(type, count);

            foreach (var entity in entities)
            {
                RegisterEntity(entity);
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
