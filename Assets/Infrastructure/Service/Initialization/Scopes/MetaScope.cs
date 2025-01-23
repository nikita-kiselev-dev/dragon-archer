using Content.DailyBonus.Scripts;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.Service.Initialization.Scopes
{
    public class MetaScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<SceneStarter>();
            builder.Register<DailyBonus>(Lifetime.Scoped).As<ControlEntity>().AsImplementedInterfaces();
        }
    }
}