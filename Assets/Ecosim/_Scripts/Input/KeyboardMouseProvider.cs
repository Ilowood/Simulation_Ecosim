using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace Ecosim
{
    public struct ActionKey
    {
        public bool IsPressed;
        public bool WasPressed;

        public InputStartContext StartContext;
    }

    public enum InputStartContext : byte
    {
        None = 0,
        World = 1,
        Interface = 2
    }

    public class KeyboardMouseProvider : IInputDeviceProvider, EcosimInput.IGameplayActions, EcosimInput.IEditActions, EcosimInput.IMenuActions
    {
        private readonly EcosimInput _input = new();
        private readonly ActionKey[] _stateActionKeys = new ActionKey[InputActionButtonId.TOTAL_BYTTONS];
        private readonly float[] _axies = new float[InputAxisId.TOTAL_AXES];

        public KeyboardMouseProvider()
        {
            _input.Edit.SetCallbacks(this);
            _input.Gameplay.SetCallbacks(this);
            _input.Menu.SetCallbacks(this);
        }

        public bool IsActionKeyState(ushort actionKeyId, ActionKeyState state, InputStartContext mode) => state == GetActionKeyState(actionKeyId, mode);
        public InputStartContext GetActionStartContext(ushort actionKeyId) => _stateActionKeys[actionKeyId].StartContext;
        public float GetAxisValue(ushort actionAxisId) => _axies[actionAxisId];

        public void OnMenuEnable() 
        { 
            _input.Edit.Disable(); 
            _input.Gameplay.Disable(); 
            
            ResetInput(); 
            _input.Menu.Enable(); 
        }
        
        public void OnGameplayEnable() 
        { 
            _input.Edit.Disable(); 
            _input.Menu.Disable(); 
            
            ResetInput(); 
            _input.Gameplay.Enable(); 
        }

        public void OnEditorEnable() 
        { 
            _input.Gameplay.Disable(); 
            _input.Menu.Disable(); 
            
            ResetInput(); 
            _input.Edit.Enable(); 
        }

        public void Sync()
        {
            for (var i = 0; i < _stateActionKeys.Length; i++)
            {
                if (_stateActionKeys[i].IsPressed && _stateActionKeys[i].StartContext == InputStartContext.None)
                {
                    _stateActionKeys[i].StartContext = EventSystem.current.IsPointerOverGameObject() 
                        ? InputStartContext.Interface 
                        : InputStartContext.World;
                }
            }
        }

        public void Tick()
        {
            for (int i = 0; i < _stateActionKeys.Length; i++)
            {
                _stateActionKeys[i].WasPressed = _stateActionKeys[i].IsPressed;

                if (!_stateActionKeys[i].IsPressed)
                    _stateActionKeys[i].StartContext = InputStartContext.None;
            }

            var mousePosition = Mouse.current != null ? Mouse.current.position.ReadValue() : Vector2.zero;
            _axies[InputAxisId.MouseX] = mousePosition.x;
            _axies[InputAxisId.MouseY] = mousePosition.y;
        }

        public ActionKeyState GetActionKeyState(ushort actionKeyId, InputStartContext mode)
        {
            var actionKey = _stateActionKeys[actionKeyId];
            if (actionKey.StartContext != mode) return ActionKeyState.None;

            return (actionKey.IsPressed, actionKey.WasPressed) switch
            {
                (true, false) => ActionKeyState.Pressed,
                (true, _)     => ActionKeyState.Hold,
                (false, true) => ActionKeyState.Released,
                _             => ActionKeyState.None
            };
        }

        public void ExtractActionKey(ushort actionKeyId)
        {
            _stateActionKeys[actionKeyId].IsPressed = false;
            _stateActionKeys[actionKeyId].WasPressed = false;
            _stateActionKeys[actionKeyId].StartContext = InputStartContext.None;
        }

        private void ButtonPhase(InputActionPhase phase, ushort actionButtonId)
        {
            switch (phase)
            {
                case InputActionPhase.Started:  _stateActionKeys[actionButtonId].IsPressed = true;  break;
                case InputActionPhase.Canceled: _stateActionKeys[actionButtonId].IsPressed = false; break;
            }
        }

        private void ResetInput()
        {
            for (var i = 0; i < _stateActionKeys.Length; i++)
            {
                _stateActionKeys[i].IsPressed = false;
                _stateActionKeys[i].WasPressed = false;
                _stateActionKeys[i].StartContext = InputStartContext.None;
            }
        }

        public void OnLeftClick(CallbackContext context)
        {
            ButtonPhase(context.phase, InputActionButtonId.LEFT_CLICK);
        }

        public void OnRightClick(CallbackContext context)
        {
            ButtonPhase(context.phase, InputActionButtonId.RIGHT_CLICK);
        }

        public void OnMove(CallbackContext context) 
        { 
            var delta = context.ReadValue<Vector2>(); 
            _axies[InputAxisId.MoveX] = delta.x; 
            _axies[InputAxisId.MoveY] = delta.y; 
        }
        
        public void OnPause(CallbackContext context) 
        { 
            ButtonPhase(context.phase, InputActionButtonId.CANCEL);
        }
        
        public void OnResume(CallbackContext context) 
        {
            ButtonPhase(context.phase, InputActionButtonId.CANCEL);
        }

        public void OnAccumulate(CallbackContext context)
        {
            ButtonPhase(context.phase, InputActionButtonId.ACCUMULATE);
        }
    }
}
