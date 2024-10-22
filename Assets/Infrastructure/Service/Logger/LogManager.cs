using UnityEngine;

namespace Infrastructure.Service.Logger
{
    public class LogManager : ILogManager
    {
        private readonly string _entityName;

        public LogManager(string entityName)
        {
            _entityName = entityName;
        }

        public void Log(string message)
        {
            var log = string.Format(
                LoggerEnvironment.LogFormat, 
                LoggerEnvironment.DefaultLogColor, 
                _entityName, 
                message);
            
            Debug.Log(log);
        }

        public void LogError(string message)
        {
            var log = string.Format(
                LoggerEnvironment.LogFormat, 
                LoggerEnvironment.ErrorLogColor, 
                _entityName, 
                message);
            
            Debug.LogError(log);
        }
    }
}