using UnityEngine;
using System.Collections.Generic;
using Untils;

namespace BugColony
{
    public class Spawner
    {
        private readonly Dictionary<EntityType, PoolObj<Entity>> _pools = new();
        private readonly Dictionary<EntityType, EntitySpecification> _configs = new();

        private int _lastIndex = 0;

        public Spawner(SpawnerEntityConfig config)
        {
            foreach (var entityConfig in config.EntitySpawnConfigs)
            {
                EntityType type = entityConfig.Specification.Type;
                _configs[type] = entityConfig.Specification;

                _pools[type] = new PoolObj<Entity>(() => InstantiateEntity(entityConfig), Release, Get, config.InitialPoolSize);
            }
        }

        public List<Entity> Spawn(EntityType type, int count)
        {
            var entities = _pools[type].Get(count);

            for (var i = 0; i < entities.Count; i++)
            {
                entities[i].transform.position = NavMeshHelper.GetRandomPoint();
                // entities[i].transform.rotation = spawnPoint.transform.rotation;
            }

            return entities;
        }

        public void Delete(Entity entity)
        {
            _pools[entity.Type].Release(entity);
        }

        private Entity InstantiateEntity(EntitySpawnConfig config)
        {
            return EntityFactory.Create(Vector3.zero, config.Parent, config.Specification);
        }

        private void Release(Entity entity)
        {
            entity.gameObject.SetActive(false);
        }

        private void Get(Entity entity, int index)
        {
            entity.gameObject.SetActive(true);
        }
    }
}
