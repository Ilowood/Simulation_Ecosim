using System.Linq;
using UnityEngine;

namespace Ecosim
{
    public class SelectionSystem
    {
        private const float DRAG_THRESHOLD = 10f; 
        
        private readonly IInputDeviceProvider _input;
        private readonly EntityRegistry _registry;
        private readonly Camera _camera;
        private readonly SelectionBuffer _buffer;
        private readonly int _entityLayerMask;

        private Vector2 _startMousePosition;
        private bool _isDragging;

        public SelectionSystem(IInputDeviceProvider input, EntityRegistry registry, SelectionBuffer buffer)
        {
            _input = input;
            _registry = registry;
            _buffer = buffer;
            _camera = Camera.main;
            _entityLayerMask = LayerMask.GetMask("Entity");
        }

        private bool IsActionKeyState(ushort actionKeyId, ActionKeyState state) 
            => _input.IsActionKeyState(actionKeyId, state, InputStartContext.World);

        public void Tick()
        {
            var mousePosision = new Vector2(_input.GetAxisValue(InputAxisId.MouseX), _input.GetAxisValue(InputAxisId.MouseY));

            if (IsActionKeyState(InputActionButtonId.LEFT_CLICK, ActionKeyState.Pressed))
            {
                _startMousePosition = mousePosision; 
                _isDragging = false;
            }

            if (IsActionKeyState(InputActionButtonId.LEFT_CLICK, ActionKeyState.Hold))
            {
                if (!_isDragging && Vector2.Distance(_startMousePosition, mousePosision) > DRAG_THRESHOLD)
                {
                    _isDragging = true;
                }

                if (_isDragging) 
                {
                    _buffer.FrameStartPoint = _startMousePosition;
                    _buffer.FrameEndPoint = mousePosision;
                    _buffer.IsFrameVisible = true;
                }
            }

            if (IsActionKeyState(InputActionButtonId.LEFT_CLICK, ActionKeyState.Released))
            {
                _input.ExtractActionKey(InputActionButtonId.LEFT_CLICK);
                _buffer.IsFrameVisible = false;

                if (_isDragging) SelectGroupInFrame(_startMousePosition, mousePosision);
                else SelectSingleEntity(mousePosision);

                _isDragging = false;
                _buffer.IsFrameVisible = false;
            }
        }

        private void SelectSingleEntity(Vector2 screenPosition)
        {
            var ray = _camera.ScreenPointToRay(screenPosition);
            var isAccumulateHold = IsActionKeyState(InputActionButtonId.ACCUMULATE, ActionKeyState.Hold);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _entityLayerMask))
            {
                var entity = hit.collider.GetComponentInParent<Entity>();
                
                if (entity)
                {
                    var component = entity.Get<SelectableComponent>();
                    if (component != default)
                    {
                        if (isAccumulateHold)
                        {
                            if (_buffer.SelectedEntities.Contains(entity))
                            {
                                component.IsSelected = false;
                                component.SelectableObject.SetActive(false);
                                _buffer.Deselect(entity);
                            }
                            else
                            {
                                _buffer.Select(entity);
                                component.IsSelected = true;
                                if (component.SelectableObject != null) component.SelectableObject.SetActive(true);
                            }
                        }
                        else
                        {
                            ResetCurrentSelectionToDeselected();

                            _buffer.Select(entity);
                            component.IsSelected = true;
                            if (component.SelectableObject != null) component.SelectableObject.SetActive(true);
                        }
                    }
                }
            }
            else
            {
                ResetCurrentSelectionToDeselected();
            }
        }

        private void SelectGroupInFrame(Vector2 start, Vector2 end)
        {
            var isAccumulateHold = IsActionKeyState(InputActionButtonId.ACCUMULATE, ActionKeyState.Hold);

            if (!isAccumulateHold)
            {
                ResetCurrentSelectionToDeselected();
            }

            var screenBounds = CreateScreenBounds(start, end);
            foreach (var entity in _registry.SelectableEntities)
            {
                if (entity == null) continue;

                var screenPos = _camera.WorldToScreenPoint(entity.transform.position);
                if (screenPos.z >= 0)
                {
                    if (screenBounds.Contains(new Vector3(screenPos.x, screenPos.y, 0f)))
                    {
                        var component = entity.Get<SelectableComponent>();
                        if (component != default)
                        {
                            component.IsSelected = true;
                            component.SelectableObject.SetActive(true);
                            _buffer.Select(entity);
                        }
                    }
                }
            }
        }

        private void ResetCurrentSelectionToDeselected()
        {
            foreach (var entity in _buffer.SelectedEntities)
            {
                if (entity != null)
                {
                    var component = entity.Get<SelectableComponent>();
                    component.IsSelected = false;
                    component.SelectableObject.SetActive(false);
                }
            }

            _buffer.Reset();
        }

        private Bounds CreateScreenBounds(Vector2 p1, Vector2 p2)
        {
            var min = Vector2.Min(p1, p2); 
            var max = Vector2.Max(p1, p2);
            
            var center = (min + max) / 2f;
            var size = new Vector3(max.x - min.x, max.y - min.y, 0f);
            
            return new Bounds(center, size);
        }
    }
}
