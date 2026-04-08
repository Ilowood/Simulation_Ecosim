using System;

namespace Ecosim
{
    [Flags]
    public enum EntityType
    {
        None = 0,
        Worker = 1 << 0,
        Predator = 1 << 1,
        Resource = 2 << 2,
    }
}
