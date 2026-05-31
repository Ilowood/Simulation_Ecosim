using System;
using System.Collections.Generic;
using Zenject;

namespace Ecosim
{
    public class SequenceTaskrFactory : ITaskFactory<SequenceTaskParams>
    {
        private TaskFactory _factory;

        [Inject]
        private void Init(TaskFactory factory)
        {
            _factory = factory;
        }

        public TaskVariants Variant => TaskVariants.SequenceTask;

        public IEntityTask Create(WorldContext context, Entity owner, ITaskParams parameters)
        {
            if (parameters is SequenceTaskParams p) 
                return Create(context, owner, p);
            
            throw new ArgumentException("Invalid params");
        }

        public IEntityTask Create(WorldContext context, Entity owner, SequenceTaskParams parameters)
        {
            var queue = new Queue<IEntityTask>();

            foreach (var snapshot in parameters.Tasks)
            {
                if (snapshot == null) continue;
                
                var task = snapshot.CreateTask(context, owner);
                queue.Enqueue(task);
            }
            
            return new SequenceTask(queue);
        }
    }
}
