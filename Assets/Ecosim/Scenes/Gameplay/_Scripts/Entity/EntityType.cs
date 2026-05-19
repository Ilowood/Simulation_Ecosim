using System;

namespace Ecosim
{
    public enum EntityType
    {
        None = 0,
        Unit = 1 << 0,
        Resource = 2 << 0,
        Building = 3 << 0,
    }
}
