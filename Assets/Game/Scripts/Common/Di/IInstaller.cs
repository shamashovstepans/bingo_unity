using Zenject;

namespace BingoGame.Common.Di
{
    public interface IInstaller
    {
        void InstallBindings(DiContainer container);
    }
}