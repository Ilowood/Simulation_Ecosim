using System;

namespace Ecosim
{
    public interface ITaskSnapshot
    {
        IEntityTask CreateTask(WorldContext context, Entity owner);
    }
}
