using UnityEngine;

namespace Ecosim
{
    public class HoverTooltipBuffer
    {
        public long Id;
        public string Title;
        public string Description;
        public Vector3 WorldPosition;
        public bool IsHover;

        public void Reset()
        {
            Id = default;
            Title = default;
            Description = default;
            WorldPosition = default;
            IsHover = false;
        }
    }
}
