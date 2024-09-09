using Cysharp.Threading.Tasks;

namespace Content.LoadingCurtain.Scripts.Controller
{
    public interface ILoadingCurtainController
    {
        public UniTaskVoid Init();
        public void Show();
        public UniTaskVoid Hide();
    }
}