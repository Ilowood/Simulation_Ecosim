using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BugColony
{
    public interface IView
    {
        void Open();
        void Close();
    }

    public abstract class Screen : MonoBehaviour, IView
    {
        [field: Header("View components")]
        [field: SerializeField] public RectTransform SaveArea { get; private set; }

        public virtual void Close()
        {
            gameObject.SetActive(false);
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }
    }
}
