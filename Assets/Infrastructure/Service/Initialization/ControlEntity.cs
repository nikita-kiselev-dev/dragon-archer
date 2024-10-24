using Cysharp.Threading.Tasks;

namespace Infrastructure.Service.Initialization
{
    public abstract class ControlEntity
    {
        public UniTask LoadPhase()
        {
            return Load();
        }

        public UniTask InitPhase()
        {
            return Init();
        }
        
        public UniTask PostInitPhase()
        {
            return PostInit();
        }

        protected virtual UniTask Load()
        {
            return UniTask.CompletedTask;
        }

        protected virtual UniTask Init()
        {
            return UniTask.CompletedTask;
        }

        protected virtual UniTask PostInit()
        {
            return UniTask.CompletedTask;
        }
    }
}