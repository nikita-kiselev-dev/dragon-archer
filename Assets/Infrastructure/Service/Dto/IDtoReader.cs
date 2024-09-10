using Cysharp.Threading.Tasks;

namespace Infrastructure.Service.Dto
{
    public interface IDtoReader
    {
        public UniTask<T> Read<T>(string configName);
    }
}