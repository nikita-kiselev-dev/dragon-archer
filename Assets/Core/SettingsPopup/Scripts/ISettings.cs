namespace Core.SettingsPopup.Scripts
{
    public interface ISettings
    {
        bool IsInited { get; }
        void OpenPopup();
    }
}