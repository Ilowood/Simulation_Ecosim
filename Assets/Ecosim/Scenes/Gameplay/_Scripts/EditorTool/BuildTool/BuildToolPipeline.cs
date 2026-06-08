using System.Collections.Generic;

namespace Ecosim
{
    public class BuildToolPipeline : EditorToolPipeline 
    {
        public BuildToolPipeline(Queue<IEditorTool> steps) 
        {
            _steps = steps;
        }
    }
}
