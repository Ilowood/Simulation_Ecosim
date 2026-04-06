using UnityEngine;
using Zenject;

namespace BugColony
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
