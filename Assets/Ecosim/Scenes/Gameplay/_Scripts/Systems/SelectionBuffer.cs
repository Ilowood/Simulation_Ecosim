using System.Collections.Generic;
using UnityEngine;

namespace Ecosim
{
    public class SelectionBuffer
    {
        private readonly HashSet<Entity> _selectedEntities = new(256);
        public IReadOnlyCollection<Entity> SelectedEntities => _selectedEntities;
        
        public bool CanSelecting => _selectedEntities.Count == 0;

        public Vector2 FrameStartPoint;
        public Vector2 FrameEndPoint;
        public bool IsFrameVisible;

        public void Select(Entity entity) 
        { 
            _selectedEntities.Add(entity);
        }

        public void ClearSelected()
        {
            _selectedEntities.Clear();
        }
        
        public void Reset()
        {
            _selectedEntities.Clear();
            IsFrameVisible = false;
        }
    }
}
