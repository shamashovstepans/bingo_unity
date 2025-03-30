using BingoGame.Module;
using BingoGame.Modules.Logger;
using Game.Scenes.HttpClient;
using UnityEngine;
using Zenject;

namespace BingoGame.Modules
{
    [CreateAssetMenu]
    internal class RootInstaller : ScriptableObjectInstaller<RootInstaller>
    {
        public override void InstallBindings()
        {
            new HttpClientInstaller().InstallBindings(Container);
            new LoggerInstaller("root").InstallBindings(Container);

            new BackendServiceInstaller().InstallBindings(Container);
        }
    }
}