using Content.LoadingCurtain.Scripts.Controller;
using Infrastructure.Service.SignalBus;
using Infrastructure.Service.View.ViewManager;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.Service.Initialization.Scopes
{
    public class MetaScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<SceneStarter>();
            
            var loadingCurtainController = FindFirstObjectByType<LoadingCurtainController>();
            builder.RegisterComponent(loadingCurtainController);
            
            builder.Register<ViewManager>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<EventSignalBus>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}