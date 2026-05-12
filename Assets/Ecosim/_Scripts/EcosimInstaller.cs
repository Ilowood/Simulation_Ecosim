using UnityEngine;
using Zenject;

namespace Ecosim
{
    public class EcosimInstaller : MonoInstaller
    {
        [SerializeField] private EntityRegistry _registry;
        
        public override void InstallBindings()
        {
            Container.Bind<EntityRegistry>().FromInstance(_registry).AsSingle();
        }
    }
}
