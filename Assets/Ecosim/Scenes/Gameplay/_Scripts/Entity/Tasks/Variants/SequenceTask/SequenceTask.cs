using System.Collections.Generic;

namespace Ecosim
{
    public class SequenceTask : IEntityTask
    {
        private readonly Queue<IEntityTask> _tasks = new();
        
        private IEntityTask _currentTask;
        private bool _isComplete;

        public bool IsComplete => _isComplete;
        public TaskVariants Variants => TaskVariants.SequenceTask;

        public SequenceTask(Queue<IEntityTask> tasks)
        {
            _tasks = tasks;
        }

        public void Puase() => _currentTask.Puase();
        public void Resume() => _currentTask.Resume();

        public void Start()
        {
            if (_tasks.Count > 0) _currentTask = _tasks.Dequeue();
            _currentTask?.Start();
        }

        public void Tick(float deltaTime, float scale)
        {
            if (_isComplete) return;

            _currentTask.Tick(deltaTime, scale);

            if (_currentTask.IsComplete)
            {
                if (_tasks.Count > 0)
                {
                    _currentTask = _tasks.Dequeue();
                    _currentTask.Start();
                }
                else
                {
                    End();
                }
            }
        }

        public void End()
        {
            _currentTask?.End();
            _isComplete = true;
            _currentTask = null;
        }

        public ITaskSnapshot GetSnapshot()
        {
            var deferredTasks = new ITaskSnapshot[_tasks.Count + 1];
            deferredTasks[0] = _currentTask.GetSnapshot();

            var i = 1;
            foreach (var task in _tasks)
            {
                deferredTasks[i++] = task.GetSnapshot();
            }

            return new SequenceTaskSnapshot(deferredTasks);
        }
    }
}
