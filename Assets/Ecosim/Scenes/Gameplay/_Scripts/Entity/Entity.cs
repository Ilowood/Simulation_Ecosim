using UnityEngine;
using UnityEngine.AI;

namespace Ecosim
{
    public class Entity : MonoBehaviour
    {
        public bool IsDead { get; set; }
        public EntityType Type { get; private set; }
        public EntityModel Model { get; private set; }
        public EntityBehavior Behavior { get; private set; }
        public NavMeshAgent Agent { get; private set; }

        public void Tick(SimulationContext context, float deltaTime, float scale)
        {
            Behavior?.Tick(this, context, deltaTime, scale);
        }

        public bool SetDefaultInfo(EntityType type)
        {
            Type = type;
            return true;
        }

        public bool SetModel3D(EntityModel model)
        {
            if (!IsNullComponent(Model)) return false;

            Model = model;
            return true;
        }

        public bool SetBehavior(EntityBehavior behavior)
        {
            if (!IsNullComponent(Behavior)) return false;

            Behavior = behavior;
            return true;
        }

        public bool SetNavMeshAgent(NavMeshAgent agent)
        {
            if (!IsNullComponent(Agent)) return false;

            Agent = agent;
            return true;
        }

        private bool IsNullComponent<T>(T obj) where T : class
        {
            return obj == default ? true : false;
        }
    }
}
