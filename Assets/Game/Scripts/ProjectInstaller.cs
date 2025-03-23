using BingoGame.Services;
using UnityEngine;
using Zenject;

namespace BingoGame
{
    internal class ProjectInstaller : MonoInstaller<ProjectInstaller>
    {
        public override void InstallBindings()
        {
            Debug.LogError("Install called");
            Container.BindInterfacesAndSelfTo<SupabaseService>().AsSingle().NonLazy();
        }
    }
}