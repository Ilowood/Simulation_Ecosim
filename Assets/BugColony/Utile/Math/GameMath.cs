using UnityEngine;

namespace Untils
{
    public static class GameMath
    {
        public static float NormalizeToSubRange(float value, float srcMin, float srcMax, float targetMin, float targetMax)
        {
            var normalized = (value - srcMin) / (srcMax - srcMin);
            return targetMin + normalized * (targetMax - targetMin);
        }
    }
}
