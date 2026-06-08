using System;

namespace Ecosim
{
    public interface IEditorTool
    {
        event Action OnCompleted;
        
        void Enter();
        void Tick();
        void Exit();
    }
}
