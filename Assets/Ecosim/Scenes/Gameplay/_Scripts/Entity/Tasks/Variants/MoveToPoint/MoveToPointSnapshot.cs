using System;

namespace Ecosim
{
    [Serializable]
    public class MoveToPointSnapshot : ITaskSnapshot
    {
        public IEntityTask CreateTask(WorldContext context, Entity owner)
        {
            var p = new MoveToPointParams { 
                Destination = owner.Get<NavigationComponent>().Destination
            };

            return context.TaskFactory.Create(TaskVariants.Move, owner, p);
        } 
    }
}
