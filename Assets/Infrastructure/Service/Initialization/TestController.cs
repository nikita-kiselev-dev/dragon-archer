using Cysharp.Threading.Tasks;

namespace Infrastructure.Service.Initialization
{
    public class TestController : ControlEntity
    {
        protected override async UniTask Init()
        {
            await UniTask.CompletedTask;
        }
    }
}