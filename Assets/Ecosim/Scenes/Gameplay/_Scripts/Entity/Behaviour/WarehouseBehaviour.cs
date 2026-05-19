using UnityEngine;

namespace Ecosim
{
    public class WarehouseBehaviour : IBehaviour
    {
        private float _squareSize = 4f;

        public WarehouseBehaviour()
        {
            
        }

        public void Tick(Entity entity, WorldContext context, float deltaTime, float scale)
        {
            var center = entity.transform.position; 
            DrawSquareXZ(center, _squareSize, Color.black);
        }

        private void DrawSquareXZ(Vector3 center, float size, Color color)
        {
            var half = size / 2f;

            var topLeft     = center + new Vector3(-half, 0, half);
            var topRight    = center + new Vector3(half, 0, half);
            var bottomLeft  = center + new Vector3(-half, 0, -half);
            var bottomRight = center + new Vector3(half, 0, -half);

            Debug.DrawLine(topLeft, topRight, color);
            Debug.DrawLine(topRight, bottomRight, color);
            Debug.DrawLine(bottomRight, bottomLeft, color);
            Debug.DrawLine(bottomLeft, topLeft, color);
        }
    }
}
