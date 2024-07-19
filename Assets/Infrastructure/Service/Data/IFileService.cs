namespace Infrastructure.Service.Data
{
    public interface IFileService
    {
        public object Load<T>(string filePath);
        public void Save<T>(string filePath, object fileContent);
    }
}