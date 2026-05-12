using System.Collections.Generic;

namespace Untils
{
    public class IdGenerator
    {
        private long _lastId = 0;
        private List<long> _freeIds = new();

        public long GetNext()
        {
            if (_freeIds.Count > 0)
            {
                long id = _freeIds[0];
                _freeIds.RemoveAt(0);
                return id;
            }
            return ++_lastId;
        }

        public void Release(long id)
        {
            if (!_freeIds.Contains(id)) _freeIds.Add(id);
        }

        public IdGeneratorSnapshot GetSnapshot() 
        {
            return new IdGeneratorSnapshot(_lastId, _freeIds);
        }

        public void Restore(IdGeneratorSnapshot snapshot)
        {
            _lastId = snapshot.LastId;
            _freeIds = new List<long>(snapshot.FreeIds);
        }

        public void Reset()
        {
            _lastId = 0;
            _freeIds.Clear();
        }
    }
}
