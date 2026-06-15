namespace Ecosim
{
    public class PlayerCommandSystem
    {
        private readonly IInputDeviceProvider _input;
        private readonly SelectionBuffer _buffer;

        public PlayerCommandSystem(IInputDeviceProvider input, SelectionBuffer buffer)
        {
            _input = input;
            _buffer = buffer;
        }

        private bool IsActionKeyState(ushort actionKeyId, ActionKeyState state) 
            => _input.IsActionKeyState(actionKeyId, state, InputStartContext.World);

        public void Tick()
        {
            if (!_buffer.CanSelecting)
            {
                if (IsActionKeyState(InputActionButtonId.LEFT_CLICK, ActionKeyState.Released))
                {
                    _input.ExtractActionKey(InputActionButtonId.LEFT_CLICK);
                }
            }
        }
    }
}
