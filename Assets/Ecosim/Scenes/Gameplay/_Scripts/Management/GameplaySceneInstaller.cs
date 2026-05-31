using Zenject;
using UnityEngine;
using Unity.AI.Navigation;

namespace Ecosim
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private SpawnerConfig _spawnerConfig;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<EntityRegistry>().AsSingle();
            Container.Bind<World>().AsSingle();
            
            InstallData();
            InstallFractory();
            InstallView();
            InstallSystem();
            InstallFSM();
        }

        private void InstallData()
        {
            Container.Bind<SpawnerConfig>().FromInstance(_spawnerConfig).AsSingle();
            Container.Bind<ISaveService>().To<JsonSaveService>().AsSingle();
        }

        private void InstallFractory()
        {
            Container.BindInterfacesAndSelfTo<SequenceTaskrFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<ResourceTransferFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<MoveToPointFactory>().AsSingle();

            Container.Bind<EntityFactory>().AsSingle();
            Container.Bind<TaskFactory>().AsSingle();
        }

        private void InstallView()
        {
            Container.Bind<LoadView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<HUDView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PauseView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ReportView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<NavMeshSurface>().FromComponentInHierarchy().AsSingle();
        }

        private void InstallSystem()
        {
            Container.Bind<SpawnSystem>().AsSingle();
            Container.Bind<StorageSystem>().AsSingle();
        }

        private void InstallFSM()
        {
            Container.BindInterfacesAndSelfTo<InitState>().AsSingle();
            Container.BindInterfacesAndSelfTo<WorldState>().AsSingle();
            Container.BindInterfacesAndSelfTo<PauseState>().AsSingle();
            Container.BindInterfacesAndSelfTo<ReportState>().AsSingle();
            Container.BindInterfacesAndSelfTo<RestartState>().AsSingle();

            Container.Bind<FSMGameplay>().AsSingle();
        }
    }
}
