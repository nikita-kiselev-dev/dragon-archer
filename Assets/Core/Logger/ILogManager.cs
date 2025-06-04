namespace Core.Logger
{
    public interface ILogManager
    {
        void Log(string message);
        void LogError(string message);
    }
}