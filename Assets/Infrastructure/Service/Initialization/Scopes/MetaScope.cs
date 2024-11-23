using Infrastructure.Service.View.Canvas;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.Service.Initialization.Scopes
{
    public class MetaScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<SceneStarter>();
            
            builder.Register<CanvasManager>(Lifetime.Singleton).As<ControlEntity>().AsImplementedInterfaces();
        }
    }
}