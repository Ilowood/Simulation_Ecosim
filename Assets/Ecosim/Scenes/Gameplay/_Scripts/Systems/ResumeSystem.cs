using System;

namespace Ecosim
{
    public class ResumeSystem : ITicable
    {
        private readonly IInputDeviceProvider _input;
        private readonly Action _onResumeRequested;

        public ResumeSystem(IInputDeviceProvider input, Action onResumeRequested)
        {
            _input = input;
            _onResumeRequested = onResumeRequested;
        }

        public void Tick(float deltaTime, float scale)
        {
            if (_input.IsActionKeyState(InputActionButtonId.CANCEL, ActionKeyState.Pressed, InputStartContext.Interface))
            {
                _input.ExtractActionKey(InputActionButtonId.CANCEL);
                _onResumeRequested?.Invoke();
            }
        }
    }
}
