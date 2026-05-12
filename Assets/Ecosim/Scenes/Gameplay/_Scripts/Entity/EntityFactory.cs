using System;
using UnityEngine;
using Zenject;

namespace Ecosim
{
    public class EntityFactory
    {
        private readonly DiContainer _container;

        public EntityFactory(DiContainer container)
        {
            _container = container;
        }

        public Entity Create(EntitySpecification specifications, Vector3 position, Transform parent)
        {
            var gameObject = new GameObject(specifications.Name);
            var entity = _container.InstantiateComponent<Entity>(gameObject);
            
            entity.Setup(specifications.Id, specifications.Type, new EntityBehavior(specifications.Behaviour));
            entity.transform.SetParent(parent);
            entity.transform.position = position;

            specifications.Configuration.ForEach(spec => spec.Apply(entity));

            return entity;
        }
    }
}
