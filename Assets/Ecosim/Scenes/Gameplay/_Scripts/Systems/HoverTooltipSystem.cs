using UnityEngine;

namespace Ecosim
{
    public class HoverTooltipSystem : ITicable
    {
        private const float REQUIRED_HOVER_TIME = 0.5f;
        
        private readonly IInputDeviceProvider _input;
        private readonly HoverTooltipBuffer _buffer;

        private readonly Camera _camera;
        private readonly int _hoverLayerMask;

        private Entity _currentHoveredEntity;
        private float _hoverTime;

        public HoverTooltipSystem(IInputDeviceProvider input, HoverTooltipBuffer buffer)
        {
            _input = input;
            _buffer = buffer;

            _camera = Camera.main;
            _hoverLayerMask = LayerMask.GetMask("Entity");
        }

        public void Tick(float deltaTime, float scale)
        {
            var mousePosition = new Vector2(_input.GetAxisValue(InputAxisId.MouseX), _input.GetAxisValue(InputAxisId.MouseY));
            var ray = _camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _hoverLayerMask))
            {
                var entity = hit.collider.GetComponentInParent<Entity>();
                var component = entity.Get<DescriptionComponent>();
                
                if (component != default)
                {
                    if (entity == _currentHoveredEntity)
                    {
                        _hoverTime += deltaTime;
                        
                        if (_hoverTime >= REQUIRED_HOVER_TIME)
                        {
                            _buffer.Id = entity.Id;
                            _buffer.Title = component.Name;
                            _buffer.Description = component.Description;
                            _buffer.WorldPosition = entity.transform.position;
                            _buffer.IsHover = true;
                        }
                    }
                    else
                    {
                        _currentHoveredEntity = entity;
                        _hoverTime = 0f;
                        _buffer.Reset();
                    }
                }
            }
            else
            {
                _currentHoveredEntity = null;
                _hoverTime = 0f;
                _buffer.Reset();
            }

        }
    }
}
