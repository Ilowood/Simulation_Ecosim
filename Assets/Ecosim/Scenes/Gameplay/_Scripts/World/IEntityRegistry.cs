using System.Collections.Generic;

namespace Ecosim
{
    public interface IEntityRegistry
    {
        IReadOnlyCollection<Entity> GetBySpecId(long specId);
        Entity GetById(long instanceId);
        
        int GetCount(long specId);
    }
}
