using Content.LoadingCurtain.Scripts.Controller;
using Infrastructure.Game;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.Service.Initialization.Scopes
{
    public sealed class BootstrapScope : LifetimeScope
    {
        [SerializeField] private LoadingCurtainController m_LoadingCurtainController;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(m_LoadingCurtainController).AsImplementedInterfaces();
            builder.RegisterEntryPoint<SceneStarter>();
            builder.Register<GameBootstrapper>(Lifetime.Scoped).AsImplementedInterfaces();
        }
    }
}