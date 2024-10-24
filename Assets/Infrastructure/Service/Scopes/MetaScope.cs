using Infrastructure.Service.View.ViewManager;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.Service.Scopes
{
    public class MetaScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<ViewManager>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}