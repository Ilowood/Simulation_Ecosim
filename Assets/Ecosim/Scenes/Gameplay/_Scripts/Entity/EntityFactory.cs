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

            gameObject.transform.SetParent(parent);
            gameObject.transform.position = position;

            specifications.Configuration.ForEach(spec => spec.Apply(entity));
            entity.SetDefaultInfo(specifications.Type);
            entity.SetBehavior(new EntityBehavior(specifications.Behaviour));

            return entity;
        }
    }
}
