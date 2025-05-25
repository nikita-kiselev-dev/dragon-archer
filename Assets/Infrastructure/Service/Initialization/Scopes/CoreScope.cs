using Infrastructure.Service.Audio;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.Service.Initialization.Scopes
{
    public class CoreScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<ButtonAudioManager>(Lifetime.Scoped).As<ControlEntity>().AsSelf();
        }
    }
}