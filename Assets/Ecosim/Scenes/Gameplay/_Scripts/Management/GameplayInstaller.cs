using Zenject;
using UnityEngine;

namespace Ecosim
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private SpawnerConfig _spawnerConfig;
        [SerializeField] private SimulationConfig _simulationConfig;

        public override void InstallBindings()
        {
            InstallConfigs();
            InstallFractory();
            InstallView();
            InstallSystem();
            InstallFSM();
        }

        private void InstallConfigs()
        {
            Container.Bind<SpawnerConfig>().FromInstance(_spawnerConfig).AsSingle();
            Container.Bind<SimulationConfig>().FromInstance(_simulationConfig).AsSingle();
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
            Container.Bind<Simulation>().AsSingle();
        }

        private void InstallFSM()
        {
            Container.BindInterfacesAndSelfTo<InitState>().AsSingle();
            Container.BindInterfacesAndSelfTo<SimulationState>().AsSingle();
            Container.BindInterfacesAndSelfTo<PauseState>().AsSingle();
            Container.BindInterfacesAndSelfTo<ReportState>().AsSingle();
            Container.BindInterfacesAndSelfTo<RestartState>().AsSingle();

            Container.Bind<FSMGameplay>().AsSingle();
        }
    }
}
