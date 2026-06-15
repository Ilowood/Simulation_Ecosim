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
            InstallService();
            InstallWorld();
        }

        private void InstallData()
        {
            Container.Bind<SpawnerConfig>().FromInstance(_spawnerConfig).AsSingle();
            Container.BindInterfacesAndSelfTo<EntityRegistry>().AsSingle();
            Container.Bind<ISaveService>().To<JsonSaveService>().AsSingle();
        }

        private void InstallFractory()
        {
            Container.Bind<EntityFactory>().AsSingle();

            Container.BindInterfacesAndSelfTo<SequenceTaskrFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<ResourceTransferFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<MoveToPointFactory>().AsSingle();
            Container.Bind<TaskFactory>().AsSingle();

            Container.BindInterfacesAndSelfTo<BuildToolPipelineFactory>().AsSingle();
            Container.Bind<ToolFactory>().AsSingle();
        }

        private void InstallView()
        {
            Container.Bind<LoadView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<HUDView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PauseView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ReportView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<LevelEditorView>().FromComponentInHierarchy().AsSingle();
        }

        private void InstallSystem()
        {
            Container.Bind<SelectionBuffer>().AsSingle();
            Container.Bind<SelectionSystem>().AsSingle();
            Container.Bind<PlayerCommandSystem>().AsSingle();
        }

        private void InstallService()
        {
            Container.Bind<SpawnService>().AsSingle();
            Container.Bind<StorageServic>().AsSingle();
        }

        private void InstallWorld()
        {
            Container.Bind<World>().AsSingle();

            Container.BindInterfacesAndSelfTo<InitState>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayState>().AsSingle();
            Container.BindInterfacesAndSelfTo<PauseState>().AsSingle();
            Container.BindInterfacesAndSelfTo<ReportState>().AsSingle();
            Container.BindInterfacesAndSelfTo<RestartState>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelEditorState>().AsSingle();
            Container.Bind<FSMGameplay>().AsSingle();
        }
    }
}
