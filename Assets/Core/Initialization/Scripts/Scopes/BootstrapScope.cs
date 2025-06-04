using Core.LoadingCurtain.Scripts.Controller;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core.Initialization.Scripts.Scopes
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