using System;
using UnityEngine;

namespace Ecosim
{
    public class FollowMouseTool : IEditorTool
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
                var ray = _camera.ScreenPointToRay(_input.CursorPosition);
                return Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _layer) ? hit.point : Vector3.zero;
            }
        }

        public FollowMouseTool(BuildContext context, IInputDeviceProvider input)
        {
            _context = context;
            _input = input;
            _camera = Camera.main;
            _layer = LayerMask.GetMask(TERRAIN_LAYER_NAME);
        }

        public event Action OnCompleted;

        public void Enter()
        {
            _input.OnToolLayerLeftMouseRelease += Exit;
        }

        public void Tick()
        {
            _context.PreviewEntity.transform.position = GetCursorWorldPosition;
        }

        public void Exit()
        {
            _input.OnToolLayerLeftMouseRelease -= Exit;
            OnCompleted?.Invoke();
        }

        private void Exit(PointerInputEvent inputEvent)
        {
            var canBuild = ValidatePosition(GetCursorWorldPosition);
            UpdatePreviewVisuals(canBuild);

            if (canBuild)
            {
                Exit();  
            }
        }

        private bool ValidatePosition(Vector3 position) => true;
        
        private void UpdatePreviewVisuals(bool canBuild)
        {
            
        }
    }
}
