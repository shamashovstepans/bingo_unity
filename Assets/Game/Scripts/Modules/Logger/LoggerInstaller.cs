using Zenject;
using IInstaller = BingoGame.Common.Di.IInstaller;

namespace BingoGame.Modules.Logger
{
    public class LoggerInstaller : IInstaller
    {
        private readonly string _category;

        public LoggerInstaller(string category)
        {
            _category = category;
        }

        public void InstallBindings(DiContainer container)
        {
            container.Bind<ILogger>().To<Logger>().AsSingle().WithArguments(_category);
        }
    }
}