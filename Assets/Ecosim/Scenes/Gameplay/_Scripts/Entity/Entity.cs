using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ecosim
{
    public class Entity : MonoBehaviour
    {
        public long Id { get; private set; }
        public long SpecId { get; private set; }
        public EntityType Type { get; private set; }

        private readonly Dictionary<Type, IEntityComponent> _components = new();
        public EntityBehavior Behavior { get; private set; }

        public bool IsActive { get; private set; }

        public void Init(long id)
        {
            Id = id;
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
            Behavior.EndTask();
        }

        public void Deinit()
        {
            IsActive = false;
            Behavior.EndTask();

            foreach (var component in _components.Values) 
                component.Reset();
        }

        public void Tick(WorldContext context, float deltaTime, float scale)
        {
            if (IsActive) Behavior?.Tick(context, deltaTime, scale);
        }

        public bool Setup(long specId, EntityType type, EntityBehavior behavior)
        {
            SpecId = specId;
            return SetBehavior(behavior) && SetType(type);
        }

        public void AddComponent<T>(T component) where T : IEntityComponent
        {
            _components[typeof(T)] = component;
        }

        public bool RemoveComponent<T>() where T : IEntityComponent
        {
            return _components.Remove(typeof(T));
        }

        public T Get<T>() where T : IEntityComponent
        {
            if (_components.TryGetValue(typeof(T), out IEntityComponent component))
                return (T)component;

            return default;
        }

        public EntitySnapshot GetSnapshot()
        {
            var snapshot = new EntitySnapshot
            {
                SpecId = SpecId,
                InstanceId = Id,
                Position = transform.position,
                Rotation = transform.rotation,
                Task = Behavior.Task?.GetSnapshot()
            };

            foreach (var component in _components.Values)
            {
                snapshot.Components.Add(component.GetSnapshot());
            }

            return snapshot;
        }

        public void Restore(EntitySnapshot snapshot)
        {
            transform.SetPositionAndRotation(snapshot.Position, snapshot.Rotation);

            foreach (var data in snapshot.Components)
            {
                if (_components.TryGetValue(data.ComponentType, out var component))
                    component.Restore(data);
            }
        }

        private bool SetBehavior(EntityBehavior behavior)
        {
            if (Behavior != null) return false;

            Behavior = behavior;
            return true;
        }

        private bool SetType(EntityType type)
        {
            if (Type != EntityType.None) return false;

            Type = type;
            return true;
        }
    }
}
