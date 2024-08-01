using Infrastructure.Service;

namespace Content.LoadingCurtain.Scripts.Controller
{
    public interface ILoadingCurtainController : IInitiable
    {
        public void Show();
        public void Hide();
    }
}