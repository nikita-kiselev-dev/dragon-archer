namespace Infrastructure.Service.File
{
    public interface IFileService
    {
        public T Load<T>(string filePath);
        public void Save<T>(string filePath, object fileContent);
    }
}