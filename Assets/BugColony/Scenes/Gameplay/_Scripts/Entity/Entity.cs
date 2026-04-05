using UnityEngine;
using UnityEngine.AI;

namespace BugColony
{
    public class Entity : MonoBehaviour
    {
        public EntityType Type { get; private set; }
        public EntityModel Model { get; private set; }
        public EntityBehavior Behavior { get; private set; }
        public NavMeshAgent Agent { get; private set; }

        public void Tick(SimulationContext context, float time)
        {
            Behavior?.Tick(this, context, time);
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
