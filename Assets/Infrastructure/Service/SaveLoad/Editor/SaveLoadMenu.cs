using System.IO;
using UnityEditor;
using UnityEngine;

namespace Infrastructure.Service.SaveLoad.Editor
{
    public class SaveLoadMenu
    {
        [MenuItem("Raycast Productions/Open Save Folder")]
        private static void OpenSaveFolder()
        {
            CreateDirectory();
            EditorUtility.RevealInFinder(SaveLoadInfo.SaveFileDirectory);
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