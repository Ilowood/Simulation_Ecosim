using UnityEngine;

namespace Untils
{
    public class UI
    {
        public static void SaveArea(RectTransform rect)
        {
            var saveArea = UnityEngine.Screen.safeArea;

            var anchorMin = saveArea.position;
            var anchorMax = anchorMin + saveArea.size;

            anchorMin.x /= UnityEngine.Screen.width;
            anchorMin.y /= UnityEngine.Screen.height;
            anchorMax.x /= UnityEngine.Screen.width;
            anchorMax.y /= UnityEngine.Screen.height;

            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
        }
    }
}
