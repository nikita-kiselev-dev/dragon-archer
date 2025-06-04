namespace Core.Controller
{
    public interface IController
    {
        bool IsInited { get; }
        bool IsActive { get; }
    }
}