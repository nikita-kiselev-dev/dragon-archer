namespace Core.File
{
    public class ByteArrayFileManager : IFileManager
    {
        public object Load(string filePath)
        {
            var loadedObject = System.IO.File.Exists(filePath) ? System.IO.File.ReadAllBytes(filePath) : null;
            return loadedObject;
        }

        public void Save(string filePath, object fileContent)
        {
            System.IO.File.WriteAllBytes(filePath, (byte[])fileContent);
        }
    }
}