using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace Ecosim
{
    public class KeyboardMouseProvider : IInputDeviceProvider, IDisposable, EcosimInput.IGameplayActions, EcosimInput.IMenuActions, EcosimInput.IEditActions
    {
        private EcosimInput _input;

        public Vector2 CursorPosition => Mouse.current.position.ReadValue();

        public KeyboardMouseProvider()
        {
            _input = new EcosimInput();

            _input.Edit.SetCallbacks(this);
            _input.Gameplay.SetCallbacks(this);
            _input.Menu.SetCallbacks(this);
        }

        public event Action<PointerInputEvent> OnWorldLayerLeftMousePress;
        public event Action<PointerInputEvent> OnWorldLayerLeftMouseRelease;
        public event Action<PointerInputEvent> OnToolLayerLeftMousePress;
        public event Action<PointerInputEvent> OnToolLayerLeftMouseRelease;

        public event Action<Vector2> OnMoveEvent;
        public event Action OnPauseEvent;
        public event Action OnResumeEvent;

        public void OnMenuEnable()
        {
            _input.Menu.Enable();
            _input.Edit.Disable();
            _input.Gameplay.Disable();
        }
        
        public void OnGameplayEnable()
        {
            _input.Gameplay.Enable();
            _input.Edit.Disable();
            _input.Menu.Disable();
        }

        public void OnEditorEnable()
        {
            _input.Edit.Enable();
            _input.Gameplay.Disable();
            _input.Menu.Disable();
        }
        
        public void Dispose()
        {
            _input.Dispose();
        }

        void EcosimInput.IGameplayActions.OnLeftClick(CallbackContext context)
        {
            var inputEvent = new PointerInputEvent(CursorPosition);

            switch (context.phase)
            {
                case InputActionPhase.Started: OnWorldLayerLeftMousePress?.Invoke(inputEvent); break;
                case InputActionPhase.Canceled: OnWorldLayerLeftMouseRelease?.Invoke(inputEvent); break;
            }
        }

        void EcosimInput.IEditActions.OnLeftClick(CallbackContext context)
        {
            var inputEvent = new PointerInputEvent(CursorPosition);

            switch (context.phase)
            {
                case InputActionPhase.Started: OnToolLayerLeftMousePress?.Invoke(inputEvent); break;
                case InputActionPhase.Canceled: OnToolLayerLeftMouseRelease?.Invoke(inputEvent); break;
            }
        }

        void EcosimInput.IGameplayActions.OnMove(CallbackContext context)
        {
            OnMoveEvent?.Invoke(context.ReadValue<Vector2>());
        }

        void EcosimInput.IGameplayActions.OnPause(CallbackContext context)
        {
            if (context.canceled) OnPauseEvent?.Invoke();
        }

        void EcosimInput.IMenuActions.OnResume(CallbackContext context)
        {
            if (context.canceled) OnResumeEvent?.Invoke();
        }
    }
}
