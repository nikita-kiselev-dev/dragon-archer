using Infrastructure.Service.View.ViewManager;

namespace Content.SettingsPopup.Scripts.View
{
    public abstract class ISettingsPopupView : MonoView
    {
        public abstract void SetSoundsSliderValue(float value);
        public abstract void SetMusicSliderValue(float value);
    }
}