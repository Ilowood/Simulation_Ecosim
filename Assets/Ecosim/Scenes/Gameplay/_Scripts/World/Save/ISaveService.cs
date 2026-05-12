using System.Collections.Generic;

namespace Ecosim
{
    public interface ISaveService
    {
        void SaveWorld(WorldSnapshot snapshot, string saveName);
        WorldSnapshot LoadWorld(string saveName);
    }
}
