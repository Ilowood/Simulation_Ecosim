using System;
using UnityEngine;

namespace Ecosim
{
    public class PointerInputEvent
    {
        public readonly Vector2 CursorPosition;

        public PointerInputEvent(Vector2 cursorPosition)
        {
            CursorPosition = cursorPosition;
        }
    }

    public interface IInputDeviceProvider
    {
        Vector2 CursorPosition { get; }

        event Action<PointerInputEvent> OnWorldLayerLeftMousePress;
        // event Action<PointerInputEvent> OnWorldLayerLeftMouseHold;
        event Action<PointerInputEvent> OnWorldLayerLeftMouseRelease;

        event Action<PointerInputEvent> OnToolLayerLeftMousePress;
        // event Action<PointerInputEvent> OnToolLayerLeftMouseHold;
        event Action<PointerInputEvent> OnToolLayerLeftMouseRelease;
        
        event Action<Vector2> OnMoveEvent;
        event Action OnPauseEvent;
        event Action OnResumeEvent;
        
        void OnMenuEnable();
        void OnGameplayEnable();
        void OnEditorEnable();
    }
}
