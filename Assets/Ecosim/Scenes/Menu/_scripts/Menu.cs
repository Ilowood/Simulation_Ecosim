using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Ecosim
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private Button _start;

        public void Awake()
        {
            _start.onClick.AddListener(() => SceneManager.LoadScene(Scenes.Gameplay));
        }
    }
}
