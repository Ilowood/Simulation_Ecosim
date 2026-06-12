namespace Ecosim
{
    public interface IInputDeviceProvider
    {
        void Tick();
        void Sync();

        void OnMenuEnable();
        void OnGameplayEnable();
        void OnEditorEnable();

        float GetAxisValue(ushort actionAx);
        ActionKeyState GetActionKeyState(ushort actionKeyId, InputStartContext mode);
        bool IsActionKeyState(ushort actionKeyId, ActionKeyState state, InputStartContext mode);
        void ExtractActionKey(ushort actionKeyId);
    }

    public enum ActionKeyState : byte
    {
        Pressed,
        Hold,
        Released,
        None
    }

    public class InputActionButtonId
    {
        public const ushort LEFT_CLICK = 0;
        public const ushort RightClick = 1;
        public const ushort CANCEL = 2;
        public const ushort Shift = 3;
        
        public const ushort TotalButtons = 4;
    }

    public static class InputAxisId
    {
        public const int MouseX = 0;
        public const int MouseY = 1;
        public const int MoveX = 2;
        public const int MoveY = 3;

        public const int TotalAxes = 4;
    }
}
