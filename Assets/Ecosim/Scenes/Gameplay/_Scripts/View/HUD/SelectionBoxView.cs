using UnityEngine;

namespace Ecosim
{
    [System.Serializable]
    public class SelectionBoxView
    {
        [SerializeField] private Color _boxColor = new Color(0f, 1f, 0f, 0.15f);
        [SerializeField] private Color _borderColor = new Color(0f, 1f, 0f, 0.8f);
        [SerializeField] private float _borderThickness = 2f;

        private Texture2D _whiteTexture;

        public void Render(SelectionBuffer buffer)
        {
            if (!buffer.IsFrameVisible) return;

            if (_whiteTexture == null)
            {
                _whiteTexture = new Texture2D(1, 1);
                _whiteTexture.SetPixel(0, 0, Color.white);
                _whiteTexture.Apply();
            }

            var p1 = buffer.FrameStartPoint;
            var p2 = buffer.FrameEndPoint;
            p1.y = Screen.height - p1.y;
            p2.y = Screen.height - p2.y;

            var rect = Rect.MinMaxRect(
                Mathf.Min(p1.x, p2.x), Mathf.Min(p1.y, p2.y), 
                Mathf.Max(p1.x, p2.x), Mathf.Max(p1.y, p2.y)
            );

            GUI.color = _boxColor;
            GUI.DrawTexture(rect, _whiteTexture);

            GUI.color = _borderColor;
            GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width, _borderThickness), _whiteTexture);
            GUI.DrawTexture(new Rect(rect.x, rect.yMax - _borderThickness, rect.width, _borderThickness), _whiteTexture);
            GUI.DrawTexture(new Rect(rect.x, rect.y, _borderThickness, rect.height), _whiteTexture);
            GUI.DrawTexture(new Rect(rect.xMax - _borderThickness, rect.y, _borderThickness, rect.height), _whiteTexture);

            GUI.color = Color.white;
        }
    }
}
