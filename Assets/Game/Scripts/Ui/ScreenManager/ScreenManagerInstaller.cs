using UnityEngine;
using Zenject;

namespace BingoGame.Ui
{
    internal class ScreenManagerInstaller : MonoInstaller<ScreenManagerInstaller>
    {
        [SerializeField] private ScreenManagerView _screenManagerView;

        public override void InstallBindings()
        {
            Container.BindInstance(_screenManagerView).AsSingle();
            Container.BindInterfacesTo<ScreenManager>().AsSingle().NonLazy();
        }
    }
}