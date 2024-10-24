using System.IO;
using Infrastructure.Service.Logger;
using UnityEditor;
using UnityEngine;

namespace Infrastructure.Service.SaveLoad.Editor
{
    public class SaveLoadMenu
    {
        private static readonly ILogManager _logger = new LogManager(nameof(SaveLoadMenu));
        
        [MenuItem("Raycast Productions/Open Save Folder")]
        private static void OpenSaveFolder()
        {
            CreateDirectory();
            EditorUtility.RevealInFinder(SaveLoadInfo.SaveFileDirectory);
            _logger.Log("Save Folder Opened.");
        }
        
        [MenuItem("Raycast Productions/Delete Save and Configs")]
        private static void DeleteSaveAndConfigs()
        {
            if (!Directory.Exists(SaveLoadInfo.SaveFileDirectory))
            {
                return;
            }

            var filePaths = Directory.GetFiles(SaveLoadInfo.SaveFileDirectory);
            
            foreach (var filePath in filePaths)
            {
                System.IO.File.Delete(filePath);
            }
            
            PlayerPrefs.DeleteAll();
            
            _logger.Log("Save and Configs are deleted.");
        }

        private static void CreateDirectory()
        {
            if (!Directory.Exists(SaveLoadInfo.SaveFileDirectory))
            {
                Directory.CreateDirectory(SaveLoadInfo.SaveFileDirectory);
            }
        }
    }
}