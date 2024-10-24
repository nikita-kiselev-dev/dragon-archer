using Content.LoadingCurtain.Scripts.Controller;
using Content.Settings.Scripts;
using Content.StartScreen.Scripts.Controller;
using Infrastructure.Service.Asset;
using Infrastructure.Service.Initialization;
using Infrastructure.Service.Scene;
using Infrastructure.Service.SignalBus;
using Infrastructure.Service.StateMachine;
using Infrastructure.Service.View.ViewFactory;
using Infrastructure.Service.View.ViewManager;
using Infrastructure.Service.View.ViewManager.ViewAnimation;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.Service.Scopes
{
    public class StartScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<SceneStarter>();
            
            builder.Register<MainAddressableAssetLoader>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<LoadingCurtainController>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<BackgroundAnimator>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SceneStateMachine>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<EventSignalBus>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SettingsPopup>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SceneService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<ViewManager>(Lifetime.Singleton).AsImplementedInterfaces();
            
            builder.Register<StartScreenController>(Lifetime.Singleton).As<ControlEntity>().AsImplementedInterfaces();
            builder.Register<ViewFactory>(Lifetime.Singleton).As<ControlEntity>().AsImplementedInterfaces();
        }
    }
}