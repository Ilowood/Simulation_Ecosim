using UnityEngine;

namespace Ecosim
{
    public class EntityModel : MonoBehaviour
    {
        private readonly int ColorPropertyId = Shader.PropertyToID("_BaseColor");

        [SerializeField] private Renderer _renderer;
        [SerializeField] public Transform _transform;
        
        private MaterialPropertyBlock _propBlock;

        public void SetColor(Color newColor)
        {
            _propBlock = new MaterialPropertyBlock();

            _renderer.GetPropertyBlock(_propBlock);
            _propBlock.SetColor(ColorPropertyId, newColor);
            _renderer.SetPropertyBlock(_propBlock);
        }

        public void SetScale(Vector3 scale)
        {
            _transform.localScale = scale;
        }

        public void SetLocalPosition(Vector3 position)
        {
            _transform.localPosition = position; 
        }
    }
}
