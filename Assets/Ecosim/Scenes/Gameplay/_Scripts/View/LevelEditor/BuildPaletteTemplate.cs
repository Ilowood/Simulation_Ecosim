using UnityEngine;
using UnityEngine.UI;
using System;

namespace Ecosim
{
    public class BuildPaletteTemplate : MonoBehaviour
    {
        [Header("UI Elements")]
        // [SerializeField] private Image _icon;
        [SerializeField] private Image _background;
        [SerializeField] private Button _button;
        
        [Header("Colors")]
        [SerializeField] private Color _normalColor = Color.white;
        [SerializeField] private Color _selectedColor = new Color(0.8f, 0.9f, 1f, 1f);
        [SerializeField] private Color _hoverColor = new Color(0.9f, 0.95f, 1f, 1f);

        public long SpecId { get; private set; }
        
        public void Init(long specId, Sprite icon, Action<BuildPaletteTemplate> click)
        {
            SpecId = specId;

            _background.sprite = icon;
            _button.onClick.AddListener(() => click(this));
        }
        
        public void SetSelected(bool isSelected)
        {
            _background.color = isSelected ? _selectedColor : _normalColor;
        }
        
        public void OnPointerEnter()
        {
            if (_background.color != _selectedColor)
            {
                _background.color = _hoverColor;
                // _icon.color = _hoverColor;
            }
        }
        
        public void OnPointerExit()
        {
            if (_background.color != _selectedColor)
            {
                _background.color = _normalColor;
                // _icon.color = _normalColor;
            }
        }
    }
}
