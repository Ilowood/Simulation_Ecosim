using UnityEngine;

namespace Ecosim
{
    public abstract class Screen : MonoBehaviour, IView
    {
        [field: Header("View components")]
        [field: SerializeField] public RectTransform SaveArea { get; private set; }

        public virtual void Close()
        {
            gameObject.SetActive(false);
        }

        public virtual void Open()
        {
            gameObject.SetActive(true);
        }
    }
}
