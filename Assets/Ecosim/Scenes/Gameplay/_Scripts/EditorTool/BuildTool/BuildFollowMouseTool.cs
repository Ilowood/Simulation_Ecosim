using System;
using UnityEngine;

namespace Ecosim
{
    public class BuildFollowMouseTool : IEditorTool
    {
        public const string TERRAIN_LAYER_NAME = "Environment";

        private readonly BuildContext _context;
        private readonly IInputDeviceProvider _input;
        private readonly Camera _camera;
        private readonly int _layer;

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

        public BuildFollowMouseTool(BuildContext context, IInputDeviceProvider input)
        {
            _context = context;
            _input = input;
            _camera = Camera.main;
            _layer = LayerMask.GetMask(TERRAIN_LAYER_NAME);
        }

        public event Action OnCompleted;

        public void Enter() { }

        public void Tick(float deltaTime, float scale)
        {
            var worldPosition = GetCursorWorldPosition;

            if (_context.PreviewEntity != null)
            {
                _context.PreviewEntity.transform.position = worldPosition;
            }

            var canBuild = ValidatePosition(worldPosition);
            UpdatePreviewVisuals(canBuild);
            
            if (_input.IsActionKeyState(InputActionButtonId.LEFT_CLICK, ActionKeyState.Pressed, InputStartContext.World) && canBuild)
                Exit();
        }

        public void Exit()
        {
            OnCompleted?.Invoke();
        }

        private bool ValidatePosition(Vector3 position) => true;
        
        private void UpdatePreviewVisuals(bool canBuild)
        {
            
        }
    }
}
