using Zenject;
using UnityEngine;

namespace BugColony
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private SpawnerEntityConfig _spawnerEntityConfig;

        public override void InstallBindings()
        {
            InstallConfigs();
            InstallView();
            InstallSystem();
            InstallFSM();
        }

        private void InstallConfigs()
        {
            Container.Bind<SpawnerEntityConfig>().FromInstance(_spawnerEntityConfig).AsSingle();
        }

        private void InstallView()
        {
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
            Container.BindInterfacesAndSelfTo<GameLoopState>().AsSingle();
            Container.BindInterfacesAndSelfTo<PauseState>().AsSingle();

            Container.Bind<FSMGameplay>().AsSingle();
        }
    }
}
