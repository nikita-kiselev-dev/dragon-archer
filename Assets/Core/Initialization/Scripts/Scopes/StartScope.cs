using Core.Asset.IconController;
using Core.Audio;
using Core.Initialization.Scripts.Decorators.FastView;
using Core.SettingsPopup.Scripts;
using Core.StartWindow.Scripts.Controller;
using Core.View.ViewManager;
using VContainer;
using VContainer.Unity;

namespace Core.Initialization.Scripts.Scopes
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