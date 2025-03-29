using UnityEngine;
using Zenject;

namespace Game.Scenes.HttpClient
{
    [CreateAssetMenu]
    internal class HttpClientInstaller : ScriptableObjectInstaller<HttpClientInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<HttpClient>().AsSingle().NonLazy();
        }
    }
}