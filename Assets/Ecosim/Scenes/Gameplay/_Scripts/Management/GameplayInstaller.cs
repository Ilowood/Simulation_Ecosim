using Zenject;
using UnityEngine;

namespace Ecosim
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private SpawnerConfig _spawnerEntityConfig;
        [SerializeField] private SimulationConfig _simulationConfig;

        public override void InstallBindings()
        {
            InstallConfigs();
            InstallView();
            InstallSystem();
            InstallFSM();
        }

        private void InstallConfigs()
        {
            Container.Bind<SpawnerConfig>().FromInstance(_spawnerEntityConfig).AsSingle();
            Container.Bind<SimulationConfig>().FromInstance(_simulationConfig).AsSingle();
        }

        private void InstallView()
        {
            Container.Bind<LoadView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<HUDView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PauseView>().FromComponentInHierarchy().AsSingle();
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

            Container.Bind<FSMGameplay>().AsSingle();
        }
    }
}
