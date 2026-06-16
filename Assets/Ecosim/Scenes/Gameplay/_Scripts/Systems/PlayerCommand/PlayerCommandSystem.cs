using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Ecosim
{
    public class PlayerCommandSystem
    {
        private IInputDeviceProvider _input;
        private SelectionBuffer _buffer;
        private List<ICommandProvider> _commands;

        [Inject]
        public void Init(IInputDeviceProvider input, SelectionBuffer buffer, List<ICommandProvider> commands)
        {
            _input = input;
            _buffer = buffer;
            _commands = commands.OrderByDescending(c => c.Priority).ToList();
        }

        private bool IsActionKeyState(ushort actionKeyId, ActionKeyState state) 
            => _input.IsActionKeyState(actionKeyId, state, InputStartContext.World);

        public void Tick()
        {
            if (_buffer.SelectedEntities.Count > 0)
            {
                if (IsActionKeyState(InputActionButtonId.RIGHT_CLICK, ActionKeyState.Released))
                {
                    _input.ExtractActionKey(InputActionButtonId.RIGHT_CLICK);

                    var mousePosition = new Vector2(_input.GetAxisValue(InputAxisId.MouseX), _input.GetAxisValue(InputAxisId.MouseY));
                    var ray = Camera.main.ScreenPointToRay(mousePosition);

                    if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                    {
                        foreach (var entity in _buffer.SelectedEntities)
                        {
                            foreach (var command in _commands)
                            {
                                if (command.CanExecute(entity, hit))
                                {
                                    command.Create(entity, hit);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
