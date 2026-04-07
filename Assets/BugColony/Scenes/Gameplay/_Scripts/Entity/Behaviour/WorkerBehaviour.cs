using UnityEngine;

namespace BugColony
{
    public class WorkerBehaviour : IBehaviour
    {
        private readonly float _speed;
        private readonly int _eatToSplit;
        private readonly float _eatDuration;
        private readonly float _distanceToEat;

        private int _eaten = 0;

        public WorkerBehaviour(float speed, int eatToSplit, float duration, float distanceToEat)
        {
            _speed = speed;
            _eatToSplit = eatToSplit;
            _eatDuration = duration;
            _distanceToEat = distanceToEat;
        }

        public void Tick(Entity entity, SimulationContext context, float deltaTime, float scale)
        {
            if (entity.Behavior.Task == null || entity.Behavior.Task.IsComplete)
            {
                var nearestResource = FindNearestResource(entity, context);

                if (_eaten < _eatToSplit)
                {
                    if (nearestResource != null)
                    {
                        var distance = Vector3.Distance(entity.transform.position, nearestResource.transform.position);
                        if (distance <= _distanceToEat)
                        {
                            entity.Behavior.SetAndStartTask(new EatState(entity, nearestResource, _eatDuration, food => OnEat(food, context)));
                        }
                        else
                        {
                            var targetPosition = nearestResource.transform.position;
                            entity.Behavior.SetAndStartTask(new MoveToTask(entity, nearestResource, targetPosition));
                        }
                    }
                    else
                    {
                        var targetPosition = GetRandomPositionAround(entity.transform.position, 5f);
                        entity.Behavior.SetAndStartTask(new MoveToTask(entity, null, targetPosition));
                    }
                }
                else
                {
                    if (entity.IsDead) return;
                    var countLiveEntities = context.Entities.Get(EntityType.Worker).Count + context.Entities.Get(EntityType.Predator).Count;

                    entity.IsDead = true;
                    context.RemoveEntityWithoutCoolbackCommand(entity);
                    _eaten = 0;

                    if (countLiveEntities > 10)
                    {
                        var mutate = Random.value <= 0.1f;

                        context.SpawnEntityCompand(1, EntityType.Worker);
                        context.SpawnEntityCompand(1, mutate ? EntityType.Predator : EntityType.Worker);
                    }
                    else
                    {
                        context.SpawnEntityCompand(2, EntityType.Worker);
                    }
                }
            }
        }

        private Entity FindNearestResource(Entity entity, SimulationContext context)
        {
            var entities = context.Entities.Get(EntityType.Food);
            if (entities.Count == 0) return null;

            Entity nearest = null;
            var minDistance = float.MaxValue;

            foreach (var resource in entities)
            {
                if (!resource.IsDead)
                {
                    var distance = Vector3.Distance(entity.transform.position, resource.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearest = resource;
                    }
                }
            }

            return nearest;
        }

        private Vector3 GetRandomPositionAround(Vector3 center, float radius)
        {
            var randomOffset = Random.insideUnitSphere * radius;
            randomOffset.y = 0f;
            return center + randomOffset;
        }

        private void OnEat(Entity resource, SimulationContext context)
        {
            if (!resource.IsDead)
            {
                resource.IsDead = true;
                _eaten++;
                context.RemoveEntityWithCoolbackCommand(resource);
            }
        }
    }
}
