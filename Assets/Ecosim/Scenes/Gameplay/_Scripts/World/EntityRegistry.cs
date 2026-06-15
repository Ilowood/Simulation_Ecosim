using System.Collections.Generic;
using Untils;

namespace Ecosim
{
    public class EntityRegistry : IEntityRegistry
    {
        private readonly IdGenerator _idGenerator = new();
        
        private readonly Dictionary<long, Entity> _entities = new(capacity: 2048);
        private readonly Dictionary<long, List<Entity>> _entitiesBySpecId = new();
        private readonly List<long> _cachedSpecIds = new(capacity: 64);

        private readonly List<Entity> _selectableEntities = new(1024);

        public IReadOnlyCollection<Entity> SelectableEntities => _selectableEntities;

        public IReadOnlyList<long> GetRegisteredSpecIds()
        {
            return _cachedSpecIds;
        }

        public IReadOnlyCollection<Entity> GetBySpecId(long specId) 
        {
            if(_entitiesBySpecId.TryGetValue(specId, out List<Entity> entities)) return entities;
            return new List<Entity>();
        }

        public Entity GetById(long instanceId)
        {
            if (_entities.TryGetValue(instanceId, out var entity))
            {
                return entity;
            }
            
            return null;
        }

        public int GetCount(long specId) 
        {
            return _entitiesBySpecId[specId].Count;
        }

        public bool IsRegistred(Entity entity)
        {
            return _entities.ContainsKey(entity.Id) && _entities[entity.Id] == entity;
        }

        public void Register(Entity entity)
        {
            _entities[entity.Id] = entity;

            if (!_entitiesBySpecId.TryGetValue(entity.SpecId, out var entities))
            {
                entities = new List<Entity>();

                _entitiesBySpecId[entity.SpecId] = entities;
                _cachedSpecIds.Add(entity.SpecId);
            }

            entities.Add(entity);

            if (entity.Get<SelectableComponent>() != default)
                _selectableEntities.Add(entity);
        }

        public void Unregister(Entity entity)
        {
            if (_entitiesBySpecId.TryGetValue(entity.SpecId, out var entities))
            {
                entities.Remove(entity);

                _idGenerator.Release(entity.Id);
                _entities.Remove(entity.Id);

                if (entities.Count == 0)
                {
                    _entitiesBySpecId.Remove(entity.SpecId);
                    _cachedSpecIds.Remove(entity.SpecId);
                }
            }

            _selectableEntities.Remove(entity);
        }

        public long GenerateNextId()
        {
            return _idGenerator.GetNext();
        }

        public IdGeneratorSnapshot GetSnapshot()
        {
            return _idGenerator.GetSnapshot();
        }

        public void Restore(IdGeneratorSnapshot snapshot)
        {
            _idGenerator.Restore(snapshot);
        }

        public void Clear()
        {
            _entities.Clear();
            _entitiesBySpecId.Clear();
            _idGenerator.Clear();
            _cachedSpecIds.Clear();
            _selectableEntities.Clear();
        }
    }
}
