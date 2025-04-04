using UnityEngine;
using Zenject;

namespace BingoGame.Ui.PopupManager
{
    internal class PopupManagerInstaller : MonoInstaller<PopupManagerInstaller>
    {
        [SerializeField] private PopupManagerView _popupManagerView;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_popupManagerView).AsSingle();
            Container.BindInterfacesTo<PopupManager>().AsSingle().NonLazy();
        }
    }
}