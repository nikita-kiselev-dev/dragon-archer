namespace Content.SettingsPopup.Scripts
{
    public interface ISettings
    {
        bool IsInited { get; }
        void OpenPopup();
    }
}