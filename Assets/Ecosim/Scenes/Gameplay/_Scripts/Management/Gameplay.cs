using UnityEngine;
using Zenject;

namespace Ecosim
{
    public class Gameplay : MonoBehaviour
    {
        [Inject] private FSMGameplay _fSMGameplay;

        private async void Start()
        {
            _fSMGameplay.EnterIn(StateGameplay.InitState);
        }
    }
}
