using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ecosim
{
    public class Entity : MonoBehaviour
    {
        public Guid Id { get; private set; }
        public EntityType Type { get; private set; }
        public string Name { get; private set; }

        private readonly Dictionary<Type, IEntityComponent> _components = new();
        public EntityBehavior Behavior { get; private set; }

        public bool IsActive { get; private set; }

        public void Init(Guid id, string name)
        {
            Id = id;
            Name = name;
            IsActive = true;
        }

        public void Deinit()
        {
            Name = string.Empty;
            IsActive = false;

            foreach (var component in _components.Values) 
                component.Reset();
        }

        public bool Setup(EntityType type, EntityBehavior behavior)
        {
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

        public void Tick(SimulationContext context, float deltaTime, float scale)
        {
            Behavior?.Tick(this, context, deltaTime, scale);
        }

        public T GetData<T>() where T : IEntityComponent
        {
            if (_components.TryGetValue(typeof(T), out IEntityComponent component))
                return (T)component;

            return default;
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
