using System.Collections.Generic;
using Zenject;

namespace Ecosim
{
    public class TaskFactory
    {
        private readonly Dictionary<TaskVariants, ITaskFactory> _factories = new();

        [Inject]
        public void Init(List<ITaskFactory> factories)
        {
            foreach (var factory in factories)
                _factories[factory.Variant] = factory;
        }

        public IEntityTask Create(TaskVariants variant, WorldContext context, Entity owner, ITaskParams parameters)
        {
            if (_factories.TryGetValue(variant, out var factory))
                return factory.Create(context, owner, parameters);

            throw new System.Exception($"No factory for variant: {variant}");
        }
    }
}
