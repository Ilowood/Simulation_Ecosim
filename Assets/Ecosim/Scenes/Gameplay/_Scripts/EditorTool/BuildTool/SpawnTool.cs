using System;

namespace Ecosim
{
    public class SpawnTool : IEditorTool
    {        
        private readonly BuildContext _context;
        private readonly SpawnService _spawner;

        public SpawnTool(BuildContext context, SpawnService spawner)
        {
            _context = context;
            _spawner = spawner;
        }

        public event Action OnCompleted;

        public void Enter()
        {
            _context.PreviewEntity = _spawner.Spawn(_context.SpecId);
            ConfigureAsPreview(_context.PreviewEntity);

            OnCompleted?.Invoke();
        }

        public void Tick(float deltaTime, float scale) { }
        public void Exit() { }

        private void ConfigureAsPreview(Entity entity)
        {
            
        }
    }
}
