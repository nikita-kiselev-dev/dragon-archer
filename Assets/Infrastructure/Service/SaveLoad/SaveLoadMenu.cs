using System.IO;
using UnityEditor;

namespace Infrastructure.Service.SaveLoad
{
    public class SaveLoadMenu
    {
        [MenuItem("Raycast Productions/Open Save Folder")]
        private static void OpenSaveFolder()
        {
            if (!Directory.Exists(SaveLoadInfo.SaveFileDirectory))
            {
                return;
            }
            
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
        }
    }
}