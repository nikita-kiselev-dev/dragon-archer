using Content.SettingsPopup.Scripts;
using Content.StartWindow.Scripts.Controller;
using Infrastructure.Service.Asset.IconController;
using Infrastructure.Service.Audio;
using Infrastructure.Service.Initialization.Decorators.FastView;
using Infrastructure.Service.View.ViewManager;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.Service.Initialization.Scopes
{
    public class StartScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<SceneStarter>();
            builder.Register<SettingsCore>(Lifetime.Scoped).As<ControlEntity>().AsImplementedInterfaces();
            builder.Register<StartWindowController>(Lifetime.Scoped).As<ControlEntity>().AsImplementedInterfaces();
            builder.Register<ButtonAudioManager>(Lifetime.Scoped).As<ControlEntity>().AsSelf();
            builder.Register<ViewManager>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<FastViewDecorator>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<IconController>(Lifetime.Scoped).AsImplementedInterfaces();
        }
    }
}