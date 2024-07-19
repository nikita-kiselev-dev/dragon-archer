using System.IO;

namespace Infrastructure.Service.Data
{
    public class StringFileManager : IFileManager
    {
        public object Load(string filePath)
        {
            return File.Exists(filePath) ? File.ReadAllText(filePath) : null;
        }
        
        public void Save(string filePath, object fileContent)
        {
            using (var streamWriter = new StreamWriter(filePath, false))
            {
                streamWriter.WriteLine(fileContent);
            }
        }
    }
}