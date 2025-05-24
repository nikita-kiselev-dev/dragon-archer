using Content.SettingsPopup.Scripts;
using Content.StartWindow.Scripts.Controller;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.Service.Initialization.Scopes
{
    public class StartScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<SceneStarter>();
            builder.Register<SettingsCore>(Lifetime.Singleton).As<ControlEntity>().AsImplementedInterfaces();
            builder.Register<StartWindowController>(Lifetime.Singleton).As<ControlEntity>().AsImplementedInterfaces();
        }
    }
}