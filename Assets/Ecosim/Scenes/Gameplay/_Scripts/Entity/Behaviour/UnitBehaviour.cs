using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Ecosim
{
    public class UnitBehaviour : IBehaviour
    {
        private readonly long _specIdWarehouses;
        private readonly long _specIdTree;
        private readonly long _specIdWood;

        private readonly StorageSystem _storageSystem;

        private readonly Entity _owner;
        private StorageComponent _storageOwner;

        private const float SearchRadius = 20f;
        
        public UnitBehaviour(Entity owner, StorageSystem storageSystem, long specIdWarehouses, long specIdTree, long specIdWood)
        {
            _owner = owner;
            _storageSystem = storageSystem;
            _storageOwner = _owner.Get<StorageComponent>();

            _specIdWarehouses = specIdWarehouses;
            _specIdTree = specIdTree;
            _specIdWood = specIdWood;
        }

        public void Tick(WorldContext context, float deltaTime, float scale)
        {
            if (_owner.Behavior.Task == null || _owner.Behavior.Task.IsComplete)
            {
                if (!_storageSystem.IsAllSlotsReserved(_storageOwner))
                {
                    var trees = context.Registry.GetBySpecId(_specIdTree);
                    var closestTree = FindClosestEntity(trees);

                    if (closestTree != null)
                    {
                        var targetPos = closestTree.transform.position;
                        _owner.Behavior.SetAndStartTask(new MoveToPointTask(_owner, targetPos));
                    }
                    else
                    {
                        MoveToRandomPoint();
                    }
                }
                else
                {
                    var warehouses = context.Registry.GetBySpecId(_specIdWarehouses);
                    var closestWarehouse = FindClosestEntity(warehouses);

                    if (closestWarehouse != null)
                    {
                        if (Vector3.Distance(closestWarehouse.transform.position, _owner.transform.position) > 1f)
                        {
                            _owner.Behavior.SetAndStartTask(new MoveToPointTask(_owner, closestWarehouse.transform.position));
                            return;
                        }
                        
                        if (_storageSystem.HasItem(_storageOwner, _specIdWood))
                        {
                            var count = _storageSystem.GetItemCount(_storageOwner, _specIdWood);

                            _owner.Behavior.SetAndStartTask(new ResourceTransferTask(
                                _storageSystem, 
                                _owner, 
                                closestWarehouse, 
                                _specIdWood, 
                                count, 
                                0));
                        }
                    }
                    else
                    {
                        MoveToRandomPoint();
                    }
                }
            }
        }

        private void MoveToRandomPoint()
        {
            var randomPos = GetRandomPoint(_owner.transform.position, SearchRadius);
            _owner.Behavior.SetAndStartTask(new MoveToPointTask(_owner, randomPos));
        }

        private Entity FindClosestEntity(IEnumerable<Entity> entities)
        {
            Entity closest = null;
            
            var minSqrDistance = float.MaxValue;
            var currentPos = _owner.transform.position;

            foreach (var entity in entities)
            {
                var direction = entity.transform.position - currentPos;
                var sqrDistance = direction.sqrMagnitude;

                if (sqrDistance < minSqrDistance)
                {
                    minSqrDistance = sqrDistance;
                    closest = entity;
                }
            }

            return closest;
        }

        private Vector3 GetRandomPoint(Vector3 center, float radius)
        {
            var randomCircle = Random.insideUnitCircle * radius;
            var direction = new Vector3(randomCircle.x, 0, randomCircle.y);
            var targetPos = center + direction;

            if (NavMesh.SamplePosition(targetPos, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
            {
                return hit.position;
            }

            return center;
        }
    }
}
