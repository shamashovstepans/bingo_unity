using BingoGame.AppNavigation;
using BingoGame.Episodes;
using Game.Scripts.Module;
using Zenject;

namespace BingoGame
{
    internal class MainSceneInstaller : MonoInstaller<MainSceneInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<EpisodesModel>().AsSingle();
            Container.Bind<GameModel>().AsSingle();
            Container.BindInterfacesTo<NavigationController>().AsSingle();
        }
    }
}