namespace Infrastructure.Service.Logger
{
    public interface ILogManager
    {
        void Log(string message);
        void LogError(string message);
    }
}