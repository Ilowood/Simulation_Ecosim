using Zenject;
using UnityEngine;

namespace Ecosim
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private SpawnerConfig _spawnerConfig;

        public override void InstallBindings()
        {
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
            Container.Bind<EntityFactory>().AsSingle();
        }

        private void InstallView()
        {
            Container.Bind<LoadView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<HUDView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PauseView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ReportView>().FromComponentInHierarchy().AsSingle();
        }

        private void InstallSystem()
        {
            Container.Bind<Spawner>().AsSingle();
            Container.Bind<World>().AsSingle();
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
