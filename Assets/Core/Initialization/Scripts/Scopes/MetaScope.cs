using Core.Asset.IconController;
using Core.Audio;
using Core.Initialization.Scripts.Decorators.FastView;
using Core.View.ViewManager;
using VContainer;
using VContainer.Unity;

namespace Core.Initialization.Scripts.Scopes
{
    public class MetaScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<SceneStarter>();
            builder.Register<DailyBonus.Scripts.DailyBonus>(Lifetime.Scoped).As<ControlEntity>().AsImplementedInterfaces();
            builder.Register<ButtonAudioManager>(Lifetime.Scoped).As<ControlEntity>().AsSelf();
            builder.Register<ViewManager>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<FastViewDecorator>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<IconController>(Lifetime.Scoped).AsImplementedInterfaces();
        }
    }
}