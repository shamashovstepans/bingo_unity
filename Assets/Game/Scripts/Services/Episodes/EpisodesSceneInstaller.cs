using UnityEngine;
using Zenject;

namespace BingoGame.Services.Episodes
{
    internal class EpisodesSceneInstaller : MonoInstaller<EpisodesSceneInstaller>
    {
        [SerializeField] private EpisodesView _episodesView;
        
        public override void InstallBindings()
        {
            Container.Bind<EpisodesController>().AsSingle().NonLazy();
            Container.BindInstance(_episodesView).AsSingle();
        }
    }
}