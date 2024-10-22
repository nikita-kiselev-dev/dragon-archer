namespace Content.Settings.Scripts.Presenter
{
    public interface ISettingsPopupPresenter
    {
        public bool IsInited { get; }
        public void Init();
        public void Open();
        public void Close();
    }
}