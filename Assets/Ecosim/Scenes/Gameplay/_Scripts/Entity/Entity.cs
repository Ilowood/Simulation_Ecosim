using UnityEngine;

namespace Ecosim
{
    public class Entity : MonoBehaviour
    {
        public bool IsDead { get; set; }
        public EntityType Type { get; private set; } = EntityType.None;
        public EntityBehavior Behavior { get; private set; }

        public void Tick(SimulationContext context, float deltaTime, float scale)
        {
            Behavior?.Tick(this, context, deltaTime, scale);
        }

        public bool SetDefaultInfo(EntityType type)
        {
            if (Type != EntityType.None) return false;

            Type = type;
            return true;
        }

        public bool SetBehavior(EntityBehavior behavior)
        {
            if (Behavior != default) return false;

            Behavior = behavior;
            return true;
        }
    }
}
