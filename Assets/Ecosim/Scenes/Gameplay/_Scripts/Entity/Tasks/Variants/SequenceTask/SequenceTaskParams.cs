using System;

namespace Ecosim
{
    [Serializable]
    public struct SequenceTaskParams : ITaskParams
    {
        TaskVariants ITaskParams.Variants => TaskVariants.SequenceTask;

        public ITaskSnapshot[] Tasks;
    }
}
