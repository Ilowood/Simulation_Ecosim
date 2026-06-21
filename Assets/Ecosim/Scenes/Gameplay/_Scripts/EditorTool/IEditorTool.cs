using System;

namespace Ecosim
{
    public interface IEditorTool : ITicable
    {
        event Action OnCompleted;
        
        void Enter();
        void Exit();
    }
}
