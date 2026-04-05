using UnityEngine;

namespace BugColony
{
    public interface IEntitySpecification
    {
        void Apply(Entity entity);
    }

    public abstract class Specification : ScriptableObject, IEntitySpecification
    {
        public abstract void Apply(Entity entity);
    }
}
