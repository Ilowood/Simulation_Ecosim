using System.Collections.Generic;
using Zenject;

namespace Ecosim
{
    public class ToolFactory
    {
        private readonly Dictionary<ToolVariants, IToolFactory> _factories = new();

        [Inject]
        public void Init(List<IToolFactory> factories)
        {
            foreach (var factory in factories)
                _factories[factory.Variant] = factory;
        }

        public EditorToolPipeline Create(ToolVariants variant, IToolParams parameters)
        {
            if (_factories.TryGetValue(variant, out var factory))
            {
                return factory.Create(parameters);
            }

            throw new System.Exception($"No factory for variant: {variant}");
        }
    }
}
