using System;

namespace Ecosim
{
    public class BuildPlacementTool : IEditorTool
    {
        public event Action OnCompleted;

        private readonly BuildContext _context;
        private readonly World _world;

        public BuildPlacementTool(BuildContext context, World world)
        {
            _context = context;
            _world = world;
        }

        public void Enter()
        {
            _world.AddEntity(_context.PreviewEntity);
            _context.IsConfirmed = true;
            _context.PreviewEntity = null;

            OnCompleted?.Invoke();
        }

        public void Tick(float deltaTime, float scale) { }

        public void Exit() { }
    }
}
