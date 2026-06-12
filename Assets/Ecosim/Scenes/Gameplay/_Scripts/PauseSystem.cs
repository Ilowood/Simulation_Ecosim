using System;

namespace Ecosim
{
    public class PauseSystem
    {
        private readonly IInputDeviceProvider _input;
        private readonly Action _onPauseRequested;

        public PauseSystem(IInputDeviceProvider input, Action onPauseRequested)
        {
            _input = input;
            _onPauseRequested = onPauseRequested;
        }

        public void Tick()
        {
            if (_input.IsActionKeyState(InputActionButtonId.CANCEL, ActionKeyState.Pressed, InputStartContext.Interface))
            {
                _input.ExtractActionKey(InputActionButtonId.CANCEL);
                _onPauseRequested?.Invoke();
            }
        }
    }
}
