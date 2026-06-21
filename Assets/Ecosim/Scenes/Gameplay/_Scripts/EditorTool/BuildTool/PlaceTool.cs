using System;

namespace Ecosim
{
    public class PlaceTool : IEditorTool
    {
        public event Action OnCompleted;

        private readonly BuildContext _context;
        private readonly World _world;

        public PlaceTool(BuildContext context, World world)
        {
            _context = context;
            _world = world;
        }

        public void Enter()
        {
            ConfigureAsReady(_context.PreviewEntity);

            _world.AddEntity(_context.PreviewEntity);
            _context.IsConfirmed = true;
            _context.PreviewEntity = null;

            OnCompleted?.Invoke();
        }

        public void Tick(float deltaTime, float scale) { }
        public void Exit() { }

        private void ConfigureAsReady(Entity entity)
        {
            
        }
    }
}
