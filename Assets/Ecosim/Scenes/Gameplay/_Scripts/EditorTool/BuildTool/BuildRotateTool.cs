using System;
using UnityEngine;

namespace Ecosim
{
    public class BuildRotateTool : IEditorTool
    {
        public const string TERRAIN_LAYER_NAME = "Environment";

        private readonly BuildContext _context;
        private readonly IInputDeviceProvider _input;
        private readonly Camera _camera;
        private readonly int _layer;

        private Vector3 _startClickWorldPosition;
        private bool _isInitialized;

        public event Action OnCompleted;

        private Vector3 GetCursorWorldPosition
        {
            get
            {
                var mouseX = _input.GetAxisValue(InputAxisId.MouseX);
                var mouseY = _input.GetAxisValue(InputAxisId.MouseY);
                var ray = _camera.ScreenPointToRay(new Vector2(mouseX, mouseY));
                
                return Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _layer) ? hit.point : Vector3.zero;
            }
        }

        public BuildRotateTool(BuildContext context, IInputDeviceProvider input)
        {
            _context = context;
            _input = input;
            _camera = Camera.main;
            _layer = LayerMask.GetMask(TERRAIN_LAYER_NAME);
        }

        public void Enter()
        {
            if (_context.PreviewEntity == null)
            {
                Exit();
                return;
            }

            _startClickWorldPosition = _context.PreviewEntity.transform.position;
            _isInitialized = true;
        }

        public void Tick(float deltaTime, float scale)
        {
            if (!_isInitialized || _context.PreviewEntity == null) return;

            if (_input.IsActionKeyState(InputActionButtonId.LEFT_CLICK, ActionKeyState.Hold, InputStartContext.World))
            {
                var currentMouseWorldPos = GetCursorWorldPosition;
                var direction = currentMouseWorldPos - _startClickWorldPosition;

                if (direction.sqrMagnitude > 0.05f)
                {
                    var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                    _context.PreviewEntity.transform.rotation = Quaternion.Euler(0, targetAngle, 0);
                }
            }

            if (_input.IsActionKeyState(InputActionButtonId.LEFT_CLICK, ActionKeyState.Released, InputStartContext.World))
            {
                _input.ExtractActionKey(InputActionButtonId.LEFT_CLICK);
                Exit();
            }
        }

        public void Exit()
        {
            _isInitialized = false;
            OnCompleted?.Invoke();
        }
    }
}
