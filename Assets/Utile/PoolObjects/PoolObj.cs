using System;
using System.Collections.Generic;

namespace Utils
{
    public class PoolObj<T>
    {
        public List<T> _poolObj { get; private set; } = new List<T>();
        private Queue<T> _poolDisabledObj = new Queue<T>();

        private Func<T> _generateEvent;
        private Action<T> _releaseEvent;
        private Action<T, int> _getEvent;

        public PoolObj(Func<T> generate, Action<T> release, Action<T, int> get) 
        {
            _generateEvent += generate;
            _releaseEvent += release;
            _getEvent += get;
        }

        public void Reserv(int countObjs)
        {
            for (var i = 0;  i < countObjs; i++) 
            {
                var obj = _generateEvent();
                _poolObj.Add(obj);
                Release(obj); 
            }
        }

        public void Release(T obj)
        {
            _releaseEvent(obj);
            _poolDisabledObj.Enqueue(obj);
        }

        public T Get()
        {
            var obj = _poolDisabledObj.Count > 0 ? _poolDisabledObj.Dequeue() : CreateNew();
            _getEvent?.Invoke(obj, 0);
            return obj;
        }

        public List<T> Get(int count)
        {
            var result = new List<T>(count);

            for (var i = 0; i < count; i++)
            {
                var obj = Get();
                _getEvent?.Invoke(obj, i); 
                result.Add(obj);
            }

            return result;
        }

        private T CreateNew()
        {
            var obj = _generateEvent();
            _poolObj.Add(obj);
            return obj;
        }
    }
}
