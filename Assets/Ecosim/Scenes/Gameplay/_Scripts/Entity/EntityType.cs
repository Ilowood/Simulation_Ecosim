using System;

namespace Ecosim
{
    [Flags]
    public enum EntityType
    {
        None = 0,
        Unit = 1 << 0,
        Tree = 1 << 1,
        Warehouse = 2 << 2,
    }
}
