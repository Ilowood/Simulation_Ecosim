using UnityEngine;
using Zenject;

namespace Ecosim
{
    public class Gameplay : MonoBehaviour
    {
        [Inject] private FSMGameplay _fSMGameplay;

        private void Start()
        {
            _fSMGameplay.EnterIn(StateGameplay.InitState);
        }
    }
}
