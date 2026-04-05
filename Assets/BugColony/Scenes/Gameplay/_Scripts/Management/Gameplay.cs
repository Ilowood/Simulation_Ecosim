using UnityEngine;
using Zenject;

namespace BugColony
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
