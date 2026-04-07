using System;
using System.Linq;
using UnityEngine;

namespace BugColony
{
    public class PredatorBehaviour : IBehaviour
    {
        private readonly float _lifeTimeInSeconds;
        private readonly float _attackDistance;
        private readonly float _attackDuration;
        private readonly float _stunDurationTimer;
        private readonly float _speed;
        private readonly int _eatToSplit;

        private int _eaten = 0;
        private float _timer;

        public PredatorBehaviour(int eatToSplit, float lifeTimeInSeconds, float attackDuration, float attackDistance, float stunDurationTimer, float speed)
        {
            _eatToSplit = eatToSplit;
            _lifeTimeInSeconds = lifeTimeInSeconds;
            _attackDistance = attackDistance;
            _attackDuration = attackDuration;
            _stunDurationTimer = stunDurationTimer;
            _speed = speed;
            _timer = 0;
        }

        public void Tick(Entity entity, SimulationContext context, float deltaTime)
        {
            if (_timer > _lifeTimeInSeconds)
            {
                Death(entity, context);
                return;
            }

            if (entity.Behavior.Task == null || entity.Behavior.Task.IsComplete)
            {
                if (_eaten < _eatToSplit)
                {
                    var target = FindNearestFromRandomType(entity, context);

                    if (target != null)
                    {
                        var distance = Vector3.Distance(entity.transform.position, target.transform.position);

                        if (distance <= _attackDistance)
                        {
                            target.Behavior.SetAndStartTask(new StunTask(target, _stunDurationTimer));
                            entity.Behavior.SetAndStartTask(new AttackTask(entity, target, _attackDistance, _attackDuration, entity => OnKill(entity, context)));
                        }
                        else
                        {
                            entity.Behavior.SetAndStartTask(new ChaseTargetTask(entity, target, _speed));
                        }
                    }
                    else
                    {
                        var roamPos = GetRandomRoamPosition(entity.transform.position, 10f);
                        entity.Behavior.SetAndStartTask(new MoveToTask(entity, null, roamPos, _speed * 0.5f));
                    }
                }
                else
                {
                    Death(entity, context);
                    context.SpawnEntityCompand(2, EntityType.Predator);
                }
            }

            _timer += deltaTime;
        }

        private void Death(Entity entity, SimulationContext context)
        {
                entity.IsDead = true;
                entity.Behavior.EndTask();
                _timer = 0;
                _eaten = 0;
                context.RemoveEntityWithCoolbackCommand(entity);
        }

        private Entity FindNearestFromRandomType(Entity self, SimulationContext context)
        {
            var types = Enum.GetValues(typeof(EntityType))
                .Cast<EntityType>()
                .OrderBy(x => Guid.NewGuid())
                .ToList();

            foreach (var type in types)
            {
                var entities = context.Entities.Get(type);
                
                Entity nearest = null;
                var minDistance = float.MaxValue;

                for (int i = 0; i < entities.Count; i++)
                {
                    var target = entities[i];
                    if (target == self || target.IsDead) continue;

                    var dist = Vector3.Distance(self.transform.position, target.transform.position);
                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        nearest = target;
                    }
                }

                if (nearest != null)
                {
                    return nearest;
                }
            }

            return null;
        }

        private void OnKill(Entity victim, SimulationContext context)
        {
            if (!victim.IsDead)
            {
                victim.IsDead = true;
                _eaten++;
                context.RemoveEntityWithCoolbackCommand(victim);
            }
        }

        private Vector3 GetRandomRoamPosition(Vector3 center, float radius)
        {
            var offset = UnityEngine.Random.insideUnitSphere * radius;
            offset.y = 0;
            return center + offset;
        }
    }
}
