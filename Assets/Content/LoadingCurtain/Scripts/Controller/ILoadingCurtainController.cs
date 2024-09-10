using Cysharp.Threading.Tasks;

namespace Content.LoadingCurtain.Scripts.Controller
{
    public interface ILoadingCurtainController
    {
        public UniTask Init();
        public void Show();
        public UniTaskVoid Hide();
    }
}