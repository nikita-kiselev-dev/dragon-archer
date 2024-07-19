using System.IO;
using UnityEngine;

namespace Infrastructure.Service.SaveLoad
{
    public static class DataManagerInfo
    {
        private const string SaveFileName = "SaveFile.json";
        
        public static readonly string SaveFileDirectory = $"{Application.persistentDataPath}/Data/";
        public static readonly string SaveFilePath = Path.Combine(SaveFileDirectory, SaveFileName);
    }
}