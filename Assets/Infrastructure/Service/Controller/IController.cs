namespace Infrastructure.Service.Controller
{
    public interface IController
    {
        bool IsInited { get; }
        bool IsActive { get; }
    }
}