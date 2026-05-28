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

            specifications.Configuration.ForEach(spec => spec.Apply(entity));

            entity.transform.SetParent(parent);
            entity.transform.position = position;

            var behavior = specifications.Behaviour?.Create(entity, _container);
            entity.Setup(specifications.SpecId, specifications.Type, new EntityBehavior(behavior));

            return entity;
        }
    }
}
