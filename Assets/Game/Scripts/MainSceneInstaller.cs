using BingoGame.AppNavigation;
using BingoGame.Episodes;
using BingoGame.Module;
using Game.Scripts.Module;
using UnityEngine;
using Zenject;

namespace BingoGame
{
    [CreateAssetMenu]
    internal class MainSceneInstaller : ScriptableObjectInstaller<MainSceneInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<BingoModule>().AsSingle().NonLazy();
            Container.Bind<EpisodesModel>().AsSingle();
            Container.Bind<GameModel>().AsSingle();
            Container.BindInterfacesTo<NavigationController>().AsSingle();
        }
    }
}