namespace Infrastructure.Service.Initialization.Decorators
{
    public interface IControlEntityDecorator
    {
        ControlEntity Decorate(ControlEntity controlEntity);
    }
}