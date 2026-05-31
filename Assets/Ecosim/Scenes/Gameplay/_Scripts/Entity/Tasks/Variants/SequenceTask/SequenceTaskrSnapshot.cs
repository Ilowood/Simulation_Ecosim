using System;

namespace Ecosim
{
    [Serializable]
    public class SequenceTaskSnapshot : ITaskSnapshot
    {
        public SequenceTaskParams Params;

        public SequenceTaskSnapshot(ITaskSnapshot[] tasks)
        {
           Params = new SequenceTaskParams
           {
               Tasks = tasks 
           };
        }

        public IEntityTask CreateTask(WorldContext context, Entity owner)
        {            
            return context.TaskFactory.Create(TaskVariants.SequenceTask, context, owner, Params);
        }
    }
}
