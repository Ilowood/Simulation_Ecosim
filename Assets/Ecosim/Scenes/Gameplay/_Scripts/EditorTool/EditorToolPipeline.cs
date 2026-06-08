using System;
using System.Collections.Generic;

namespace Ecosim
{
    public abstract class EditorToolPipeline
    {
        protected Queue<IEditorTool> _steps;

        private IEditorTool _currentStep;
        private bool _isDisposed;

        public event Action OnCompleted;
        public event Action OnCancelled;

        public void NextStep()
        {
            if (_isDisposed) return;

            if (_currentStep != null)
            {
                _currentStep.OnCompleted -= NextStep;
                _currentStep.Exit();
            }

            if (_steps.Count == 0)
            {
                _currentStep = null;
                OnPipelineFinished();
                return;
            }

            _currentStep = _steps.Dequeue();
            _currentStep.OnCompleted += NextStep;
            _currentStep.Enter();
        }

        public void Tick()
        {
            _currentStep?.Tick();
        }

        public virtual void Cancel()
        {
            if (_isDisposed) return;
            _isDisposed = true;

            if (_currentStep != null)
            {
                _currentStep.OnCompleted -= NextStep;
                _currentStep.Exit();
                _currentStep = null;
            }

            _steps.Clear();
            OnCancelled?.Invoke();
        }

        protected virtual void OnPipelineFinished() 
        {
            OnCompleted?.Invoke();
        }
    }
}
