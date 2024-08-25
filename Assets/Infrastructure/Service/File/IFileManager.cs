namespace Infrastructure.Service.File
{
    public interface IFileManager
    {
        public object Load(string filePath);
        public void Save(string filePath, object fileContent);
    }
}