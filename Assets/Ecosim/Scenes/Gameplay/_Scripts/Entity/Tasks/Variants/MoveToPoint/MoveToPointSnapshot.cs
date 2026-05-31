using System;
using UnityEngine;

namespace Ecosim
{
    [Serializable]
    public class MoveToPointSnapshot : ITaskSnapshot
    {
        public MoveToPointParams Params;

        public MoveToPointSnapshot(Vector3 destination)
        {
            Params = new MoveToPointParams { Destination = destination };
        }

        public IEntityTask CreateTask(WorldContext context, Entity owner)
        {
            var p = new MoveToPointParams { 
                Destination = owner.Get<NavigationComponent>().Destination
            };

            return context.TaskFactory.Create(TaskVariants.Move, context, owner, Params);
        } 
    }
}
