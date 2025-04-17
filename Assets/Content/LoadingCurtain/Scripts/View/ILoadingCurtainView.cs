namespace Content.LoadingCurtain.Scripts.View
{
    public abstract class ILoadingCurtainView : Infrastructure.Service.View.ViewManager.View
    {
        public abstract void SetLoadingText(string text);
    }
}