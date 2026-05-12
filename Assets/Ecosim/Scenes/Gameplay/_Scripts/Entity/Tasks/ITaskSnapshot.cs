using System;

namespace Ecosim
{
    public interface ITaskSnapshot
    {
        IEntityTask CreateTask(Entity root);
    }
}
