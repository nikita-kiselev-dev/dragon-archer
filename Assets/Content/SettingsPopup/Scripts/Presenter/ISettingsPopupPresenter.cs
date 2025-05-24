namespace Content.SettingsPopup.Scripts.Presenter
{
    public interface ISettingsPopupPresenter
    {
        public bool IsInited { get; }
        public void Init();
        public void Open();
    }
}