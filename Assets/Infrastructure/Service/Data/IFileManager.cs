namespace Infrastructure.Service.Data
{
    public interface IFileManager
    {
        public object Load(string filePath);
        public void Save(string filePath, object fileContent);
    }
}