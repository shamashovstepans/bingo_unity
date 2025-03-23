using UnityEngine;
using Zenject;

namespace BingoGame.Services
{
    internal class SignInSceneInstaller : MonoInstaller<SignInSceneInstaller>
    {
        [SerializeField] private SignInFormView _signInFormView;
        [SerializeField] private SignUpSceneView _signUpSceneView;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<SignInSceneController>().AsSingle().NonLazy();
            Container.BindInstance(_signInFormView).AsSingle();
            Container.BindInstance(_signUpSceneView).AsSingle();
        }
    }
}