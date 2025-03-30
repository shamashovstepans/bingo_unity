using Zenject;
using IInstaller = BingoGame.Common.Di.IInstaller;

namespace Game.Scenes.HttpClient
{
    internal class HttpClientInstaller : IInstaller
    {
        public void InstallBindings(DiContainer container)
        {
            container.Bind<IHttpClient>().To<HttpClient>().AsSingle();
        }
    }
}