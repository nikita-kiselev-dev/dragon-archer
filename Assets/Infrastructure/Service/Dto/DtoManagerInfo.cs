using System.IO;
using UnityEngine;

namespace Infrastructure.Service.Dto
{
    public class DtoManagerInfo
    {
        private const string ConfigName = "Config.json";
        
        public static readonly string ConfigDirectory = $"{Application.persistentDataPath}/Data/";
        public static readonly string ConfigPath = Path.Combine(ConfigDirectory, ConfigName);
    }
}