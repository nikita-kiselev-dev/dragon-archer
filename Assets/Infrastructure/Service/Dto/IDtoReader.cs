namespace Infrastructure.Service.Dto
{
    public interface IDtoReader
    {
        public T Read<T>(string configName);
    }
}