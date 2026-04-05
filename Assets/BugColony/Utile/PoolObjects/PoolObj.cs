using System;
using System.Collections.Generic;

namespace Untils
{
    public class PoolObj<T>
    {
        public List<T> poolObj { get; private set; } = new List<T>();
        private Queue<T> poolDisabledObj = new Queue<T>();

        private Func<T> generationEvent;
        private Action<T> releaseEvent;
        private Action<T, int> getEvent;

        public PoolObj(Func<T> generation, Action<T> release, 
            Action<T, int> get, int countObj) 
        {
            generationEvent += generation;
            releaseEvent += release;
            getEvent += get;

            GenerationObjects(countObj);
        }

        private void GenerationObjects(int countObjs)
        {
            for (var i = 0;  i < countObjs; i++) 
            {
                var obj = generationEvent();
                poolObj.Add(obj);
                Release(obj); 
            }
        }

        public void Release(T obj)
        {
            releaseEvent(obj);
            poolDisabledObj.Enqueue(obj);
        }

        public List<T> Get(int countObjs)
        {
            var objs = new List<T>();

            for (var i = 0; i < countObjs; i++)
            {
                objs.Add(poolDisabledObj.Count > 0 ? poolDisabledObj.Dequeue() : generationEvent());
                getEvent(objs[i], i);
            }

            return objs;
        }
    }
}
