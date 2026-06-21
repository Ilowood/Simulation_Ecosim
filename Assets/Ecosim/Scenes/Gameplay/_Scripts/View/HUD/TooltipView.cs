using TMPro;
using UnityEngine;
using Zenject;

namespace Ecosim
{
    public class TooltipView : MonoBehaviour, ITicable
    {
        [Inject] private HoverTooltipBuffer _buffer;

        [SerializeField] private Canvas _canvas;
        [SerializeField] private RectTransform _this;
        [SerializeField] private RectTransform _scrollRect;

        [Space, SerializeField] private TMP_Text _type;
        [SerializeField] private TMP_Text _id;
        [SerializeField] private TMP_Text _description;

        [Space, SerializeField] private float _minHeightDescription = 40f;
        [SerializeField] private float _maxHeightDescription = 200f;
        [SerializeField] private float _offset = 15f;

        public void Tick(float deltaTime, float scale)
        {
            if (!_buffer.IsHover)
            {
                if (gameObject.activeInHierarchy) Hide();
            }
            else
            {
                Show();
                PositionTooltip();
            }
        }

        private void Show()
        {
            _type.text = $"{_buffer.Title}";
            _id.text = $"{_buffer.Id}";
            _description.text = $"{_buffer.Description}";

            if (!gameObject.activeSelf) gameObject.SetActive(true);
            
            Canvas.ForceUpdateCanvases();
        
            var clampedHeight = Mathf.Clamp(_description.preferredHeight, _minHeightDescription, _maxHeightDescription);
            _scrollRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clampedHeight);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }  

        private void PositionTooltip()
        {
            var canvasRect = (RectTransform)transform.parent;
            var screenPos = Camera.main.WorldToScreenPoint(_buffer.WorldPosition);

            if (screenPos.z < 0)
            {
                Hide();
                return;
            }

            var camForUtility = (_canvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : Camera.main;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                screenPos,
                camForUtility,
                out Vector2 targetCanvasPos
            );
            
            var tooltipSize = _this.rect.size;
            var canvasSize = canvasRect.rect.size;
            var finalPosisiotn = CalculateBestPositionInCanvas(targetCanvasPos, tooltipSize, canvasSize);

            _this.anchoredPosition = finalPosisiotn;
        }
        
        private Vector2 CalculateBestPositionInCanvas(Vector2 targetPos, Vector2 tooltipSize, Vector2 canvasSize)
        {
            var canvasLeft = -canvasSize.x / 2f;
            var canvasRight = canvasSize.x / 2f;
            var canvasBottom = -canvasSize.y / 2f;
            var canvasTop = canvasSize.y / 2f;

            var spaceTop = canvasTop - targetPos.y;
            var spaceBottom = targetPos.y - canvasBottom;
            var spaceRight = canvasRight - targetPos.x;
            var spaceLeft = targetPos.x - canvasLeft;
            
            var resultPos = default(Vector2);
            var offset = Vector2.zero;

            var canFitTop = spaceTop > tooltipSize.y + _offset;
            var canFitBottom = spaceBottom > tooltipSize.y + _offset;
            var canFitRight = spaceRight > tooltipSize.x + _offset;
            var canFitLeft = spaceLeft > tooltipSize.x + _offset;

            if (canFitTop)
            {
                offset = new Vector2(0, tooltipSize.y / 2f + _offset);
            }
            else if (canFitBottom)
            {
                offset = new Vector2(0, -tooltipSize.y / 2f - _offset);
            }
            else if (canFitRight)
            {
                offset = new Vector2(tooltipSize.x / 2f + _offset, 0);
            }
            else if (canFitLeft)
            {
                offset = new Vector2(-tooltipSize.x / 2f - _offset, 0);
            }
            else
            {
                return Vector2.zero;
            }

            resultPos = targetPos + offset;

            var minX = canvasLeft + tooltipSize.x / 2f;
            var maxX = canvasRight - tooltipSize.x / 2f;
            var minY = canvasBottom + tooltipSize.y / 2f;
            var maxY = canvasTop - tooltipSize.y / 2f;
            
            if (tooltipSize.x < canvasSize.x && tooltipSize.y < canvasSize.y)
            {
                resultPos.x = Mathf.Clamp(resultPos.x, minX, maxX);
                resultPos.y = Mathf.Clamp(resultPos.y, minY, maxY);
            }
            else
            {
                resultPos = Vector2.zero;
            }

            return resultPos;
        }      
    }
}
