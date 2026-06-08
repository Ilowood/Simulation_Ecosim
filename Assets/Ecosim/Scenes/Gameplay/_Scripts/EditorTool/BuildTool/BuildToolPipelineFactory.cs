using System;
using System.Collections.Generic;
using Zenject;

namespace Ecosim
{
    public class BuildToolPipelineFactory : IToolFactory<BuildToolParams>
    {
        private IInputDeviceProvider _input;
        private SpawnService _spawnService;
        private World _world;

        [Inject]
        private void Init(World world, SpawnService spawner, IInputDeviceProvider input)
        {
            _world = world;
            _spawnService = spawner;
            _input = input;
        }

        public ToolVariants Variant => ToolVariants.Build;

        public EditorToolPipeline Create(IToolParams parameters)
        {
            if (parameters is BuildToolParams p) 
                return Create(p);
            
            throw new ArgumentException("Invalid params");
        }

        public EditorToolPipeline Create(BuildToolParams parameters)
        {
            var context = new BuildContext
            {
                SpecId = parameters.SpecId, 
            };

            var steps = new Queue<IEditorTool>(3);
            steps.Enqueue(new SpawnTool(context, _spawnService));
            steps.Enqueue(new FollowMouseTool(context, _input));
            steps.Enqueue(new PlaceTool(context, _world));

            return new BuildToolPipeline(steps);
        }
    }
}
