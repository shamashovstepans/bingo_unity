using Zenject;
using IInstaller = BingoGame.Common.Di.IInstaller;

namespace BingoGame.Module
{
    internal class BackendServiceInstaller : IInstaller
    {
        public void InstallBindings(DiContainer container)
        {
            container.Bind<IBackendService>().To<BackendService>().AsSingle();
        }
    }
}