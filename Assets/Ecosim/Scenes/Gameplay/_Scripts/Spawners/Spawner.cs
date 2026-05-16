using UnityEngine;
using System.Collections.Generic;
using Untils;
using Cysharp.Threading.Tasks;
using System;

namespace Ecosim
{
    public class Spawner
    {
        private readonly EntityRegistry _registry;
        private readonly EntityFactory _factory;
        private readonly SpawnerConfig _config;

        private Dictionary<long, PoolObj<Entity>> _pools = new();

        private Transform _globalContainer;

        public Spawner(EntityFactory factory, SpawnerConfig config, EntityRegistry registry)
        {
            _registry = registry;
            _factory = factory;
            _config = config;
        }

        public async UniTask InitAsync()
        {
            _globalContainer = new GameObject("World (Dinamic)").transform;
            var currentFrameCount = 0;

            foreach (var config in _config.PoolConfigs)
            {
                var spec = _registry.GetById(config.SpecId);
                
                var container = new GameObject($"Pool_{spec.SpecId}").transform;
                container.SetParent(_globalContainer); 

                var pool = new PoolObj<Entity>(() => Instantiate(spec, container), Release, Get);
                var remainingToReserve = config.Size;

                while (remainingToReserve > 0)
                {
                    var spaceInFrame = _config.MAX_PRE_FRAME - currentFrameCount;
                    var amountToSpawn = Math.Min(remainingToReserve, spaceInFrame);

                    if (amountToSpawn > 0)
                    {
                        pool.Reserv(amountToSpawn);
                        remainingToReserve -= amountToSpawn;
                        currentFrameCount += amountToSpawn;
                    }

                    if (currentFrameCount >= _config.MAX_PRE_FRAME)
                    {
                        await UniTask.NextFrame();
                        currentFrameCount = 0;
                    }
                }

                _pools[config.SpecId] = pool;
            }

            await UniTask.Yield(PlayerLoopTiming.Update);
        }

        public Entity Spawn(long instanceId, long specId)
        {
            var entity = _pools[specId].Get();
            
            entity.Init(instanceId);
            return entity;
        }

        public void Despawn(Entity entity)
        {
            _pools[entity.SpecId].Release(entity);
        }

        private Entity Instantiate(EntitySpecification specification, Transform parent)
        {
            return _factory.Create(specification, Vector3.zero, parent);
        }

        private void Release(Entity entity)
        {
            entity.Deinit();
            entity.gameObject.SetActive(false);
        }

        private void Get(Entity entity, int index)
        {
            entity.gameObject.SetActive(true);
        }
    }
}
