using System.IO;
using UnityEngine;

namespace Infrastructure.Service.SaveLoad
{
    public static class SaveLoadInfo
    {
        public const string SaveFileName = "SaveFile.bin";
        public static readonly string SaveFileDirectory = $"{Application.persistentDataPath}/Data/";
        public static readonly string SaveFilePath = Path.Combine(SaveFileDirectory, SaveFileName);
    }
}