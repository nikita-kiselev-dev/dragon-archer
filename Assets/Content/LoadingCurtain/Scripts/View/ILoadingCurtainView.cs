using Infrastructure.Service.View;

namespace Content.LoadingCurtain.Scripts.View
{
    public interface ILoadingCurtainView : IMonoBehaviour
    {
        public void SetLoadingText(string text);
    }
}