using UnityEngine;
using UnityEngine.UI;

namespace Untils
{
    public class UIHelper
    {
        public static void SaveArea(RectTransform rect)
        {
            var saveArea = Screen.safeArea;

            var anchorMin = saveArea.position;
            var anchorMax = anchorMin + saveArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
        }

        public static float CalculateCellSize(float containerWidth, int columns, float padding, float spacing)
        {
            float availableWidth = containerWidth - padding * 2 - spacing * (columns - 1);
            return availableWidth / columns;
        }
        
        public static int CalculateColumns(float containerWidth, float preferredCellSize, float spacing, int minColumns, int maxColumns)
        {
            int columns = Mathf.FloorToInt((containerWidth - spacing) / (preferredCellSize + spacing));
            return Mathf.Clamp(columns, minColumns, maxColumns);
        }
        
        public static void ApplyGridSettings(GridLayoutGroup grid, int columns, float cellSize, Vector2 spacing, Vector2 padding)
        {
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = columns;
            grid.cellSize = new Vector2(cellSize, cellSize);
            grid.spacing = spacing;
            grid.padding.left = (int)padding.x;
            grid.padding.right = (int)padding.x;
            grid.padding.top = (int)padding.y;
            grid.padding.bottom = (int)padding.y;
        }

    }
}
