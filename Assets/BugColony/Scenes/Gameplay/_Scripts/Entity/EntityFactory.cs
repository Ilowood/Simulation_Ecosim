using UnityEngine;

namespace BugColony
{
    public class EntityFactory
    {
        public static Entity Create(Vector3 position, Transform parent, EntitySpecification specifications)
        {
            var gameObject = new GameObject(specifications.Name);
            
            gameObject.transform.SetParent(parent);
            gameObject.transform.position = position;

            var entity = gameObject.AddComponent<Entity>();

            specifications.Configuration.ForEach(spec => spec.Apply(entity));
            entity.SetDefaultInfo(specifications.Type);
            entity.SetBehavior(new EntityBehavior(specifications.Behaviour));

            return entity;
        }
    }
}
