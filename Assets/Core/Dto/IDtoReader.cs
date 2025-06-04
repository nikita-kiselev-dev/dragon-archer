using Cysharp.Threading.Tasks;

namespace Core.Dto
{
    public interface IDtoReader
    {
        public UniTask<T> Read<T>(string configName);
    }
}