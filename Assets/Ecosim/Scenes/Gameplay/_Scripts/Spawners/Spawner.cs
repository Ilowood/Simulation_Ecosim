using UnityEngine;
using System.Collections.Generic;
using Untils;
using Cysharp.Threading.Tasks;

namespace Ecosim
{
    public class Spawner
    {
        private const int Max_PRE_FRAME = 50;


        private readonly EntityFactory _factory;
        private readonly SpawnerConfig _config;

        private Dictionary<EntityType, PoolObj<Entity>> _pools = new();
        private Dictionary<EntityType, EntitySpecification> _configs = new();

        public Spawner(EntityFactory factory, SpawnerConfig config)
        {
            _factory = factory;
            _config = config;
        }

        public async UniTask InitAsync()
        {
            var currentFrameCount = 0;

            foreach (var entityConfig in _config.EntitySpawnConfigs)
            {
                var type = entityConfig.Specification.Type;
                _configs[type] = entityConfig.Specification;

                var pool = new PoolObj<Entity>(() => InstantiateEntity(entityConfig), Release, Get);
                _pools[type] = pool;

                var remainingToReserve = _config.InitialPoolSize;

                while (remainingToReserve > 0)
                {
                    var spaceInFrame = Max_PRE_FRAME - currentFrameCount;
                    var amountToSpawn = System.Math.Min(remainingToReserve, spaceInFrame);

                    if (amountToSpawn > 0)
                    {
                        pool.Reserv(amountToSpawn);
                        remainingToReserve -= amountToSpawn;
                        currentFrameCount += amountToSpawn;
                    }

                    if (currentFrameCount >= Max_PRE_FRAME)
                    {
                        await UniTask.Yield(PlayerLoopTiming.Update);
                        currentFrameCount = 0;
                    }
                }
            }

            await UniTask.Yield(PlayerLoopTiming.Update);
        }

        public List<Entity> Spawn(EntityType type, int count)
        {
            var entities = _pools[type].Get(count);

            for (var i = 0; i < entities.Count; i++)
            {
                entities[i].transform.position = NavMeshHelper.GetRandomPoint();
            }

            return entities;
        }

        public void Delete(Entity entity)
        {
            _pools[entity.Type].Release(entity);
        }

        private Entity InstantiateEntity(EntityConfig config)
        {
            return _factory.Create(config.Specification, Vector3.zero, config.Parent);
            // return EntityFactory.Create(Vector3.zero, config.Parent, config.Specification);
        }

        private void Release(Entity entity)
        {
            entity.gameObject.SetActive(false);
        }

        private void Get(Entity entity, int index)
        {
            entity.IsDead = false;
            entity.gameObject.SetActive(true);
        }
    }
}
