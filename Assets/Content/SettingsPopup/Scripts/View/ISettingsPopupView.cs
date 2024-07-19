using Infrastructure.Service.View.ViewManager;

namespace Content.SettingsPopup.Scripts.View
{
    public interface ISettingsPopupView : IView
    {
        public void SetSoundsSliderValue(float value);
        public void SetMusicSliderValue(float value);
    }
}