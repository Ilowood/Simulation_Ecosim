using UnityEngine;
using Zenject;

namespace Ecosim
{
    public class EcosimInstaller : MonoInstaller
    {
        [SerializeField] private EntityDatabase _entityDatabase;
        [SerializeField] private ItemDatabase _itemDatabase;
        
        public override void InstallBindings()
        {
            Container.Bind<EntityDatabase>().FromInstance(_entityDatabase).AsSingle();
            Container.Bind<ItemDatabase>().FromInstance(_itemDatabase).AsSingle();
        }
    }
}
